using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.core;
using talabat.core.Entites;
using talabat.core.Entites.Order_Aggregate;
using talabat.core.Repositories;
using talabat.core.services;
using talabat.core.Specifications;

namespace talabat.service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepositries _basketRepositries;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPyamentService _pyamentService;

        ///private readonly iGenericRepository<Product> _productRepo;
        ///private readonly iGenericRepository<DeliveryMethod> _deliveryMethod;
        ///private readonly iGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepositries basketRepositries,IUnitOfWork unitOfWork , IPyamentService pyamentService
                            ///iGenericRepository<Product> ProductRepo,
                            ///iGenericRepository<DeliveryMethod> deliveryMethod,
                            ///iGenericRepository<Order> OrderRepo
                            )
        {
            _basketRepositries = basketRepositries;
            _unitOfWork = unitOfWork;
            _pyamentService = pyamentService;
            ///_productRepo = ProductRepo;
            ///_deliveryMethod = deliveryMethod;
            ///_orderRepo = OrderRepo;
        }


        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            // 1. Get basket From Basket Repo
            var Basket = await _basketRepositries.GetBasketAsynk(BasketId);

            // 2. Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();

            if (Basket?.Items?.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.repository<Product>().GetbyidAsync(item.id);
                    var ProductItemOrdered = new PrductItemOrder(Product.id , Product.Name , Product.PictureUrl);
                    var OrderdItem = new OrderItem(ProductItemOrdered , item.Quantity , Product.Price);
                    OrderItems.Add(OrderdItem);

                }
            }
            
            // 3. Calculate SubTotal
            var SubTotal = OrderItems.Sum(x => x.Price * x.Quantity);
            
            // 4. Get Delivery Method From Delivery Method Repo
            
            var deleverymethod = await _unitOfWork.repository<DeliveryMethod>().GetbyidAsync(DeliveryMethodId);

            // 5. Create Order 

            var spec = new OrderWithPaymentIntentIdSpec(Basket.PaymentIntentId);
            var ExistingOrder = await _unitOfWork.repository<Order>().GetEntitywithspecasync(spec);
            if(ExistingOrder is not null)
            {
                _unitOfWork.repository<Order>().Delete(ExistingOrder);

                await _pyamentService.CreateOrUpdatePaymentIntent(BasketId);
            } 


            var order = new Order(BuyerEmail , ShippingAddress , deleverymethod , OrderItems , SubTotal  ,Basket.PaymentIntentId);
            await _unitOfWork.repository<Order>().add(order);

            // 6. Save To Database [TODO]

            var result =  await _unitOfWork.Complete();
            if (result <= 0) return null;
            return order;

        }

        public async Task<Order> CreateOrderByIdForUserAsync(string BuyerEmail, int OrderId)
        {
            var spec = new OrderSpacifications(OrderId, BuyerEmail);
            var order = await _unitOfWork.repository<Order>().GetEntitywithspecasync(spec);
            return order;

        }

        public async Task<IReadOnlyList<Order>> CreateOrdersForUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpacifications(BuyerEmail);
            var Orders = await _unitOfWork.repository<Order>().Getallwithspecasync(spec);
            return Orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsyn()
        {
            var DeliveryMethods = await _unitOfWork.repository<DeliveryMethod>().Getallasync();
            return DeliveryMethods;
        }
    }
}
