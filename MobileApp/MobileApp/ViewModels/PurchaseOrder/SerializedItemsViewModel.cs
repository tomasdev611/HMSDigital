using System.Windows.Input;
using Xamarin.Forms;
using MobileApp.Interface.Services;
using MobileApp.Models;
using MobileApp.Service;
using System;
using Xamarin.Essentials;
using MobileApp.Pages.CommonPages;
using MobileApp.Assets.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace MobileApp.ViewModels.PurchaseOrder
{
    public class SerializedItemsViewModel : BaseViewModel
    {
        #region Private Properties

        private readonly IPurchaseOrderService _purchaseOrderService;
        private PurchaseOrderLineItemModel _selectedLineItemModel;
        private ObservableCollection<AssetTagsModel> _assetTagModelList;

        private bool _isRefreshing = true;

        private ICommand _closeCommand;
        private ICommand _doneCommand;
        private ICommand _scanCommand;

        #endregion

        /// <summary>
        /// Is Refreshing the View
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected LineItemModel
        /// </summary>
        public PurchaseOrderLineItemModel SelectedLineItemModel
        {
            get
            {
                return _selectedLineItemModel;
            }
            set
            {
                _selectedLineItemModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Asset Tags & Models
        /// </summary>
        public ObservableCollection<AssetTagsModel> AssetTagsModelList
        {
            get
            {
                return _assetTagModelList;
            }
            set
            {
                _assetTagModelList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get the Purchase Orders
        /// </summary>
        public ICommand CloseSheetCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(CloseCommandExecute));
            }
        }

        /// <summary>
        /// Get the Purchase Orders
        /// </summary>
        public ICommand DoneCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(DoneCommandExecute));
            }
        }

        /// <summary>
        /// Scan Command
        /// </summary>
        public ICommand ScanCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(ExecuteScanCommand));
            }
        }


        public SerializedItemsViewModel() : base()
        {
            _purchaseOrderService = new InventoryManagementService();
            PageTitle = "Scan Asset Tag";
        }

        public override void InitializeViewModel(object data = null)
        {
            var lineItem = (PurchaseOrderLineItemModel)data;
            SelectedLineItemModel = lineItem;
            SubscribeMessages();
        }

        public override void DestroyViewModel()
        {
            UnsubscribeMessagingCenter();
        }

        private void UnsubscribeMessagingCenter()
        {
            MessagingCenter.Unsubscribe<Inventory>(this, MessagingConstant.SCANNED_INVENTORY);
        }

        private void SubscribeMessages()
        {
            MessagingCenter.Subscribe<Inventory>(this, MessagingConstant.SCANNED_INVENTORY, AddInventoryInDatabase);
        }

        private async void CloseCommandExecute(object obj)
        {
            // TODO: Pending return items to normal state.
            // Fix will be ready in next PBI for Trucks
            await _navigationService.NavigateBackAsync();
        }

        private async void DoneCommandExecute(object obj)
        {
            await _navigationService.NavigateBackAsync();
        }

        private void AddInventoryInDatabase(Inventory inventory)
        {
            if (AssetTagsModelList == null)
            {
                AssetTagsModelList = new ObservableCollection<AssetTagsModel>();
            }

            AssetTagsModelList.Add(new AssetTagsModel()
            {
                AssetTag = inventory.AssetTagNumber,
                SerialNumber = inventory.SerialNumber
            });
        }

        private async void ExecuteScanCommand(object obj)
        {
            try
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
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                return;
            }
        }
    }
}
