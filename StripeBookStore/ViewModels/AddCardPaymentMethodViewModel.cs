using System;
using System.Diagnostics;
using System.Globalization;
using Prism.Commands;
using Prism.Navigation;
using Stripe;
using StripeBookStore.Shared.Constants;
using StripeBookStore.Shared.Models;
using StripeBookStore.ViewModels.Base;
using Xamarin.Essentials.Interfaces;
using Card = StripeBookStore.Shared.Models.Card;

namespace StripeBookStore.ViewModels
{
    public class AddCardPaymentMethodViewModel : BaseViewModel
    {
        readonly IMainThread _mainThread;
        readonly IPreferences _preferences;

        public AddCardPaymentMethodViewModel(INavigationService navigationService, IMainThread mainThread, IPreferences preferences) : base(navigationService)
        {
            _mainThread = mainThread;
            _preferences = preferences;

            OnAddCardCommand = new DelegateCommand(CreatePaymentMethod());
        }

        public DelegateCommand OnAddCardCommand { get; set; }

        private string _pageTitle = "Add Payment Method";
        public string PageTitle
        {
            get => _pageTitle;
        }

        public bool AllCardFielsValid
        {
            get
            {
                return !(string.IsNullOrEmpty(CardNumber) && string.IsNullOrEmpty(CardCVC)
                        && string.IsNullOrEmpty(CardExpirationDate)) && CardNumber?.Length == 19
                        && CardExpirationDate?.Length == 5 && CardCVC?.Length == 3;
            }
        }

        private string _cardNumber;
        public string CardNumber
        {
            get => _cardNumber;
            set => SetProperty(ref _cardNumber, value);
        }

        private string _cardCVC;
        public string CardCVC
        {
            get => _cardCVC;
            set => SetProperty(ref _cardCVC, value);
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

        Action CreatePaymentMethod()
        {
            return async () =>
            {
                try
                {
                    if (AllCardFielsValid)
                    {
                        DateTime currentDateTime = DateTime.Now.AddYears(100);
                        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                        calendar.TwoDigitYearMax = currentDateTime.Year;

                        var twoDigitMonth = CardExpirationDate.Split('/')[0];
                        var twoDigitYear = Convert.ToInt32(CardExpirationDate.Split('/')[1]);
                        var fourDigitYear = calendar.ToFourDigitYear(twoDigitYear);

                        var card = new Card()
                        {
                            Number = CardNumber,
                            CVC = CardCVC,
                            ExpirationDate = CardExpirationDate,
                            ExpMonth = Convert.ToInt64(twoDigitMonth),
                            ExpYear = Convert.ToInt64(fourDigitYear),
                        };

                        PaymentMethodService paymentMethodService = new PaymentMethodService(new StripeClient(_preferences.Get(StripeBookStoreConstants.SettingPublishableKey, string.Empty)));
                        var paymentMethodOptions = new PaymentMethodCreateOptions()
                        {
                            Card = new PaymentMethodCardOptions()
                            {
                                Number = card.Number,
                                Cvc = card.CVC,
                                ExpMonth = card.ExpMonth,
                                ExpYear = card.ExpYear,
                            },
                            Type = "card"
                        };

                        var paymentMethod = await paymentMethodService.CreateAsync(paymentMethodOptions).ConfigureAwait(false);

                        if (!string.IsNullOrEmpty(paymentMethod.Id))
                            card.Id = paymentMethod.Id;

                        await _mainThread.InvokeOnMainThreadAsync(async () => await NavigationService.GoBackAsync(new NavigationParameters() { { NavigationDataConstants.Card, card } }));
                    }
                    else
                    {
                        if (Xamarin.Forms.Application.Current?.MainPage is Xamarin.Forms.Page mainPage)
                            await _mainThread.InvokeOnMainThreadAsync(async () => await mainPage.DisplayAlert("Incorrect Information", "Your credit card information is not in the required format, please review and try again.", "OK")).ConfigureAwait(false);
                    }
                }
                catch(StripeException ex)
                {
                    if (Xamarin.Forms.Application.Current?.MainPage is Xamarin.Forms.Page mainPage)
                        await _mainThread.InvokeOnMainThreadAsync(async () => await mainPage.DisplayAlert("Error", $"{ex.Message}", "OK")).ConfigureAwait(false);
                }
                catch (IndexOutOfRangeException ex)
                {
                    Debug.WriteLine($"Card Expiration date was entered with incorrect format: {ex}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };
        }
    }
}
