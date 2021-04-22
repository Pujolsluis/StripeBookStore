using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using StripeBookStore.Shared.Models;

namespace StripeBookStore.Shared.Interfaces
{
    [Headers("User-Agent: " + nameof(StripeBookStore), "Accept-Encoding: gzip", "Accept: application/json")]
    public interface IStripeBookStoreApi
    {
        [Get("/api/tokens/public-keys")]
        Task<HttpResponseMessage> GetStripePublicKeys(string startingAfter, int pageSize);

        [Get("/api/customers?startingAfter={startingAfter}&pageSize={pageSize}")]
        Task<HttpResponseMessage> GetCustomers(string startingAfter, int pageSize);

        [Get("/api/products?startingAfter={startingAfter}&pageSize={pageSize}")]
        Task<HttpResponseMessage> GetProducts(string startingAfter, int pageSize);

        [Get("/api/paymentIntents?startingAfter={startingAfter}&pageSize={pageSize}")]
        Task<HttpResponseMessage> GetPaymentIntents(string startingAfter, int pageSize);

        [Post("/api/paymentIntents")]
        Task<HttpResponseMessage> PostPaymentIntent([Body] PaymentRequest request);
    }
}
