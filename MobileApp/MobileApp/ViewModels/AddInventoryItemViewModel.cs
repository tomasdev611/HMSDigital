using JsonDiffPatchDotNet;
using JsonDiffPatchDotNet.Formatters.JsonPatch;
using MobileApp.Exceptions;
using MobileApp.Models;
using MobileApp.Service;
using MobileApp.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class AddInventoryItemViewModel : BaseViewModel
    {
        private readonly InventoryService _inventoryService;

        private readonly SiteService _siteService;

        public AddInventoryItemViewModel() : base()
        {
            _inventoryService = new InventoryService();
            _siteService = new SiteService();
        }

        public async override void InitializeViewModel(object data = null)
        {
            if (data == null)
            {
                InventoryItem = new Inventory();
                await GetProductsList();
            }
            else
            {
                var inventoryItem = (Inventory)data;
                IsEditForm = true;
                ProductsList = new List<Item>() { inventoryItem.Item };
                InventoryItem = inventoryItem;
                await GetLocationList();
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

                SelectedLocation = LocationList.FirstOrDefault(l => l.Id == InventoryItem.CurrentLocationId);
                SelectedStatus = StatusList.FirstOrDefault(s => s.Id == InventoryItem.StatusId);
            }
            catch (BadRequestException bex)
            {
                _toastMessageService.ShowToast(bex.Message);
            }
            catch(Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Not able to fetch location");
            }
        }

        private async Task GetProductsList()
        {
            try
            {
                IsLoading = true;
                ProductsList = await CacheManager.GetItemsList();
            }
            catch (BadRequestException bex)
            {
                _toastMessageService.ShowToast(bex.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Something went wrong");
            }
            finally
            {
                IsLoading = false;
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

        private IEnumerable<LookUp> _statusList = new List<LookUp>() {
            new LookUp() {Id= 1, Name= "Available"},
            new LookUp() {Id= 2, Name= "NotReady"},
            new LookUp() {Id= 3, Name= "WillBeAvailable"},
            new LookUp() {Id= 4, Name= "LockedForPickup"},
            new LookUp() {Id= 5, Name= "InTransit"}
        };

        private LookUp _selectedStatus;

        public LookUp SelectedStatus
        {
            get
            {
                return _selectedStatus;
            }
            set
            {
                _selectedStatus = value;
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

        private bool _isEditForm;

        public bool IsEditForm
        {
            get
            {
                return _isEditForm;
            }
            set
            {
                _isEditForm = value;
                OnPropertyChanged();
            }
        }

        private bool _isFormInValid;

        public bool IsFormInValid
        {
            get
            {
                return _isFormInValid;
            }
            set
            {
                _isFormInValid = value;
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

        public IEnumerable<LookUp> StatusList
        {
            get
            {
                return _statusList;
            }
            set
            {
                _statusList = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<Item> _productsList;

        public IEnumerable<Item> ProductsList
        {
            get
            {
                return _productsList;
            }
            set
            {
                _productsList = value;
                OnPropertyChanged();
            }
        }

        private bool _isStandalone;

        public bool IsStandalone
        {
            get
            {
                return _isStandalone;
            }
            set
            {
                _isStandalone = value;
                OnPropertyChanged();
            }
        }

        private Inventory _inventoryItem;

        public Inventory InventoryItem
        {
            get
            {
                return _inventoryItem;
            }
            set
            {
                _inventoryItem = value;
                OnPropertyChanged();
            }
        }


        private ICommand _productSelectionChangedCommand;

        public ICommand ProductSelectionChangedCommand
        {
            get
            {
                return _productSelectionChangedCommand ?? (_productSelectionChangedCommand = new Command(ProductSelectionChanged));
            }
        }

        private void ProductSelectionChanged()
        {
            IsStandalone = (!InventoryItem.Item.IsAssetTagged && !InventoryItem.Item.IsSerialized && !InventoryItem.Item.IsLotNumbered);
        }

        private ICommand _saveInventoryCommand;

        public ICommand SaveInventoryCommand
        {
            get
            {
                return _saveInventoryCommand ?? (_saveInventoryCommand = new Command(SaveInventory));
            }
        }

        private async void SaveInventory()
        {
            try
            {
                ErrorMessage = null;
                IsFormInValid = false;
                this.IsLoading = true;
                if (ValidateForm())
                {
                    if (IsEditForm)
                    {
                        var inventoryPatchOperation = GetPatchOperations();

                        var updateInventoryResponse = await this._inventoryService.UpdatePhysicalInventory(inventoryPatchOperation, InventoryItem.Id);
                        if (updateInventoryResponse)
                        {
                            await this._navigationService.NavigateBackAsync();
                            this._toastMessageService.ShowToast($"Inventory is updated for item {InventoryItem.Item.Name}");
                        }
                        return;
                    }
                    var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();
                    if (currentSiteId == null)
                    {
                        throw new BadRequestException("Invalid Site location");
                    }
                    var success = await this._inventoryService.AdjustPhysicalInventoryAtLocation(this.InventoryItem, currentSiteId ?? 0);
                    if (success)
                    {
                        await this._navigationService.NavigateBackAsync();
                        this._toastMessageService.ShowToast($"Inventory is created for item {InventoryItem.Item.Name}");
                    }
                }
            }
            catch (ValidationException vex)
            {
                ErrorMessage = vex.Message;
                IsFormInValid = true;
            }
            catch (BadRequestException bex)
            {
                this._toastMessageService.ShowToast(bex.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                this._toastMessageService.ShowToast("Error while saving inventory");
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private IList<Operation> GetPatchOperations()
        {
            var fomattedInventoryItem = JObject.Parse(JsonConvert.SerializeObject(InventoryItem));

            var updatedItem = InventoryItem;
            updatedItem.StatusId = SelectedStatus.Id;
            updatedItem.CurrentLocationId = SelectedLocation.Id;

            var formattedUpdatedItem = JObject.Parse(JsonConvert.SerializeObject(updatedItem));

            var patchDiff = new JsonDiffPatch().Diff(fomattedInventoryItem, formattedUpdatedItem);
            return new JsonDeltaFormatter().Format(patchDiff);
        }

        protected virtual bool ValidateForm()
        {
            if (InventoryItem.Item == null)
            {
                throw new ValidationException("Please select item/product");
            }
            if (InventoryItem.Item.IsAssetTagged && string.IsNullOrEmpty(InventoryItem.AssetTagNumber))
            {
                throw new ValidationException("Please enter asset tag number");
            }
            if (InventoryItem.Item.IsSerialized && string.IsNullOrEmpty(InventoryItem.SerialNumber))
            {
                throw new ValidationException("Please enter serial number");
            }
            if (InventoryItem.Item.IsLotNumbered && string.IsNullOrEmpty(InventoryItem.LotNumber))
            {
                throw new ValidationException("Please enter lot number");
            }
            if (!(InventoryItem.Item.IsSerialized
                 || InventoryItem.Item.IsAssetTagged
                 || InventoryItem.Item.IsLotNumbered
                 ) && InventoryItem.Count <= 0
               )
            {
                throw new ValidationException("Quantity count should be more than 0");
            }
            return true;
        }
    }
}
