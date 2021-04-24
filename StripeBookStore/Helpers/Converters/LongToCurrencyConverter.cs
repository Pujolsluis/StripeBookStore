using System;
using System.Globalization;
using Xamarin.Forms;

namespace StripeBookStore.Helpers.Converters
{
    public class LongToCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long amount = (long)value;

            return $"${(decimal)amount / 100 : 0.00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal amount;

            decimal.TryParse((string)value, out amount);

            return (long)amount * 100;
        }
    }
}
