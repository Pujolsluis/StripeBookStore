using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Stripe;
using StripeBookStore.Services;
using StripeBookStore.Shared.Constants;
using StripeBookStore.Shared.Models;
using StripeBookStore.Shared.Models.DTOs;
using StripeBookStore.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;
using Card = StripeBookStore.Shared.Models.Card;

namespace StripeBookStore.ViewModels
{
    public class CheckoutPageViewModel : BaseViewModel, IInitialize, INavigationAware, IDestructible
    {
        readonly IPreferences _preferences;
        readonly IApiManager _apiManager;
        readonly IMainThread _mainThread;
        readonly HubConnection hubConnection;
        CreatePaymentIntentResponse _paymentIntent;
        Card _cardPaymentMethod;
        TaskCompletionSource<PaymentEvent> _paymentChargeEventCompletedTcs;

        public CheckoutPageViewModel(INavigationService navigationService, IPreferences preferences, IMainThread mainThread, IApiManager apiManager) : base(navigationService)
        {
            _apiManager = apiManager;
            _preferences = preferences;
            _mainThread = mainThread;

            PageTitle = "Checkout";

            OnSelectPaymentMethodCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync(NavigationConstants.AddPaymentMethod);
            });

            OnConfirmPaymentCommand = new DelegateCommand(() => ConfirmPaymentIntent(_paymentIntent, _cardPaymentMethod).SafeFireAndForget());

            hubConnection = new HubConnectionBuilder().WithUrl(StripeBookStoreConstants.PaymentEventsHubUrl).Build();
            hubConnection.On<PaymentEvent>(StripeBookStoreConstants.SendPaymentEventsHubResponse, OnReceivedPaymentEvent);
        }

        public DelegateCommand OnSelectPaymentMethodCommand { get; set; }
        public DelegateCommand OnConfirmPaymentCommand { get; set; }

        string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        Book _book;
        public Book Book
        {
            get => _book;
            set => SetProperty(ref _book, value);
        }

        long _orderSubTotalAmount;
        public long OrderSubTotalAmount
        {
            get => _orderSubTotalAmount;
            set => SetProperty(ref _orderSubTotalAmount, value, onOrderSubTotalChanged);
        }

        long _orderTaxAmount;
        public long OrderTaxAmount
        {
            get => _orderTaxAmount;
            set => SetProperty(ref _orderTaxAmount, value);
        }

        long _orderTotalAmount;
        public long OrderTotalAmount
        {
            get => _orderTotalAmount;
            set => SetProperty(ref _orderTotalAmount, value);
        }

        void onOrderSubTotalChanged()
        {
            OrderTaxAmount = (long)(((decimal)OrderSubTotalAmount / 100 * (decimal)(0.0825)) * 100);
            OrderTotalAmount = OrderSubTotalAmount + OrderTaxAmount;
            OnPropertyChanged(nameof(PayAmount));
        }

        string _payAmount;
        public string PayAmount
        {
            get => $"Pay ${(decimal)_orderTotalAmount / 100: 0.00}";
            set => SetProperty(ref _payAmount, value);
        }

        string _paymentMethod;
        public string PaymentMethod
        {
            get => string.IsNullOrEmpty(_paymentMethod) ? "+ Add" : _paymentMethod;
            set => SetProperty(ref _paymentMethod, value);
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(NavigationDataConstants.Book))
            {
                Book = parameters.GetValue<Book>(NavigationDataConstants.Book);
                OrderSubTotalAmount = Book.Price;

                InitializeCheckoutPaymentIntent(Book).SafeFireAndForget(); ;
            }
        }

        async Task ConnectPaymentsHub()
        {
            await hubConnection.StartAsync();
        }

        async Task DisconnectPaymentsHub()
        {
            await hubConnection.StopAsync();
        }

        private async Task<CreatePaymentIntentResponse> InitializeCheckoutPaymentIntent(Book book)
        {
            await ConnectPaymentsHub();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                ErrorInitializing = false;
                IsBusy = true;
            });

            CreatePaymentIntentRequest requestPaymentIntent = new CreatePaymentIntentRequest(Book.Sku);

            HttpResponseMessage? postPaymentIntentResponse = null;
            CreatePaymentIntentResponse paymentIntent = new CreatePaymentIntentResponse();
            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                postPaymentIntentResponse = await _apiManager.PostPaymentIntent(requestPaymentIntent, cts);
                var rawPostPaymentIntentResponse = await postPaymentIntentResponse.Content.ReadAsStringAsync();

                if (postPaymentIntentResponse.IsSuccessStatusCode)
                {
                   paymentIntent = JsonConvert.DeserializeObject<CreatePaymentIntentResponse>(rawPostPaymentIntentResponse);
                    _paymentIntent = paymentIntent;
                }
            }
            catch (Exception ex)
            {
                //TODO: Improvements - Add AppCenter Crash Analytics
                Debug.WriteLine(ex);
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ErrorInitializing = postPaymentIntentResponse == null;
                    IsBusy = false;
                });
            }

            return paymentIntent;
        }

        private async Task ConfirmPaymentIntent(CreatePaymentIntentResponse paymentIntent, Card cardPaymentMethod)
        {
            await _mainThread.InvokeOnMainThreadAsync(() => IsBusy = true);

            string error = string.Empty;

            try
            {
                if (_preferences.ContainsKey(StripeBookStoreConstants.SettingPublishableKey) && _paymentIntent != null && cardPaymentMethod != null)
                {
                    var paymentIntentService = new PaymentIntentService(new StripeClient(_preferences.Get(StripeBookStoreConstants.SettingPublishableKey, string.Empty)));

                    var paymentConfirmOptions = new PaymentIntentConfirmOptions()
                    {
                        ClientSecret = paymentIntent.ClientSecret,
                        Expand = new List<String> { "payment_method" },
                        PaymentMethod = cardPaymentMethod.Id,
                        UseStripeSdk = true,
                        ReturnUrl = "payments-example://stripe-redirect"
                    };
                    var confirmIntent = await paymentIntentService.ConfirmAsync(paymentIntent.Id, paymentConfirmOptions);

                    //Payment Succeeded
                    if (confirmIntent.Status.Equals("succeeded"))
                    {
                        _paymentChargeEventCompletedTcs = new TaskCompletionSource<PaymentEvent>();

                        //Await for Charge Confirmation from Server
                        var paymentChargeEvent = await _paymentChargeEventCompletedTcs.Task.ConfigureAwait(false);
                    }
                    else
                    {
                        //Log Payment confirmation error
                        error = confirmIntent.Status;

                        if (Xamarin.Forms.Application.Current?.MainPage is Xamarin.Forms.Page mainPage)
                            await _mainThread.InvokeOnMainThreadAsync(async () => await mainPage.DisplayAlert("Error", $"Payment did not succeeded, it currently has status {confirmIntent.Status}", "OK")).ConfigureAwait(false);
                    }
                }
            }
            catch(StripeException ex)
            {
                //TODO: Improvements - Add AppCenter Crash Analytics
                error = ex.Message;

                if (Xamarin.Forms.Application.Current?.MainPage is Xamarin.Forms.Page mainPage)
                    await _mainThread.InvokeOnMainThreadAsync(async () => await mainPage.DisplayAlert("Error", $"{ex.Message}", "OK")).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                //TODO: Improvements - Add AppCenter Crash Analytics
                error = ex.Message;
                Debug.WriteLine(ex);
            }
            finally
            {
                _paymentChargeEventCompletedTcs = null;

                if(!string.IsNullOrEmpty(error))
                    await _mainThread.InvokeOnMainThreadAsync(() => IsBusy = false);
            }
        }

        async Task OnReceivedPaymentEvent(PaymentEvent paymentEvent)
        {
            Debug.WriteLine($"Recived PaymentEvent with Id: {paymentEvent.Id} and amount {paymentEvent.Amount}");

            await _mainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = false;

                decimal decimalAmount = (decimal)paymentEvent.Amount / 100;
                string chargeId = paymentEvent.Id;

                if (Xamarin.Forms.Application.Current?.MainPage is Xamarin.Forms.Page mainPage)
                    await mainPage.DisplayAlert("Success", $"Your payment of ${decimalAmount} with Charge Id {chargeId}, is now completed. Thank you for shopping with us!", "OK");

                await NavigationService.NavigateAsync($"/{NavigationConstants.BooksCatalog}");
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.ContainsKey(NavigationDataConstants.Card))
            {
                _cardPaymentMethod = parameters.GetValue<Card>(NavigationDataConstants.Card);
                PaymentMethod = $"ending on {_cardPaymentMethod.Number.Substring(_cardPaymentMethod.Number.Length - 4)}";
            }
        }

        public void Destroy()
        {
            hubConnection.DisposeAsync().SafeFireAndForget();
        }
    }
}
