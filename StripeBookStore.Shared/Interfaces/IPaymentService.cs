using System;
using System.Threading.Tasks;
using Stripe;
using StripeBookStore.Shared.Models.DTOs;

namespace StripeBookStore.Shared.Interfaces
{
    public interface IPaymentService
    {
        public Task<CreatePaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request);
        public Task<StripeList<PaymentIntent>> GetPaymentIntentsAsync(string intentId = "", string startingAfter = "", int pageSize = 25);
        public Task<StripeList<Price>> GetPricesAsync(string priceId = "", string startingAfter = "", int pageSize = 25);
        public Task<StripeList<Product>> GetProductsAsync(string productId = "", string startingAfter = "", int pageSize = 25);
        public Task<StripeList<Customer>> GetCustomersAsync(string customerId = "", string startingAfter = "", int pageSize = 25);
    }
}
