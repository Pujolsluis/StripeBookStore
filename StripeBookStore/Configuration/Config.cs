using System;
using Xamarin.Forms;

namespace StripeBookStore.Configuration
{
    public static class Config
    {
        #region localhosts
        //iOS Emulator localhost
        const string _iOSBaseLocalhostUrl = "http://localhost:42424/api";
        //Android Emulator localhost
        const string _androidBaseLocalhostUrl = "http://10.0.2.2:42424/api";
        #endregion


        #region Hubs and HubResponses
        //iOS Emulator Base URL
        public const string _iOSPaymentEventsHubUrl = "http://localhost:42424/hubs/paymentEvents";
        //Android Emulator Base Url
        public const string _androidPaymentEventsHubUrl = "http://10.0.2.2:42424/hubs/paymentEvents";

        public const string SendPaymentEventsHubResponse = "SendPaymentEvent";
        #endregion

        public static string StripeBookStoreBaseUrl => Device.RuntimePlatform switch
        {
            Device.iOS => _iOSBaseLocalhostUrl,
            Device.Android => _androidBaseLocalhostUrl,
            _ => throw new NotSupportedException()
        };

        public static string PaymentEventsHubUrl => Device.RuntimePlatform switch
        {
            Device.iOS => _iOSPaymentEventsHubUrl,
            Device.Android => _androidPaymentEventsHubUrl,
            _ => throw new NotSupportedException()
        };
    }
}
