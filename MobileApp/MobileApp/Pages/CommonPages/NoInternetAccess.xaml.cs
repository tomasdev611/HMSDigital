using MobileApp.Assets.Constants;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.CommonPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetAccess : ContentPage
    {
        public NoInternetAccess()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PagesUtils.PageNavigationFollowUp(nameof(NoInternetAccess));
        }
    }
}