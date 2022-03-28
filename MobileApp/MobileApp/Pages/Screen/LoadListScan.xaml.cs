using MobileApp.Models;
using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.CommonPages
{
    public partial class LoadListScan : ContentPage
    {
        private readonly BaseViewModel _viewModel;

        private readonly VehicleLoadlist _vehicleLoadlist;

        public LoadListScan(VehicleLoadlist vehicleLoadlists)
        {
            InitializeComponent();
            _viewModel = new LoadListScanViewModel();
            this.BindingContext = _viewModel;

            _vehicleLoadlist = vehicleLoadlists;
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(LoadListScan));
            _viewModel.InitializeViewModel(_vehicleLoadlist);
        }

        protected override void OnDisappearing()
        {
            _viewModel.DestroyViewModel();
        }
    }
}
