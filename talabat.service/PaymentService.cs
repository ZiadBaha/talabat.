using Microsoft.Extensions.Configuration;
using Stripe;
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
using Product = talabat.core.Entites.Product;

namespace talabat.service
{
    public class PaymentService : IPyamentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepositries _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepositries BasketRepo , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = BasketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketid)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSetings:Secretkey"];

            var basket = await _basketRepo.GetBasketAsynk(basketid);
            if (basket is null) return null;

            var ShipingCost = 0M;
            if(basket.DeliveryMethodId.HasValue)
            {
                var Deliverymethod =await _unitOfWork.repository<DeliveryMethod>().GetbyidAsync(basket.DeliveryMethodId.Value);
                ShipingCost = Deliverymethod.Cost;
                basket.ShippingCost = Deliverymethod.Cost;
            }

            if(basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.repository<Product>().GetbyidAsync(item.id);
                    if(item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntent paymentIntent;
            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // create paymentintentid
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => (item.Price * 100) * item.Quantity) + (long)ShipingCost * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else  // updatepaymentintent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => (item.Price * 100) * item.Quantity) + (long)ShipingCost * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId , options);
            }
            await _basketRepo.UbdateBasketAsynk(basket);
                return basket;

        }
    }
}
