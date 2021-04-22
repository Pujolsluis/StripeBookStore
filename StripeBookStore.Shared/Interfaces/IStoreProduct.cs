using System;
using System.ComponentModel.DataAnnotations;

namespace StripeBookStore.Shared.Interfaces
{
    public interface IStoreProduct
    {
        [Required]
        public string Sku { get; }
        public string Name { get; }
    }
}
