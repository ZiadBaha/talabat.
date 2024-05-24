using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.core.Entites;

namespace talabat.core.Repositories
{
    public interface IBasketRepositries
    {
        Task<CustomerBasket?> GetBasketAsynk(string basketid);
        Task<CustomerBasket?> UbdateBasketAsynk(CustomerBasket basket);
        Task<bool> DeleteBasketAsynk(string basketid);
    }
}
