using System;
using StripeBookStore.Shared.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StripeBookStore.Shared.Models;
using Stripe;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace StripeBookStore.API.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IOptions<StripeOptions> _options;
        private readonly IStripeClient _client;

        public PaymentsController(ILogger<PaymentsController> logger, IOptions<StripeOptions> options)
        {
            this._logger = logger;
            this._options = options;
            _client = new StripeClient(_options.Value.SecretKey);
        }

        /// <summary>
        /// Provides Stripe API Keys
        /// </summary>
        /// <returns></returns>
        [HttpGet("public-keys")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublicKeyResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PublicKeyResponse> GetPublicKeys()
        {
            _logger.LogDebug($"Publishable Key Requested by Client");

            if (string.IsNullOrEmpty(_options.Value.PublishableKey))
                return NotFound();

            return new PublicKeyResponse
            {
                PublicKey = _options.Value.PublishableKey,
            };
        }

        /// <summary>
        /// Returns the List of Customers associated to our Stripe Account
        /// </summary>
        /// <returns></returns>
        [HttpGet("customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StripeList<Customer>>> GetAllCustomers()
        {
            _logger.LogDebug($"GetAllCustomers Requested by Client");
            var options = new CustomerListOptions
            {
                Limit = 100,
            };

            var customerService = new CustomerService(_client);
            return await customerService.ListAsync(options);
        }

        /// <summary>
        /// Returns the List of Products
        /// </summary>
        /// <returns></returns>
        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StripeList<Product>>> GetAllProducts()
        {
            _logger.LogDebug($"GetAllProducts Requested by Client");
            
            var options = new ProductListOptions
            {
                Limit = 100,
            };

            var productService = new ProductService(_client);
            return await productService.ListAsync(options);
        }

        /// <summary>
        /// Return the list of Payment Intents
        /// </summary>
        /// <returns></returns>
        [HttpGet("payment-intents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StripeList<PaymentIntent>>> GetAllPaymentIntents()
        {
            _logger.LogDebug($"GetAllPaymentIntents Requested by Client");

            var options = new PaymentIntentListOptions
            {
                Limit = 10,
            };

            var paymentIntentService = new PaymentIntentService(_client);
            return await paymentIntentService.ListAsync(options);
        }

        
        private async Task<ActionResult<StripeList<Price>>> GetAllPrices()
        {
            _logger.LogDebug($"GetAllPaymentIntents Requested by Client");

            var options = new PriceListOptions
            {
                Limit = 10,
            };

            var priceService = new PriceService(_client);
            return await priceService.ListAsync(options);
        }

        /// <summary>
        /// Creates a Payment Intent for one of our Products
        /// </summary>
        /// <param name="bookPaymentIntent"></param>
        /// <returns>Client secret associated to the newly created payment intent</returns>
        /// <response code="200">Returns Payment Intent client secret</response>
        /// <response code="400">If item is null or has an invalid state</response>
        /// <response code="404">If associated price for product in Payment Intent is not found</response>
        [HttpPost("payment-intent")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> CreateProductPaymentIntent([FromBody] BookStorePaymentIntent bookPaymentIntent)
        {
            _logger.LogDebug($"PostBookPaymentIntent for Book Id {bookPaymentIntent.Sku}");

            if (bookPaymentIntent == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var prices = await GetAllPrices();
            var productPrice = prices.Value.Data.Where(book => book.ProductId.Equals(bookPaymentIntent.Sku)).FirstOrDefault();

            if (productPrice == null)
                return NotFound();

            var options = new PaymentIntentCreateOptions
            {
                Amount = productPrice.UnitAmount,
                Currency = "usd"
            };

            var paymentService = new PaymentIntentService(_client);
            PaymentIntent paymentIntent = await paymentService.CreateAsync(options);

            return paymentIntent.ClientSecret;
        }
    }

    
}
