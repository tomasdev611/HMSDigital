using MobileApp.Assets.Constants;
using MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.CommonPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoLocationAccess : ContentPage
    {
        BaseViewModel vm;
        public NoLocationAccess()
        {
            InitializeComponent();
            vm = new NoLocationAccessViewModel();
            this.BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            vm.InitializeViewModel();
            PagesUtils.PageNavigationFollowUp(nameof(NoLocationAccess));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}