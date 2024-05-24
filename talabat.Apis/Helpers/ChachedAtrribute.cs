using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing.Printing;
using System.Text;
using System.Xml.Linq;
using talabat.core.services;

namespace talabat.Apis.Helpers
{
    public class ChachedAtrribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSec;

        public ChachedAtrribute(int TimeToLiveInSec )
        {
            _timeToLiveInSec = TimeToLiveInSec;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>();

            var ChacheKey = GenerateChacheKeyFromRequest(context.HttpContext.Request);

            var CachedResponse = await CacheService.GetCashedResponseAsync(ChacheKey);
            if(!string.IsNullOrEmpty(CachedResponse))
            {
                var ContentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = ContentResult;
                return;
            }

            var ExcutedEndPointContext = await next();
            if( ExcutedEndPointContext.Result is OkObjectResult okObjResult )
            {
                await CacheService.CashResponseAsync( ChacheKey , okObjResult.Value , TimeSpan.FromSeconds(_timeToLiveInSec));
            }
        }

        private string GenerateChacheKeyFromRequest(HttpRequest request)
        {
            // /api/products?pageindex =1&pagesize=5&sort=name
            var KeyBuilder = new StringBuilder();

            KeyBuilder.Append(request.Path); // api/products 
            foreach( var (key , value ) in request.Query.OrderBy(Z=>Z.Key))
            {
                //pageindex = 1 
                //pagesize = 5 
                //sort = name

                KeyBuilder.Append($"|{key}-{value}");

            }
            // /api/products|pageindex=1|pagesize=5|sort=name
             
            return KeyBuilder.ToString();

        }
    }
}
