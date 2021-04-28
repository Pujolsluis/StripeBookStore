using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using StripeBookStore.Shared.Models.DTOs;

namespace StripeBookStore.Shared.Interfaces
{
    [Headers("User-Agent: " + nameof(StripeBookStore), "Accept-Encoding: gzip", "Accept: application/json")]
    public interface IStripeBookStoreApi
    {
        [Get("/tokens/public-keys")]
        Task<HttpResponseMessage> GetStripePublicKeys();

        [Get("/customers?startingAfter={startingAfter}&pageSize={pageSize}")]
        Task<HttpResponseMessage> GetCustomers(string startingAfter = "", int pageSize = 25);

        [Get("/products?startingAfter={startingAfter}&pageSize={pageSize}")]
        Task<HttpResponseMessage> GetProducts(string startingAfter = "", int pageSize = 25);

        [Get("/paymentIntents?startingAfter={startingAfter}&pageSize={pageSize}")]
        Task<HttpResponseMessage> GetPaymentIntents(string startingAfter = "", int pageSize = 25);

        [Post("/paymentIntents")]
        Task<HttpResponseMessage> PostPaymentIntent([Body] CreatePaymentIntentRequest request);
    }
}
