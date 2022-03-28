using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen.CommonPages
{
    public partial class OrdersViewScreen : ContentPage
    {
        private readonly Dialer _dialer;

        private readonly ListGroup<OrderData> _orderList;

        private readonly BaseViewModel _viewModel;

        public OrdersViewScreen(ListGroup<OrderData> orderList)
        {
            _dialer = new Dialer();
            _orderList = orderList;
            InitializeComponent();
            _viewModel = new OrdersViewScreenViewModel();
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel(_orderList);
            PagesUtils.PageNavigationFollowUp(nameof(OrdersViewScreen));
        }
    }
}
