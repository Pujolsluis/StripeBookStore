using System;
namespace StripeBookStore.Shared.Models
{
    public class Card
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string CVC { get; set; }
        public string ExpirationDate { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public long? ExpMonth { get; set; }
        public long? ExpYear { get;  set; }
    }
}
