using MobileApp.Models;
using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.CommonPages
{
    public partial class OrderListScan : ContentPage
    {
        private readonly BaseViewModel _viewModel;

        private readonly OrderData _orderData;

        public OrderListScan(OrderData orderData)
        {
            _viewModel = new OrderListScanViewModel();
            this.BindingContext = _viewModel;
            _orderData = orderData;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(OrderListScan));
            _viewModel.InitializeViewModel(_orderData);
        }

        protected override void OnDisappearing()
        {
            _viewModel.DestroyViewModel();
        }
    }
}
