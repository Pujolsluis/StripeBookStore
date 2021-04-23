using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StripeBookStore.Pages
{
    public partial class BooksCatalogPage : ContentPage
    {
        public BooksCatalogPage()
        {
            InitializeComponent();
            //Work around for https://github.com/xamarin/Xamarin.Forms/issues/9879
            booksCollectionsView.Header = Device.RuntimePlatform is Device.Android ? new BoxView { HeightRequest = 4 } : null;
            booksCollectionsView.Footer = Device.RuntimePlatform is Device.Android ? new BoxView { HeightRequest = 16 } : null;
        }
    }
}
