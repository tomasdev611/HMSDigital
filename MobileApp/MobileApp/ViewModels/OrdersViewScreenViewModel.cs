using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Pages.PopUpMenu;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class OrdersViewScreenViewModel : BaseViewModel
    {
        private readonly Dialer _dialer;

        public OrdersViewScreenViewModel() : base()
        {
            _dialer = new Dialer();
        }

        private ListGroup<OrderData> _ordersList;

        public ListGroup<OrderData> OrdersList
        {
            get
            {
                return _ordersList;
            }
            set
            {
                _ordersList = value;
                OnPropertyChanged();
            }
        }

        public override void InitializeViewModel(object data = null)
        {
            OrdersList = (ListGroup<OrderData>)data;
        }

        private ICommand _callContactPersonCommand;

        public ICommand CallContactPersonCommand
        {
            get
            {
                return _callContactPersonCommand ?? (_callContactPersonCommand = new Command(CallContactPerson));
            }
        }

        private async void CallContactPerson(object parameter)
        {
            var contactNumber = (string)parameter;

            if (contactNumber != null)
            {
                _dialer.OpenDialerWithNumber(contactNumber);
            }
            await _navigationService.PushPopupAsync<CallReport>(contactNumber);
        }
    }
}
