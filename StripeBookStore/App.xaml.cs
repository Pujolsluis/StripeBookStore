using System;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using StripeBookStore.Pages;
using StripeBookStore.Services;
using StripeBookStore.Shared;
using StripeBookStore.Shared.Constants;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Services;
using StripeBookStore.ViewModels;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace StripeBookStore
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null, IApiManager apiManager = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            Sharpnado.MaterialFrame.Initializer.Initialize(loggerEnable: false, false);

            NavigationService.NavigateAsync(NavigationConstants.BooksCatalog);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BooksCatalogPage, BooksCatalogPageViewModel>();
            containerRegistry.RegisterForNavigation<CheckoutPage, CheckoutPageViewModel>();
            containerRegistry.RegisterForNavigation<AddCardPaymentMethodPage, AddCardPaymentMethodViewModel>();

            IStripeBookStoreApi stripeBookStoreApiClient = RefitExtensions.For<IStripeBookStoreApi>(BaseApiService.CreateHttpClient(StripeBookStoreConstants.StripeBookStoreBaseUrl));

            //Services
            containerRegistry.RegisterSingleton<IPreferences, PreferencesImplementation>();
            containerRegistry.RegisterSingleton<ISecureStorage, SecureStorageImplementation>();
            containerRegistry.RegisterSingleton<IConnectivity, ConnectivityImplementation>();
            containerRegistry.RegisterSingleton<IMainThread, MainThreadImplementation>();
            containerRegistry.Register<IApiManager, ApiManager>();
            containerRegistry.RegisterInstance(stripeBookStoreApiClient);
        }
    }
}
