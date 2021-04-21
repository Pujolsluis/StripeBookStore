using System;
using System.ComponentModel.DataAnnotations;

namespace StripeBookStore.Shared.Models
{
    public class Book : IStoreProduct
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public long Price { get; set; }
    }

    public class BookStorePaymentIntent : Book
    {
        public BookStorePaymentIntent(string sku)
        {
            Sku = sku;
        }
    }

    public interface IStoreProduct
    {
        [Required]
        public string Sku { get; }
    }
}
