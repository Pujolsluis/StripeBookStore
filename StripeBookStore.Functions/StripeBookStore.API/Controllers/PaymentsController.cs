using System;
using StripeBookStore.Shared.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.API.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IOptions<StripeOptions> options;

        public PaymentsController(IOptions<StripeOptions> options)
        {
            this.options = options;
        }

        [HttpGet("public-keys")]
        public ActionResult<PublicKeyResponse> GetPublicKeys()
        {
            return new PublicKeyResponse
            {
                PublicKey = this.options.Value.PublishableKey,
            };
        }
    }
}
