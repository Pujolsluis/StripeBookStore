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

        [HttpGet("public-keys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        public async Task<ActionResult<StripeList<Price>>> GetAllPrices()
        {
            _logger.LogDebug($"GetAllPaymentIntents Requested by Client");

            var options = new PriceListOptions
            {
                Limit = 10,
            };

            var priceService = new PriceService(_client);
            return await priceService.ListAsync(options);
        }

        [HttpPost("payment-intent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
