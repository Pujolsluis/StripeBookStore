using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using StripeBookStore.Shared.Configuration;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models.DTOs;

namespace StripeBookStore.API.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IOptions<StripeOptions> _options;
        private readonly IPaymentService _stripePaymentService;

        public CustomersController(ILogger<CustomersController> logger, IOptions<StripeOptions> options,
                                   IPaymentService stripePaymentService)
        {
            _logger = logger;
            _options = options;
            _stripePaymentService = stripePaymentService;
        }

        /// <summary>
        /// Returns List of Customers associated to our Stripe Bookstore Account
        /// </summary>
        /// <param name="startingAfter"></param>
        /// <param name="pageSize"></param>
        /// <returns>List of customers</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StripeList<Customer>>> GetCustomers([FromQuery]string startingAfter, [FromQuery]int pageSize)
        {
            _logger.LogDebug($"GetAllCustomers Requested by Client");

            return await _stripePaymentService.GetCustomersAsync(startingAfter: startingAfter, pageSize: pageSize);
        }
    }
}
