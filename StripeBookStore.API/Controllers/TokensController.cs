using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StripeBookStore.Shared.Configuration;
using StripeBookStore.Shared.Models.DTOs;

namespace StripeBookStore.API.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class TokensController : Controller
    {
        private readonly ILogger<TokensController> _logger;
        private readonly IOptions<StripeOptions> _options;

        public TokensController(ILogger<TokensController> logger, IOptions<StripeOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        /// <summary>
        /// Returns Stripe Public API Key
        /// </summary>
        /// <returns>Stripe Public API Key</returns>
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
    }
}
