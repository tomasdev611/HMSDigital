using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileApp.Exceptions;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using MobileApp.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ProfileScreenViewModel : BaseViewModel
    {
        private readonly StorageService _storageService;

        private readonly CurrentLocation _locationService;

        private readonly NotificationService _notificationService;

        public ProfileScreenViewModel() : base()
        {
            _storageService = new StorageService();
            _locationService = new CurrentLocation();
            _notificationService = new NotificationService();

            GetProfileData();
        }

        public async void GetProfileData()
        {
            await GetUserProfileInfo();
            await GetLocationAsync();
        }

        private string _siteAddress;

        public string SiteAddress
        {
            get
            {
                return _siteAddress;
            }
            set
            {
                _siteAddress = value;
                OnPropertyChanged();
            }
        }

        private User _userDetails;

        public User UserDetails
        {
            get
            {
                return _userDetails;
            }
            set
            {
                _userDetails = value;
                OnPropertyChanged();
            }
        }

        private bool _shouldShowVehicle = true;

        public bool ShouldShowVehicle
        {
            get
            {
                return _shouldShowVehicle;
            }
            set
            {
                _shouldShowVehicle = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get
            {
                return _phoneNumber ?? "-";
            }
            set
            {
                if (!string.Equals("0", value))
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _siteName;

        public string SiteName
        {
            get
            {
                return _siteName;
            }
            set
            {
                _siteName = value;
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

        private string _vehicleName;

        public string VehicleName
        {
            get
            {
                return _vehicleName;
            }
            set
            {
                _vehicleName = value;
                OnPropertyChanged();
            }
        }

        private decimal _siteLatitude;

        public decimal SiteLatitude
        {
            get
            {
                return _siteLatitude;
            }
            set
            {
                _siteLatitude = value;
                OnPropertyChanged();
            }
        }

        private decimal _siteLongitude;

        public decimal SiteLongitude
        {
            get
            {
                return _siteLongitude;
            }
            set
            {
                _siteLongitude = value;
                OnPropertyChanged();
            }
        }

        private double _currentLatitude;

        public double CurrentLatitude
        {
            get
            {
                return _currentLatitude;
            }
            set
            {
                _currentLatitude = value;
                OnPropertyChanged();
            }
        }

        private double _currentLongitude;

        public double CurrentLongitude
        {
            get
            {
                return _currentLongitude;
            }
            set
            {
                _currentLongitude = value;
                OnPropertyChanged();
            }
        }

        private string _currentLocation;

        public string CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;
                OnPropertyChanged();
            }
        }

        private ICommand _logoutCommand;

        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ?? (_logoutCommand = new Command(Logout));
            }
        }

        private async Task GetUserProfileInfo()
        {
            try
            {
                UserDetails = await CacheManager.GetUserDetails();
                if (UserDetails != null)
                {
                    PhoneNumber = UserDetails.PhoneNumber.ToString();
                }

                var currentVehicle = await UserDetailsUtils.GetCurrentVehicleDetails();
                if (currentVehicle == null)
                {
                    ShouldShowVehicle = false;
                }
                else
                {
                    VehicleName = $"{currentVehicle.Name} ({currentVehicle.Cvn})";
                }

                var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();

                var currentSiteDetails = await CacheManager.GetSiteDetailByIdFromCache(currentSiteId ?? 0);
                var address = currentSiteDetails.Address;
                if (address != null)
                {
                    SiteAddress = CommonUtility.AddressToString(address);
                    SiteLatitude = address.Latitude;
                    SiteLongitude = address.Longitude;
                }
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error occured while fetching profile information");
                return;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetLocationAsync()
        {
            try
            {
                var location = await _locationService.GetCurrentLocationAsync();

                if (location == null)
                {
                    _toastMessageService.ShowToast("Not able to fetch the location");
                    return;
                }
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var currentArea = placemarks?.FirstOrDefault();
                CurrentLocation = $"{currentArea.Locality} {currentArea.AdminArea} ({currentArea.CountryName})";

                CurrentLatitude = location.Latitude;
                CurrentLongitude = location.Longitude;
            }
            catch (PermissionRequiredException prex)
            {
                _toastMessageService.ShowToast(prex.Message);
            }
            catch(Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Unable to fetch location");
            }
        }

        public async void Logout()
        {
            try
            {
                IsLoading = true;
                await _notificationService.DeregisterDeviceAsync();

                _storageService.ClearStorage();
                CacheManager.ClearCache();
                await _navigationService.NavigateToAsync(typeof(LoginScreen));
            }
            catch (BadRequestException ex)
            {
                _toastMessageService.ShowToast(ex.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Not able to complete the request at the moment. Please try again");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}