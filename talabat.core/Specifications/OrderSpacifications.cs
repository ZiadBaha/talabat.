﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.core.Entites.Order_Aggregate;

namespace talabat.core.Specifications
{
    public class OrderSpacifications : baseSpecification<Order>
    {
        public OrderSpacifications( int Id , string email ) : base(o => o.id == Id && o.BuyerEmail == email)
        {
            Iincludes.Add(O => O.DeliveryMethod);  
            Iincludes.Add(O => O.Items);
        }
       
        public OrderSpacifications( string email) : base(o => o.BuyerEmail == email) 
        {
            Iincludes.Add(o => o.DeliveryMethod);
            Iincludes.Add(o => o.Items);

            AddOrderByDesc(o => o.OrderDate);
        }
    }
}
