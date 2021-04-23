using System;

namespace StripeBookStore.Shared.Interfaces
{
    public interface IStoreProduct
    {
        public string Sku { get; }
        public string Name { get; }
    }
}
