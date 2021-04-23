using System;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using StripeBookStore.Pages;
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
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            Sharpnado.MaterialFrame.Initializer.Initialize(loggerEnable: false, false);

            NavigationService.NavigateAsync(NavigationConstants.BooksCatalog);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<BooksCatalogPage, BooksCatalogPageViewModel>();

            IStripeBookStoreApi stripeBookStoreApiClient = RefitExtensions.For<IStripeBookStoreApi>(BaseApiService.CreateHttpClient(StripeBookStoreConstants.StripeBookStoreBaseUrl));

            //Services
            containerRegistry.RegisterSingleton<IPreferences, PreferencesImplementation>();
            containerRegistry.RegisterInstance(stripeBookStoreApiClient);
        }
    }
}
