using MobileApp.Assets.Constants;
using MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenuMaster : ContentPage
    {
        public ListView ListView;

        private BaseViewModel vm;

        public HamburgerMenuMaster()
        {
            InitializeComponent();

            ListView = MenuItemsListView;

            vm = new HamburgerMenuMasterViewModel();
            this.BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            vm.InitializeViewModel();
        }
    }
}
