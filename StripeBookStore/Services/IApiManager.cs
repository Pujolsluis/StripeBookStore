using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using StripeBookStore.Shared.Models.DTOs;

namespace StripeBookStore.Services
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> GetStripePublicKeys(CancellationTokenSource cts);

        Task<HttpResponseMessage> GetCustomers(CancellationTokenSource cts, string startingAfter = "", int pageSize = 25);

        Task<HttpResponseMessage> GetProducts(CancellationTokenSource cts, string startingAfter = "", int pageSize = 25);

        Task<HttpResponseMessage> GetPaymentIntents(CancellationTokenSource cts, string startingAfter = "", int pageSize = 25);

        Task<HttpResponseMessage> PostPaymentIntent(PaymentRequest request, CancellationTokenSource cts);
    }
}
