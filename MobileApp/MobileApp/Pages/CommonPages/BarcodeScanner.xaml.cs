using MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.CommonPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarcodeScanner : ContentPage
    {
        private readonly BaseViewModel _viewModel;
        public BarcodeScanner()
        {
            InitializeComponent();
            _viewModel = new BarcodeScannerViewModel();
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PagesUtils.PageNavigationFollowUp(nameof(BarcodeScanner));
        }

        protected override void OnDisappearing()
        {
            _viewModel.DestroyViewModel();
        }
    }
}