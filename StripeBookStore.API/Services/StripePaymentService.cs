using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using StripeBookStore.Shared.Configuration;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models.DTOs;
using System.Linq;
using StripeBookStore.Shared.Constants;

namespace StripeBookStore.API.Services
{
    public class StripeApiPaymentService : IPaymentService
    {
        private readonly IStripeClient _client;
        private readonly IOptions<StripeOptions> _options;

        public StripeApiPaymentService(IOptions<StripeOptions> options, IStripeClient client)
        {
            _client = client;
            _options = options;
        }

        public async Task<CreatePaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request)
        {
            //Get product price from in Memory Collection
            var productPriceAmount = GetProductPriceFromMemory(request);

            //Get product price from in Stripe Account
            //var productPriceAmount = GetProductPriceFromStripe(request);

            //Apply 8.25 Sales Tax to product
            long orderTaxAmount = (long)((decimal)productPriceAmount / 100 * (decimal)0.0825 * 100);
            long orderTotal = (long)(productPriceAmount + orderTaxAmount);

            var options = new PaymentIntentCreateOptions
            {
                Amount = orderTotal,
                Currency = "usd",
                Customer = request.Customer,
            };

            var paymentService = new PaymentIntentService(_client);
            PaymentIntent paymentIntent = await paymentService.CreateAsync(options);

            return new CreatePaymentIntentResponse{ Id = paymentIntent.Id,ClientSecret = paymentIntent.ClientSecret };
        }

        long GetProductPriceFromMemory(CreatePaymentIntentRequest request)
        {
            return StripeBookStoreConstants.BooksCollection.Where(book => book.Sku.Equals(request.Sku)).FirstOrDefault().Price;
        }

        async Task<long> GetProductPriceFromStripe(CreatePaymentIntentRequest request)
        {

            StripeList<Price> prices = await GetPricesAsync();
            var productPrice = prices.Where(price => price.ProductId.Equals(request.Sku)).FirstOrDefault();

            if (productPrice == null)
                return 0;

            return (long)productPrice.UnitAmount;
        }
        public async Task<StripeList<Customer>> GetCustomersAsync(string customerId = "", string startingAfter = "", int pageSize = 25)
        {
            var options = new CustomerListOptions
            {
                Limit = pageSize,
            };

            if (!string.IsNullOrEmpty(startingAfter)) options.StartingAfter = startingAfter;

            var customerService = new CustomerService(_client);
            return await customerService.ListAsync(options);
        }

        public async Task<StripeList<PaymentIntent>> GetPaymentIntentsAsync(string intentId = "", string startingAfter = "", int pageSize = 25)
        {
            var options = new PaymentIntentListOptions
            {
                Limit = pageSize,
            };

            if (!string.IsNullOrEmpty(startingAfter)) options.StartingAfter = startingAfter;

            var paymentIntentService = new PaymentIntentService(_client);
            return await paymentIntentService.ListAsync(options);
        }

        public async Task<StripeList<Price>> GetPricesAsync(string priceId = "", string startingAfter = "", int pageSize = 25)
        {

            var options = new PriceListOptions
            {
                Limit = pageSize,
            };

            if (!string.IsNullOrEmpty(startingAfter)) options.StartingAfter = startingAfter;

            var priceService = new PriceService(_client);
            return await priceService.ListAsync(options);
        }

        public async Task<StripeList<Product>> GetProductsAsync(string productId = "", string startingAfter = "", int pageSize = 25)
        {
            var options = new ProductListOptions
            {
                Limit = pageSize,
            };

            if (!string.IsNullOrEmpty(startingAfter)) options.StartingAfter = startingAfter;

            var productService = new ProductService(_client);
            return await productService.ListAsync(options); ;
        }
    }
}
