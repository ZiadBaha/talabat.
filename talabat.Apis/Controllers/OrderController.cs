using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using talabat.Apis.Dtos;
using talabat.Apis.Errors;
using talabat.core.Entites.Order_Aggregate;
using talabat.core.services;

namespace talabat.Apis.Controllers
{
    [Authorize]
    public class OrderController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController( IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // improving for swagger
        [ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof (ApiResponse) , StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var address = _mapper.Map<OrderAddressDto, Address>(orderDto.ShipingAddress);
            var order =  await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethod , address );
            if(order is null)  return BadRequest(new ApiResponse(400));
            return Ok(order);
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await  _orderService.CreateOrdersForUserAsync(buyerEmail);
            return Ok(Orders);
        }




        [ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrdersForUser(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
             var order = await _orderService.CreateOrderByIdForUserAsync(email , id);
            if(order is null) return NotFound(new ApiResponse(404));
            return Ok(order);
        }


        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _orderService.GetDeliveryMethodAsyn();
            return Ok(DeliveryMethods);
        }
    }
}
 