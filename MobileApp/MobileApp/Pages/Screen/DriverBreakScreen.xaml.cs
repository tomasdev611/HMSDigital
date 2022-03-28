using MobileApp.Models;
using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class DriverBreakScreen : ContentPage
    {
        private readonly BaseViewModel _viewModel;

        private readonly OrderData _orderData;

        public DriverBreakScreen(OrderData orderData)
        {
            InitializeComponent();

            _viewModel = new DriverBreakScreenViewModel();
            this.BindingContext = _viewModel;
            _orderData = orderData;
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel(_orderData);
            PagesUtils.PageNavigationFollowUp(nameof(DriverBreakScreen));
        }
    }
}
