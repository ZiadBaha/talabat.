using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talabat.core.services
{
    public interface IResponseCashService
    {
        Task CashResponseAsync(string cacheKey, object response, TimeSpan timeTolive);
        Task<string> GetCashedResponseAsync(string cacheKey);
    }
}
