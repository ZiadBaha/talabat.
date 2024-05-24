using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using talabat.Apis.Dtos;
using talabat.Apis.Errors;
using talabat.core.Entites;
using talabat.core.services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace talabat.Apis.Controllers
{

    public class PaymentsController : ApiBaseController
    {
        private readonly IPyamentService _pyamentService;

        public PaymentsController(IPyamentService pyamentService)
        {
            _pyamentService = pyamentService;
        }

        [Authorize]
        [HttpPost("{basketid}")]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketid)
        {
           var basket =  await _pyamentService.CreateOrUpdatePaymentIntent(basketid);
            if (basket is null) return NotFound(new ApiResponse(404 , "There Is Aproblem With Your Basket"));
            return Ok(basket);
       
        }

    }
}
