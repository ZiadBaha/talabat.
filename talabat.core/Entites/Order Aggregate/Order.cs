using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talabat.core.Entites.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
                
        }
        public Order(string buyerEmail, Address shipingAddress, DeliveryMethod deliveryMethod,
            ICollection<OrderItem> items, decimal subTotal , string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipingAddress = shipingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PayemnyIntentId = paymentIntentId; 
            
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus status { get; set; } = OrderStatus.Pending;
        public Address ShipingAddress { get; set; }

        // public int DeliveryMethodID { get; set; } //Fk
        public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [ one ]
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; } //Price Of Product * Quantity

        //[NotMapped]
        //public decimal Total { get => SubTotal + DeliveryMethod.Cost; }

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;
        public string PayemnyIntentId { get; set; } = string.Empty; // ""


    }
}
