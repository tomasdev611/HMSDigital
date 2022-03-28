using MobileApp.Assets;
using MobileApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class LoginScreen : ContentPage
    {
        public LoginScreen()
        {
            InitializeComponent();
            branchVersion.Text = $"Version: {AppConfiguration.Environment}-{VersionTracking.CurrentVersion}";
            this.BindingContext = new LoginScreenViewModel();
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(LoginScreen));
            base.OnAppearing();
        }
    }
}

