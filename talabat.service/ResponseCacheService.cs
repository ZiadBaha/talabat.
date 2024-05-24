using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using talabat.core.services;

namespace talabat.service
{
    public class ResponseCacheService : IResponseCashService
    {
        private readonly IDatabase _database;
        public ResponseCacheService( IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CashResponseAsync(string cacheKey, object response, TimeSpan timeTolive)
        {
            if (response == null) return;
            var Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var SerializeResponse = JsonSerializer.Serialize(response , Options);
            await _database.StringSetAsync(cacheKey, SerializeResponse, timeTolive);
            
        }

        public async Task<string> GetCashedResponseAsync(string cacheKey)
        {
            var CachResponse =  await _database.StringGetAsync(cacheKey);

            if (CachResponse.IsNullOrEmpty) return null;
            return CachResponse;
            
        }
    }
}
