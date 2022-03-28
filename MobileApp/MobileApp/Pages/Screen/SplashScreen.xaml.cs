using MobileApp.Assets.Constants;
using MobileApp.Interface;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class SplashScreen : ContentPage
    {
        private INavigationService _navigationService;

        private Image splashImage;

        public SplashScreen()
        {
            _navigationService = App.NavigationService;
            NavigationPage.SetHasNavigationBar(this, false);

            var screenLayout = new AbsoluteLayout();
            splashImage = new Image
            {
                Source = ImageSource.FromResource(AppConstants.IMAGE_PATH + "appLogo.png"),
                WidthRequest = 100,
                HeightRequest = 100
            };
            AbsoluteLayout.SetLayoutFlags(splashImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            screenLayout.Children.Add(splashImage);

            this.BackgroundColor = Color.FromHex("#FFFFFF");
            this.Content = screenLayout;

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await splashImage.ScaleTo(3, 500, Easing.BounceOut);
            await _navigationService.InitializeAsync();
        }
    }
}
