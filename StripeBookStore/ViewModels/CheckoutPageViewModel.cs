using System;
using Prism.Navigation;
using StripeBookStore.Shared.Models;
using StripeBookStore.ViewModels.Base;

namespace StripeBookStore.ViewModels
{
    public class CheckoutPageViewModel : BaseViewModel, IInitialize
    {

        public CheckoutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private string _bookName;
        public string BookName
        {
            get => _bookName;
            set => SetProperty(ref _bookName, value);
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(NavigationDataConstants.Book))
                BookName = parameters.GetValue<Book>(NavigationDataConstants.Book).Name;
        }
    }
}
