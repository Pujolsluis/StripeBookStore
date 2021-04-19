using System;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Prism.Mvvm;
using Prism.Navigation;
using StripeBookStore.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace StripeBookStore.ViewModels
{
    public class BooksCatalogPageViewModel : BaseViewModel, IDestructible
    {

        private readonly IPreferences _preferences;

        public BooksCatalogPageViewModel(INavigationService navigationService, IPreferences preferences) : base(navigationService)
        {
            _preferences = preferences;
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
            if (!IsFirstTime)
                return string.Empty;
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
            });

            await Task.Delay(3000);

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
