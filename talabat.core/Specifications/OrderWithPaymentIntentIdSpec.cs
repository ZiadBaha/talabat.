using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.core.Entites.Order_Aggregate;

namespace talabat.core.Specifications
{
    public class OrderWithPaymentIntentIdSpec : baseSpecification<Order>
    {
        public OrderWithPaymentIntentIdSpec(string paymentintentid) : base(O => O.PayemnyIntentId == paymentintentid)
        {
                
        }
    }
}
