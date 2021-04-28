using System;
using StripeBookStore.Shared.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StripeBookStore.Shared.Models.DTOs;
using Stripe;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using StripeBookStore.Shared.Interfaces;
using Microsoft.AspNetCore.SignalR;
using StripeBookStore.API.Hubs;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.API.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class PaymentIntentsController : Controller
    {
        private readonly ILogger<PaymentIntentsController> _logger;
        private readonly IOptions<StripeOptions> _options;
        private readonly IPaymentService _stripePaymentService;
        private readonly IHubContext<PaymentsHub, IPaymentsHub> _paymentsHub;

        public PaymentIntentsController(ILogger<PaymentIntentsController> logger, IOptions<StripeOptions> options,
                                  IPaymentService stripePaymentService, IHubContext<PaymentsHub, IPaymentsHub> paymentsHub)
        {
            _logger = logger;
            _options = options;
            _stripePaymentService = stripePaymentService;
            _paymentsHub = paymentsHub;
        }

        /// <summary>
        /// Returns List of Payment Intents
        /// </summary>
        /// <returns>List of Payment Intents</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StripeList<PaymentIntent>))]
        public async Task<ActionResult<StripeList<PaymentIntent>>> GetPaymentIntents([FromQuery] string startingAfter, [FromQuery] int pageSize)
        {
            _logger.LogDebug($"GetAllPaymentIntents Requested by Client");

            return await _stripePaymentService.GetPaymentIntentsAsync(startingAfter: startingAfter, pageSize: pageSize);
        }

        /// <summary>
        /// Creates a Payment Intent for one of our Products
        /// </summary>
        /// <param name="requestPaymentIntent"></param>
        /// <returns>Client secret associated to the newly created payment intent</returns>
        /// <response code="200">Returns Payment Intent client secret</response>
        /// <response code="400">If item is null or has an invalid state</response>
        /// <response code="404">If associated price for product in Payment Intent is not found</response>
        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreatePaymentIntentResponse>> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest requestPaymentIntent)
        {
            _logger.LogDebug($"PostBookPaymentIntent for Book Id {requestPaymentIntent.Sku}");

            if (requestPaymentIntent == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paymentIntent = await _stripePaymentService.CreatePaymentIntentAsync(requestPaymentIntent);

            if (string.IsNullOrEmpty(paymentIntent.ClientSecret))
                return NotFound("Product Price not Found.");

            return paymentIntent;
        }

        /// <summary>
        /// Receives Stripe Events
        /// </summary>
        /// <response code="200">Processed Stripe Event Successfully</response>
        /// <response code="404">Could not process Stripe Event</response>
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Event stripeEvent;

            var webhookSecret = _options.Value.WebhookSecret;
            _logger.LogDebug($"Your Webhook secret is {webhookSecret}");

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret
                    );
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to Deserialize Event Data Exception: {ex.StackTrace}");
                return BadRequest();
            }

            if(stripeEvent.Type == "charge.succeeded")
            {
                var charge = stripeEvent.Data.Object as Stripe.Charge;
                //Send Payment Event to Clients
                await _paymentsHub.Clients.All.SendPaymentEvent(new PaymentEvent { Id = charge.Id, Amount = charge.Amount});
                _logger.LogDebug($"Payment completed of ${charge.Amount/100} with charge ID {charge.Id}");
            }

            return Ok();
        }

    }
}
