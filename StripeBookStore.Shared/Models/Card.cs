using System;
namespace StripeBookStore.Shared.Models
{
    public class Card
    {
        public string Number { get; set; }
        public string CVV { get; set; }
        public string ExpirationDate { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
    }
}
