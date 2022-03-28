using MobileApp.Assets.Constants;
using MobileApp.Models;
using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class CompleteOrderScreen : ContentPage
    {
        private readonly BaseViewModel _viewModel;

        private readonly OrderData _orderData;
        public CompleteOrderScreen(OrderData orderData)
        {
            InitializeComponent();

            _viewModel = new CompleteOrderScreenViewModel();
            this.BindingContext = _viewModel;
            _orderData = orderData;
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(CompleteOrderScreen));
            _viewModel.InitializeViewModel(_orderData);
        }
    }
}