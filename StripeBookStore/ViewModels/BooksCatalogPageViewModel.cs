using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Stripe;
using StripeBookStore.Services;
using StripeBookStore.Shared.Constants;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models;
using StripeBookStore.Shared.Models.DTOs;
using StripeBookStore.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace StripeBookStore.ViewModels
{
    public class BooksCatalogPageViewModel : BaseViewModel, IInitialize, IDestructible
    {

        private readonly IPreferences _preferences;
        private readonly IApiManager _apiManager;

        public BooksCatalogPageViewModel(INavigationService navigationService, IPreferences preferences, IApiManager apiManager) : base(navigationService)
        {
            _apiManager = apiManager;
            _preferences = preferences;

            BooksCatalog = StripeBookStoreConstants.BooksCollection;

            OnRetryInitializationCommand = new DelegateCommand(() =>
            {
                InitializeStripePublishableKey().SafeFireAndForget();
            });

            OnBuyItemCommand = new DelegateCommand<Book>(async (book) =>
            {
                await NavigationService.NavigateAsync(NavigationConstants.CheckoutPage, new NavigationParameters() { { NavigationDataConstants.Book, book } });
            });

        }

        public DelegateCommand<Book> OnBuyItemCommand { get; set; }
        public DelegateCommand OnRetryInitializationCommand { get; set; }

        private ObservableCollection<Book> _booksCatalog;
        public ObservableCollection<Book> BooksCatalog
        {
            get => _booksCatalog;
            set => SetProperty(ref _booksCatalog, value);
        }

        private string _pageTitle = "Book Catalog";
        public string PageTitle
        {
            get => _pageTitle;
        }

        bool _isBusy;
        public new bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, () =>
            {
                OnPropertyChanged(nameof(ErrorInitializing));
                OnPropertyChanged(nameof(IsCollectionViewReady));
            });
        }

        private bool _errorInitializing;
        public bool ErrorInitializing
        {
            get => _errorInitializing;
            set => SetProperty(ref _errorInitializing, value);
        }

        public bool IsCollectionViewReady
        {
            get => !IsBusy && !ErrorInitializing;
        }

        private async Task<bool> InitializeStripePublishableKey()
        {

            MainThread.BeginInvokeOnMainThread(() =>
            {
                ErrorInitializing = false;
                IsBusy = true;
            });

            HttpResponseMessage? getPublishableKeyResponse = null;
            PublicKeyResponse publicKey = new PublicKeyResponse();
            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                getPublishableKeyResponse = await _apiManager.GetStripePublicKeys(cts);
                var rawPublishableKeyResponse = await getPublishableKeyResponse.Content.ReadAsStringAsync();

                if (getPublishableKeyResponse.IsSuccessStatusCode)
                {
                    publicKey = JsonConvert.DeserializeObject<PublicKeyResponse>(rawPublishableKeyResponse);
                    if (!string.IsNullOrEmpty(publicKey.PublicKey))
                        _preferences.Set(StripeBookStoreConstants.SettingPublishableKey, publicKey.PublicKey);
                }
            }
            catch(Exception ex)
            {
                //TODO: Improvements - Add AppCenter Crash Analytics
                Debug.WriteLine(ex);
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ErrorInitializing = getPublishableKeyResponse == null;
                    IsBusy = false;
                });
            }

            return string.IsNullOrEmpty(_preferences.Get(StripeBookStoreConstants.SettingPublishableKey, string.Empty));
        }

        public void Destroy()
        {
        }

        public void Initialize(INavigationParameters parameters)
        {
            InitializeStripePublishableKey().SafeFireAndForget();
        }
    }
}
