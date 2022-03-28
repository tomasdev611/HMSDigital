using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.DataBaseAttributes;
using MobileApp.Exceptions;
using MobileApp.Models;
using MobileApp.Pages.CommonPages;
using MobileApp.Pages.PopUpMenu;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using MobileApp.Utils;
using MobileApp.Utils.Validator;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LoadListScanViewModel : BaseViewModel
    {
        private ICommand _finishLoadingCommand;

        private ICommand _undoScanCommand;

        private readonly DispatchService _dispatchService;

        private readonly DatabaseService<ScanItem> _databaseService;

        private readonly StorageService _storageService;

        private VehicleLoadlist _vehicleLoadlist;

        private ScannableLoadListGroup _itemToLoad;

        public LoadListScanViewModel() : base()
        {
            _databaseService = new DatabaseService<ScanItem>();
            _dispatchService = new DispatchService();
            _storageService = new StorageService();
        }

        public override void InitializeViewModel(object data = null)
        {
            subscribeMessages();
            _vehicleLoadlist = LoadListUtils.RemoveFulfilledItem((VehicleLoadlist)data);
            GetLoadList();
        }

        public override void DestroyViewModel()
        {
            UnsubscribeMessagingCenter();
        }

        private void UnsubscribeMessagingCenter()
        {
            MessagingCenter.Unsubscribe<Inventory>(this, MessagingConstant.SCANNED_INVENTORY);
            MessagingCenter.Unsubscribe<string>(this, MessagingConstant.UNDO_SCANNED_INVENTORY);
        }

        private void subscribeMessages()
        {
            MessagingCenter.Subscribe<Inventory>(this, MessagingConstant.SCANNED_INVENTORY, AddInventoryInDatabase);
            MessagingCenter.Subscribe<string>(this, MessagingConstant.UNDO_SCANNED_INVENTORY, (undo) =>
            {
                GetLoadList();
            });
        }

        ObservableCollection<ScannableLoadListGroup> _loadListGroup;

        public ObservableCollection<ScannableLoadListGroup> LoadListGroup
        {
            get
            {
                return _loadListGroup;
            }
            set
            {
                _loadListGroup = value;
                OnPropertyChanged();
            }
        }

        bool _isLoading;
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

        int _totalItemsToLoad;
        public int TotalItemsToLoad
        {
            get
            {
                return _totalItemsToLoad;
            }
            set
            {
                _totalItemsToLoad = value;
                OnPropertyChanged();
            }
        }

        int _totalItemsLoaded;
        public int TotalItemsLoaded
        {
            get
            {
                return _totalItemsLoaded;
            }
            set
            {
                _totalItemsLoaded = value;
                OnPropertyChanged();
            }
        }

        ScanItem _itemToUndo;
        public ScanItem ItemToUndo
        {
            get
            {
                return _itemToUndo;
            }
            set
            {
                _itemToUndo = value;
                OnPropertyChanged();
            }
        }

        public ICommand UndoScanCommand
        {
            get
            {
                return _undoScanCommand ?? (_undoScanCommand = new Command(ID =>
                {
                    var id = ID.ToString();
                    UndoScan(int.Parse(id));
                }));
            }
        }

        private async void UndoScan(int ID)
        {
            ItemToUndo = await _databaseService.GetAsync(i => i.ID == ID);
            if (ItemToUndo != null)
            {
                await _navigationService.PushPopupAsync<UndoPopUp>(ItemToUndo);
            }
        }

        public ICommand FinishLoadingCommand
        {
            get
            {
                return _finishLoadingCommand ?? (_finishLoadingCommand = new Command(FinishLoading));
            }
        }

        private async void FinishLoading()
        {
            IsLoading = true;
            try
            {
                var itemList = await _databaseService.GetManyAsync(sci => sci.VehicleId == _vehicleLoadlist.VehicleId);
                var dispatchItemsList = itemList.Select(i =>
                      new DispatchItems
                      {
                          Count = i.QuantityScanned > 0 ? i.QuantityScanned : 1,
                          SerialNumber = i.SerialNumber ?? "",
                          AssetTagNumber = i.AssetTag,
                          LotNumber = i.LotNumber,
                          ItemId = i.ItemId
                      });

                var pickupResult = await _dispatchService.SendPickupRequestAsync(dispatchItemsList, _vehicleLoadlist.Vehicle.SiteId ?? 0, "loadlist", _vehicleLoadlist.Vehicle.Id);

                if (pickupResult)
                {
                    _storageService.AddToStorage(StorageConstants.LOADED_ITEM_COUNT, TotalItemsLoaded.ToString());
                    await _databaseService.DeleteAllAsync();
                    _toastMessageService.ShowToast("LoadList pickup completed");

                    await _navigationService.ChangeDetailPageAsync<Dashboard>();

                    return;
                }
            }
            catch (PermissionRequiredException prex)
            {
                _toastMessageService.ShowToast(prex.Message);
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

        private ICommand _scanAndLoadCommand;

        public ICommand ScanAndLoadCommand
        {
            get
            {
                return _scanAndLoadCommand ?? (_scanAndLoadCommand = new Command(ScanAndLoad));
            }
        }

        private async void ScanAndLoad(object item)
        {
            try
            {
                _itemToLoad = (ScannableLoadListGroup)item;

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
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                return;
            }
        }

        private ICommand _loadManuallyCommand;

        public ICommand LoadManuallyCommand
        {
            get
            {
                return _loadManuallyCommand ?? (_loadManuallyCommand = new Command(LoadManually));
            }
        }

        private async void LoadManually(object item)
        {
            try
            {
                _itemToLoad = (ScannableLoadListGroup)item;
                await _navigationService.PushPopupAsync<ManualScannerPopup>(_itemToLoad);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                return;
            }
        }

        private async void GetLoadList()
        {
            var dbItemsList = await _databaseService.GetManyAsync(sci => sci.VehicleId == _vehicleLoadlist.VehicleId);

            var loadList = new ObservableCollection<ScannableLoadListGroup>();

            foreach (var item in _vehicleLoadlist.Items)
            {
                var scannedItems = dbItemsList.Where(di => di.ItemId == item.ItemId);
                var equipmentSettings = GetEquipmentSettingsForItem(item.ItemId);
                loadList.Add(new ScannableLoadListGroup
                        (
                            scannedItems.ToList(),
                            item,
                            equipmentSettings.ToList()
                        )
                    );
            }

            LoadListGroup = loadList;
            TotalItemsToLoad = _vehicleLoadlist.Items.Sum(li => li.ItemCount);
            TotalItemsLoaded = dbItemsList.Sum(di => di.QuantityScanned);
        }

        private IEnumerable<JArray> GetEquipmentSettingsForItem(int itemId)
        {
            var orderLineItems = _vehicleLoadlist.Orders.SelectMany(o => o.OrderLineItems).Where(ol => ol.ItemId == itemId);
            return orderLineItems.Where(ol => ol.EquipmentSettings.Count() > 0).Select(ol => ol.EquipmentSettings);
        }

        private async void AddInventoryInDatabase(Inventory inventory)
        {
            try
            {
                if (_itemToLoad == null)
                    return;

                if (ScannedItemValidator.ValidateScannedItemWithId(_itemToLoad.ItemId, inventory))
                {
                    var itemsWithAssetTag = await _databaseService.GetManyAsync(sci => sci.AssetTag.ToLower() == inventory.AssetTagNumber.ToLower()
                                                                          || sci.SerialNumber.ToLower() == inventory.SerialNumber.ToLower()
                                                                          || sci.LotNumber.ToLower() == inventory.LotNumber.ToLower());
                    if (itemsWithAssetTag.Count == 0)
                    {
                        var scannedItem = new ScanItem
                        {
                            AssetTag = inventory.AssetTagNumber,
                            LotNumber = inventory.LotNumber,
                            SerialNumber = inventory.SerialNumber,
                            IsScanned = true,
                            ItemId = inventory.ItemId,
                            QuantityScanned = inventory.Count,
                            VehicleId = _vehicleLoadlist.VehicleId,
                        };
                        await _databaseService.SaveAsync(scannedItem);

                        LoadListGroup.FirstOrDefault(lg => lg.ItemId == _itemToLoad.ItemId).Add(scannedItem);
                        TotalItemsLoaded += inventory.Count;
                    }
                }
            }
            catch (ValidationException ex)
            {
                _toastMessageService.ShowToast(ex.Message, ToastMessageDuration.Long);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Not able to complete the request at the moment. Please try again", ToastMessageDuration.Long);
            }
        }
    }
}
