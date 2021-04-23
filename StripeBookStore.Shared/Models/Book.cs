using StripeBookStore.Shared.Interfaces;

namespace StripeBookStore.Shared.Models
{
    public class Book : IStoreProduct
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public long Price { get; set; }
    }
}
