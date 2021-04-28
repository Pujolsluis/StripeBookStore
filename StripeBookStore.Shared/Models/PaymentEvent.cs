using System;
namespace StripeBookStore.Shared.Models
{
    public class PaymentEvent
    {
        public string Id { get; set; }
        public long Amount { get; set; }
    }
}
