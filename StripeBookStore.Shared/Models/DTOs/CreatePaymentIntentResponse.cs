using System;
namespace StripeBookStore.Shared.Models.DTOs
{
    public class CreatePaymentIntentResponse
    {
        public string ClientSecret { get; set; }
        public string Id { get; set; }
    }
}
