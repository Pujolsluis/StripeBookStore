using System;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace StripeBookStore.Shared.Interfaces
{
    [Headers("User-Agent: " + nameof(StripeBookStore), "Accept-Encoding: gzip", "Accept: application/json")]
    public interface IStripeBookStoreApi
    {
        [Get("/weatherforecast")]
        Task<HttpResponseMessage> GetWeatherForecast();
    }
}
