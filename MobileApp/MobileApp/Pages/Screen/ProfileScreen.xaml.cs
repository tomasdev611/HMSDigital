using MobileApp.Assets;
using MobileApp.ViewModels;
using Xamarin.Essentials;

namespace MobileApp.Pages.Screen
{
    public partial class ProfileScreen
    {
        public ProfileScreen()
        {
            InitializeComponent();
            this.BindingContext = new ProfileScreenViewModel();
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(ProfileScreen));
            VersionTracking.Track();
            branchVersion.Text = $"Version: {AppConfiguration.Environment}-{VersionTracking.CurrentVersion}";
        }
    }
}