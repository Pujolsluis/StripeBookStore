using System;
using Prism.Commands;
using Prism.Navigation;
using StripeBookStore.Shared.Models;
using StripeBookStore.ViewModels.Base;

namespace StripeBookStore.ViewModels
{
    public class CheckoutPageViewModel : BaseViewModel, IInitialize
    {
        


        public CheckoutPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            PageTitle = "Checkout";

            OnSelectPaymentMethodCommand = new DelegateCommand(() =>
            {
                PaymentMethod = string.IsNullOrEmpty(_paymentMethod) ? "Visa" : string.Empty;
            });

            OnConfirmPaymentCommand = new DelegateCommand(() =>
            {
                //TODO: Confirm Payment Intent with selected payment method
                OrderSubTotalAmount += 10;
            });
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(NavigationDataConstants.Book))
            {
                Book = parameters.GetValue<Book>(NavigationDataConstants.Book);
                OrderSubTotalAmount = Book.Price;
            }
        }

        public DelegateCommand OnSelectPaymentMethodCommand { get; set; }
        public DelegateCommand OnConfirmPaymentCommand { get; set; }

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        private Book _book;
        public Book Book
        {
            get => _book;
            set => SetProperty(ref _book, value);
        }

        private long _orderSubTotalAmount;
        public long OrderSubTotalAmount
        {
            get => _orderSubTotalAmount;
            set => SetProperty(ref _orderSubTotalAmount, value, onOrderSubTotalChanged);
        }

        private long _orderTaxAmount;
        public long OrderTaxAmount
        {
            get => _orderTaxAmount;
            set => SetProperty(ref _orderTaxAmount, value);
        }

        private long _orderTotalAmount;
        public long OrderTotalAmount
        {
            get => _orderTotalAmount;
            set => SetProperty(ref _orderTotalAmount, value);
        }

        private void onOrderSubTotalChanged()
        {
            OrderTaxAmount = (long)(((decimal)OrderSubTotalAmount / 100 * (decimal)(0.0825)) * 100);
            OrderTotalAmount = OrderSubTotalAmount + OrderTaxAmount;
            OnPropertyChanged(nameof(PayAmount));
        }

        private string _payAmount;
        public string PayAmount
        {
            get => $"Pay ${(decimal)_orderTotalAmount / 100: 0.00}";
            set => SetProperty(ref _payAmount, value);
        }

        private string _paymentMethod;
        public string PaymentMethod
        {
            get => string.IsNullOrEmpty(_paymentMethod) ? "+ Add" : _paymentMethod;
            set => SetProperty(ref _paymentMethod, value);
        }
    }
}
