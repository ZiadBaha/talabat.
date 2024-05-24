using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.core.Entites;

namespace talabat.core.services
{
    public interface IPyamentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketid);
        
    }
}
