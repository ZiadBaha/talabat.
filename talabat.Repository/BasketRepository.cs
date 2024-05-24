using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using talabat.core.Entites;
using talabat.core.Repositories;

namespace talabat.Repository
{
    public class BasketRepository : IBasketRepositries
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        { 
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsynk(string basketid)
        {
            return await _database.KeyDeleteAsync(basketid);
        }

        public async Task<CustomerBasket?> GetBasketAsynk(string basketid)
        {
            var basket = await _database.StringGetAsync(basketid);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UbdateBasketAsynk(CustomerBasket basket)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var createdorubdated = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(1));
            if (!createdorubdated) return null;
            return await GetBasketAsynk(basket.Id);

        }
    }
}
