using System;
using System.Windows.Input;
using MobileApp.Methods;
using MobileApp.Models;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class DriverBreakScreenViewModel : BaseViewModel
    {
        public DriverBreakScreenViewModel() : base()
        {
        }

        public override void InitializeViewModel(object data = null)
        {
            var orderData = (OrderData)data;
            ShippingAddress = CommonUtility.AddressToString(orderData.ShippingAddress);
            Name = orderData.ContactPerson;
            Duration = DateTime.UtcNow.Subtract(orderData.FulfillmentStartDateTime).TotalHours.ToString("0:00") + " hours";
        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        string _shippingAddress;
        public string ShippingAddress
        {
            get
            {
                return _shippingAddress;
            }
            set
            {
                _shippingAddress = value;
                OnPropertyChanged();
            }
        }

        string _duration;
        public string Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        private ICommand _navigateToNextOrderCommand;

        public ICommand NavigateToNextOrderCommand
        {
            get
            {
                return _navigateToNextOrderCommand ?? (_navigateToNextOrderCommand = new Command(NavigateToNextOrder));
            }
        }

        private async void NavigateToNextOrder()
        {
            await _navigationService.PopToRootAsync();
        }
    }
}
