using MobileApp.Assets.Constants;
using MobileApp.DataBaseAttributes;
using MobileApp.DataBaseAttributes.Repositories;
using MobileApp.Models;
using MobileApp.Pages.CommonPages;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MobileApp.ViewModels
{
    class InventoryAuditViewModel : BaseViewModel
    {
        private readonly InventoryService _inventoryService;
        public InventoryAuditViewModel() : base()
        {
            _inventoryService = new InventoryService();
        }

        public override void InitializeViewModel(object data = null)
        {
            MessagingCenter.Subscribe<Inventory>(this, MessagingConstant.SCANNED_INVENTORY, (inventory) =>
            {
                InventoryItem = inventory;
                if (InventoryItem != null)
                {
                    LogScanHistoryItem(InventoryItem);
                    ShouldShowErrorMessage = false;
                    return;
                }
                else
                {
                    ShouldShowErrorMessage = true;
                }
            });
        }

        public override void DestroyViewModel()
        {
            MessagingCenter.Unsubscribe<Inventory>(this, MessagingConstant.SCANNED_INVENTORY);
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

        private ObservableCollection<Inventory> _scanHistoryList = new ObservableCollection<Inventory>();

        public ObservableCollection<Inventory> ScanHistoryList
        {
            get
            {
                return _scanHistoryList;
            }
            set
            {
                _scanHistoryList = value;
                OnPropertyChanged();
            }
        }

        private bool _shouldShowErrorMessage;

        public bool ShouldShowErrorMessage
        {
            get
            {
                return _shouldShowErrorMessage;
            }
            set
            {
                _shouldShowErrorMessage = value;
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

        private string _assetTag;

        public string AssetTag
        {
            get
            {
                return _assetTag;
            }
            set
            {
                _assetTag = value;
                OnPropertyChanged();
            }
        }

        private ICommand _getInventoryForAssetTagCommand;

        public ICommand GetInventoryForAssetTagCommand
        {
            get
            {
                return _getInventoryForAssetTagCommand ?? (_getInventoryForAssetTagCommand = new Command(GetInventoryForAssetTag));
            }
        }

        private ICommand _goToInventoryScanCommand;

        public ICommand GoToInventoryScanCommand
        {
            get
            {
                return _goToInventoryScanCommand ?? (_goToInventoryScanCommand = new Command(GoToInventoryScan));
            }
        }

        private async void GoToInventoryScan()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if(status == PermissionStatus.Granted)
            {
                await _navigationService.NavigateToAsync<BarcodeScanner>();
            }
            else if(status == PermissionStatus.Denied)
            {
                _toastMessageService.ShowToast("You should enable Camera Permission in order to scan");
            }
            else
            {
                var permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if(permissionStatus == PermissionStatus.Granted)
                    await _navigationService.NavigateToAsync<BarcodeScanner>();
            }


            await _navigationService.NavigateToAsync<BarcodeScanner>();
        }

        private async void GetInventoryForAssetTag()
        {
            try
            {
                IsLoading = true;
                var filterString = $"(serialNumber | assetTagNumber| lotNumber) == {AssetTag}";
                InventoryItem = (await _inventoryService.GetInventory(filterString)).FirstOrDefault();
                if (InventoryItem != null)
                {
                    LogScanHistoryItem(InventoryItem);
                    ShouldShowErrorMessage = false;
                    return;
                }
                ShouldShowErrorMessage = true;
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                ShouldShowErrorMessage = true;
                _toastMessageService.ShowToast("Error occured while fetching the inventory details");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LogScanHistoryItem(Inventory inventoryItem)
        {
            try
            {
               ScanHistoryList.Insert(0, inventoryItem);
            }
            catch
            {
                _toastMessageService.ShowToast("History log failed");
            }
        }

        private ICommand _addNewItemCommand;

        public ICommand AddNewItemCommand
        {
            get
            {
                return _addNewItemCommand ?? (_addNewItemCommand = new Command(AddNewItem));
            }
        }

        public async void AddNewItem()
        {
            await _navigationService.NavigateToAsync<AddInventoryItem>();
        }

        private ICommand _editInventoryCommand;

        public ICommand EditInventoryCommand
        {
            get
            {
                return _editInventoryCommand ?? (_editInventoryCommand = new Command(EditInventory));
            }
        }

        private async void EditInventory()
        {
            await _navigationService.NavigateToAsync<AddInventoryItem>(InventoryItem);
        }
    }
}
