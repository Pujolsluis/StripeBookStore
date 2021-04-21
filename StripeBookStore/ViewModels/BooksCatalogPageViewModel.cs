using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace StripeBookStore.ViewModels
{
    public class BooksCatalogPageViewModel : BaseViewModel, IDestructible
    {

        private readonly IPreferences _preferences;
        private readonly IStripeBookStoreApi _stripeBookStoreApi;

        public BooksCatalogPageViewModel(INavigationService navigationService, IPreferences preferences, IStripeBookStoreApi stripeBookStoreApi) : base(navigationService)
        {
            _preferences = preferences;
            _stripeBookStoreApi = stripeBookStoreApi;
            CustomerName = IsFirstTime ? "Authenticating..." : "Luis Pujols | @Pujolsluis";
            LoadName().SafeFireAndForget() ;
        }

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _customerName;

        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }

        bool IsFirstTime
        {
            get => _preferences.Get(nameof(IsFirstTime), true);
            set => _preferences.Set(nameof(IsFirstTime), value);
        }

        private async Task<string> LoadName()
        {
            //if (!IsFirstTime)
            //    return string.Empty;
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
            });

            var weatherForecast = await _stripeBookStoreApi.GetWeatherForecast();

            var res = await weatherForecast.Content.ReadAsStringAsync();

            var forecastList = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(res);

            IsFirstTime = false;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsBusy = false;
            });

            return CustomerName = "Luis Pujols | @Pujolsluis";
        }

        public void Destroy()
        {
        }
    }

    public class WeatherForecast
    {
        public DateTime date { get; set; }
        public int temperatureC { get; set; }
        public int temperatureF { get; set; }
        public string summary { get; set; }
    }
}
