using MobileApp.Assets.Constants;
using MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenu : FlyoutPage
    {
        private BaseViewModel vm;

        bool _shouldShowFlashMessage = false;

        public HamburgerMenu(bool shouldShowFlashMessage)
        {
            InitializeComponent();
            _shouldShowFlashMessage = shouldShowFlashMessage;
            vm = new HamburgerMenuViewModel(_shouldShowFlashMessage);
            this.BindingContext = vm;
            MasterPage.IconImageSource = ImageSource.FromResource(AppConstants.IMAGE_PATH + "menuIcon.png");
        }

        protected override void OnAppearing()
        {
            vm.InitializeViewModel();
        }
    }
}
