using System;
using System.Windows.Input;
using MobileApp.Models;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class AssignedTruckViewModel : BaseViewModel
    {
        public AssignedTruckViewModel() : base()
        {
            IsVehicleAssigned = true;
        }

        public override void InitializeViewModel(object data = null)
        {
            OrderCompleted = (bool)data;
            GetDriverData();
        }

        private bool _orderCompleted;

        public bool OrderCompleted
        {
            get
            {
                return _orderCompleted;
            }
            set
            {
                _orderCompleted = value;
                OnPropertyChanged();
            }
        }

        private string _driverName;

        public string DriverName
        {
            get
            {
                return _driverName;
            }
            set
            {
                _driverName = value;
                OnPropertyChanged();
            }
        }

        private Driver _driverDetails;

        public Driver DriverDetails
        {
            get
            {
                return _driverDetails;
            }
            set
            {
                _driverDetails = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading = true;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private bool _isVehicleAssigned;

        public bool IsVehicleAssigned
        {
            get
            {
                return _isVehicleAssigned;
            }
            set
            {
                _isVehicleAssigned = value;
                OnPropertyChanged();
            }
        }

        private ICommand _acceptTruckCommand;

        public ICommand AcceptTruckCommand
        {
            get
            {
                return _acceptTruckCommand ?? (_acceptTruckCommand = new Command(AcceptTruck));
            }
        }

        public async void GetDriverData()
        {
            try
            {
                DriverDetails = await CacheManager.GetDriverDetailsFromCache();
                DriverName = $"{DriverDetails.FirstName} {DriverDetails.LastName}";
                IsVehicleAssigned = DriverDetails.CurrentVehicleId != null;
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("something went wrong while fetching data");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void AcceptTruck()
        {
            await _navigationService.NavigateToAsync(typeof(HamburgerMenu), true);
        }
    }

}
