using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using StripeBookStore.Shared.Configuration;
using StripeBookStore.Shared.Interfaces;

namespace StripeBookStore.API.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class ProductsController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IOptions<StripeOptions> _options;
        private readonly IPaymentService _stripePaymentService;

        public ProductsController(ILogger<ProductsController> logger, IOptions<StripeOptions> options,
                                   IPaymentService stripePaymentService)
        {
            _logger = logger;
            _options = options;
            _stripePaymentService = stripePaymentService;
        }

        /// <summary>
        /// Returns List of Products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StripeList<Product>))]
        public async Task<ActionResult<StripeList<Product>>> GetProducts()
        {
            _logger.LogDebug($"GetAllProducts Requested by Client");

            return await _stripePaymentService.GetProductsAsync();
        }
    }
}
