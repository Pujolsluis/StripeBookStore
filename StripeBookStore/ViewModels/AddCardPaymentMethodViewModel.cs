using System;
using Prism.Commands;
using Prism.Navigation;
using StripeBookStore.Shared.Models;
using StripeBookStore.ViewModels.Base;

namespace StripeBookStore.ViewModels
{
    public class AddCardPaymentMethodViewModel : BaseViewModel
    {
        public AddCardPaymentMethodViewModel(INavigationService navigationService) : base(navigationService)
        {

            OnAddCardCommand = new DelegateCommand(async () =>
            {
                var card = new Card()
                {
                    Number = CardNumber,
                    CVV = CardCvv,
                    ExpirationDate = CardExpirationDate
                };

                await NavigationService.GoBackAsync(new NavigationParameters() { { NavigationDataConstants.Card, card } });
            });
        }

        public DelegateCommand OnAddCardCommand { get; set; }

        private string _pageTitle = "Add Payment Method";
        public string PageTitle
        {
            get => _pageTitle;
        }

        private string _cardNumber;
        public string CardNumber
        {
            get => _cardNumber;
            set => SetProperty(ref _cardNumber, value);
        }

        private string _cardCvv;
        public string CardCvv
        {
            get => _cardCvv;
            set => SetProperty(ref _cardCvv, value);
        }

        private string _cardExpirationDate;
        public string CardExpirationDate
        {
            get => _cardExpirationDate;
            set => SetProperty(ref _cardExpirationDate, value);
        }

        private string _zipCode;
        public string ZipCode
        {
            get => _zipCode;
            set => SetProperty(ref _zipCode, value);
        }
    }
}
