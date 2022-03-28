using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Exceptions;
using MobileApp.Models;
using MobileApp.Pages.CommonPages;
using MobileApp.Service;
using MobileApp.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class InventoryLoadViewModel : BaseViewModel
    {
        private readonly InventoryService _inventoryService;

        private readonly SiteService _siteService;

        public InventoryLoadViewModel() : base()
        {
            _inventoryService = new InventoryService();
            _siteService = new SiteService();
        }
        public async override void InitializeViewModel(object data = null)
        {
            initMessages();
            await InitLocation();
        }

        public override void DestroyViewModel()
        {
            UnsubscribeMessagingCenter();
        }

        private void UnsubscribeMessagingCenter()
        {
            MessagingCenter.Unsubscribe<Inventory>(this, MessagingConstant.SCANNED_INVENTORY);
        }


        private void initMessages()
        {
            MessagingCenter.Subscribe<Inventory>(MessagingConstant.INVENTORY_LOAD_VIEWMODEL, MessagingConstant.SCANNED_INVENTORY, (inventory) =>
            {
                InventoryDetails = inventory;
                ErrorMessage = "";
                IsLoading = false;
                if (!string.IsNullOrWhiteSpace(SuccessMessage))
                {
                    SuccessMessage = "";
                }
            });
        }

        private bool _isLocationPresent = true;

        public bool IsLocationPresent
        {
            get
            {
                return _isLocationPresent;
            }
            set
            {
                _isLocationPresent = value;
                OnPropertyChanged();
            }
        }

        private ICommand _getInventory;
        public ICommand GetInventoryCommand
        {
            get
            {
                return _getInventory ?? (_getInventory = new Command(GetInventory));
            }
        }

        private ICommand _loadInventory;

        public ICommand LoadInventoryCommand
        {
            get
            {
                return _loadInventory ?? (_loadInventory = new Command(LoadInventory));
            }
        }

        private ICommand _scanInventoryCommand;

        public ICommand ScanInventoryCommand
        {
            get
            {
                return _scanInventoryCommand ?? (_scanInventoryCommand = new Command(ScanInventory));
            }
        }

        private bool _isLoading;
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

        private bool _isLoadSelected = true;
        public bool IsLoadSelected
        {
            get
            {
                return _isLoadSelected;
            }
            set
            {
                _isLoadSelected = value;
                ErrorMessage = "";
                SuccessMessage = "";
                if (value)
                {
                    ButtonActionText = "Load";
                }
                InitLocation();
                OnPropertyChanged();
            }
        }

        private bool _isDropSelected;
        public bool IsDropSelected
        {
            get
            {
                return _isDropSelected;
            }
            set
            {
                _isDropSelected = value;
                ErrorMessage = "";
                SuccessMessage = "";
                if (value)
                {
                    ButtonActionText = "Drop";
                }
                InitLocation();
                OnPropertyChanged();
            }
        }

        private string _buttonActionText;
        public string ButtonActionText
        {
            get
            {
                return _buttonActionText;
            }
            set
            {
                _buttonActionText = value;
                OnPropertyChanged();
            }
        }

        private LookUp _selectedLocation;

        public LookUp SelectedLocation
        {
            get
            {
                return _selectedLocation;
            }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LookUp> _locationList;

        public ObservableCollection<LookUp> LocationList
        {
            get
            {
                return _locationList;
            }
            set
            {
                _locationList = value;
                OnPropertyChanged();
            }
        }
        private string _serialNumber;
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
                if (!string.IsNullOrWhiteSpace(SuccessMessage))
                {
                    SuccessMessage = "";
                }
                OnPropertyChanged();
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private string _successMessage;
        public string SuccessMessage
        {
            get
            {
                return _successMessage;
            }
            set
            {
                _successMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsInventoryAvailable
        {
            get
            {
                return InventoryDetails != null;
            }
        }

        private Inventory _inventoryDetails;
        public Inventory InventoryDetails
        {
            get
            {
                return _inventoryDetails;
            }
            set
            {
                _inventoryDetails = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsInventoryAvailable));
            }
        }

        private async void GetInventory()
        {
            if (string.IsNullOrEmpty(SerialNumber))
            {
                ErrorMessage = "Please enter tag to fetch the inventory";
                return;
            }
            IsLoading = true;
            try
            {
                var filterString = $"(serialNumber | assetTagNumber | lotNumber) == {SerialNumber}";

                var inventory = (await _inventoryService.GetInventory(filterString))?.FirstOrDefault();
                if (inventory == null)
                {
                    ErrorMessage = "No inventory found for this tag";
                }
                else
                {
                    InventoryDetails = inventory;
                    ErrorMessage = "";
                }
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                ErrorMessage = "Unable to fetch the inventory ";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void ScanInventory()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status == PermissionStatus.Granted)
            {
                await _navigationService.NavigateToAsync<BarcodeScanner>();
            }
            else if (status == PermissionStatus.Denied)
            {
                _toastMessageService.ShowToast("You should enable Camera Permission in order to scan");
            }
            else
            {
                var permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (permissionStatus == PermissionStatus.Granted)
                    await _navigationService.NavigateToAsync<BarcodeScanner>();
            }


            await _navigationService.NavigateToAsync<BarcodeScanner>();
        }

        private async void LoadInventory()
        {
            IsLoading = true;
            try
            {
                var moveInventoryRequest = new MoveInventoryRequest
                {
                    AssetTagNumber = InventoryDetails.AssetTagNumber,
                    SerialNumber = InventoryDetails.SerialNumber,
                    ItemNumber = InventoryDetails.Item.ItemNumber
                };

                if (SelectedLocation == null || SelectedLocation.Id == 0)
                {
                    ErrorMessage = "Invalid Location Selected";
                    return;
                }

                if (IsLoadSelected)
                {
                    moveInventoryRequest.DestinationLocationId = SelectedLocation.Id;
                }
                else
                {
                    moveInventoryRequest.DestinationLocationId = SelectedLocation.Id;
                }

                var inventoryResponse = await _inventoryService.MoveInventory(moveInventoryRequest);

                if (inventoryResponse)
                {
                    InventoryDetails = null;
                    SuccessMessage = "Inventory loaded succesfully";
                }
                else
                {
                    ErrorMessage = "Error while loading inventory";
                }
            }
            catch (BadRequestException ex)
            {
                _toastMessageService.ShowToast(ex.Message);
                ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                ErrorMessage = "Error while loading inventory";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task InitLocation()
        {
            await GetLocationList();
            if (IsLoadSelected)
            {
                var currentVehicleId = await UserDetailsUtils.GetUsersCurrentVehicleId();
                if (currentVehicleId == null)
                {
                    IsLocationPresent = false;
                }
                else
                {
                    IsLocationPresent = true;
                    SelectedLocation = LocationList.FirstOrDefault(l => l.Id == currentVehicleId);
                }
                return;
            }
            var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();
            if (currentSiteId == null)
            {
                IsLocationPresent = false;
            }
            else
            {
                IsLocationPresent = true;
                SelectedLocation = LocationList.FirstOrDefault(l => l.Id == currentSiteId);
            }
        }

        private async Task GetLocationList()
        {
            try
            {
                var siteList = await _siteService.GetAllSitesAsync();

                if (siteList == null)
                {
                    return;
                }

                LocationList = new ObservableCollection<LookUp>();
                foreach (var site in siteList)
                {
                    LocationList.Add(new LookUp() { Id = site.Id, Name = site.Name });
                }
            }
            catch (BadRequestException bex)
            {
                _toastMessageService.ShowToast(bex.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Not able to fetch location");
            }
        }
    }
}
