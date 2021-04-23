using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Stripe;
using StripeBookStore.Shared.Constants;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models;
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

            BooksCatalog = StripeBookStoreConstants.BooksCollection;
            CustomerName = IsFirstTime ? "Authenticating..." : "Luis Pujols | @Pujolsluis";

            LoadName().SafeFireAndForget();
        }

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private ObservableCollection<Book> _booksCatalog;
        public ObservableCollection<Book> BooksCatalog
        {
            get => _booksCatalog;
            set => SetProperty(ref _booksCatalog, value);
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

            var paymentIntents = await _stripeBookStoreApi.GetPaymentIntents();

            var res = await paymentIntents.Content.ReadAsStringAsync();

            var paymentIntentList = JsonConvert.DeserializeObject<StripeList<PaymentIntent>>(res);

            var stripePaymentService = new PaymentIntentService(new StripeClient("pk_test_51Ia6gUDgG0lk5Mcc2KwPpSleSO5Md6pWi8m8qUGBnkkGtUFcLy7tjY5pxDR0VYRTNHTyaFHuM7CZgrafs3RM6uiz00uhBEPTjR"));

            var paymentIntent = paymentIntentList.Data.ToArray().GetValue(0) as PaymentIntent;

            var paymentConfirmOptions = new PaymentIntentConfirmOptions()
            {
                ClientSecret = paymentIntent.ClientSecret,
                Expand = new List<String> { "payment_method" },
                //PaymentMethod = "src_1Ia79ODgG0lk5MccsUI85V8C",
                UseStripeSdk = true,
                ReturnUrl = "payments-example://stripe-redirect"
            };

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
}
