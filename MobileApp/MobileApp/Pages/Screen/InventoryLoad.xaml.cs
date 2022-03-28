using MobileApp.Pages.CommonPages;
using MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryLoad : ContentPage
    {
        private readonly BaseViewModel _viewModel;

        public InventoryLoad()
        {
            InitializeComponent();
            _viewModel = new InventoryLoadViewModel();
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(InventoryLoad));
            _viewModel.InitializeViewModel();
        }

        protected override void OnDisappearing()
        {
            _viewModel.DestroyViewModel();
        }

    }
}