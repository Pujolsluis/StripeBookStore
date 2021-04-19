using System;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using StripeBookStore.Pages;
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
            NavigationService.NavigateAsync(NavigationConstants.BooksCatalog);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<BooksCatalogPage, BooksCatalogPageViewModel>();

            //Services
            containerRegistry.RegisterSingleton<IPreferences, PreferencesImplementation>();
        }
    }
}
