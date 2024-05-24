using talabat.core.Entites.Order_Aggregate;

namespace talabat.Apis.Dtos
{
    public class OrderDto
    {
        
            public string BasketId { get; set; }
            public int DeliveryMethod { get; set; }
            public OrderAddressDto ShipingAddress { get; set; }

            
    }
}
