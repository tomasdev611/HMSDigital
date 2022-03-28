using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Assets.Enums;
using MobileApp.DataBaseAttributes;
using MobileApp.DataBaseAttributes.Repositories;
using MobileApp.Exceptions;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Pages.CommonPages;
using MobileApp.Pages.PopUpMenu;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using MobileApp.Utils.Validator;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class OrderListScanViewModel : BaseViewModel
    {
        private readonly DatabaseService<ScanItem> _databaseService;

        private readonly DispatchService _dispatchService;

        private readonly Dialer _dialer;

        private ScannableOrderList _itemToScan;

        private readonly PendingOrderRepository _pendingOrderRepository;

        public OrderListScanViewModel() : base()
        {
            _databaseService = new DatabaseService<ScanItem>();
            _dispatchService = new DispatchService();
            _pendingOrderRepository = new PendingOrderRepository();
            _dialer = new Dialer();
        }

        public override void InitializeViewModel(object data)
        {
            subscribeMessages();
            OrderDetail = (OrderData)data;
            GetOrderLineItems();
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
                GetOrderLineItems();
            });
        }

        ObservableCollection<ScannableOrderList> _orderLineItemGroup;

        public ObservableCollection<ScannableOrderList> OrderLineItemsGroup
        {
            get
            {
                return _orderLineItemGroup;
            }
            set
            {
                _orderLineItemGroup = value;
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

        OrderData _orderDetail;
        public OrderData OrderDetail
        {
            get
            {
                return _orderDetail;
            }
            set
            {
                _orderDetail = value;
                OnPropertyChanged();
            }
        }

        int _totalItemsToScan;
        public int TotalItemsToScan
        {
            get
            {
                return _totalItemsToScan;
            }
            set
            {
                _totalItemsToScan = value;
                OnPropertyChanged();
            }
        }

        int _totalItemsScanned;
        public int TotalItemsScanned
        {
            get
            {
                return _totalItemsScanned;
            }
            set
            {
                _totalItemsScanned = value;
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

        private ICommand _showMoreOrderNotesCommand;

        public ICommand ShowMoreOrderNotesCommand
        {
            get
            {
                return _showMoreOrderNotesCommand ?? (_showMoreOrderNotesCommand = new Command(ShowMoreOrderNotes));
            }
        }

        public async void ShowMoreOrderNotes()
        {
            await _navigationService.PushPopupAsync<DetailedOrderNotes>(OrderDetail.OrderNotes);
        }

        private ICommand _showMorePatientNotesCommand;

        public ICommand ShowMorePatientNotesCommand
        {
            get
            {
                return _showMorePatientNotesCommand ?? (_showMorePatientNotesCommand = new Command(ShowMorePatientNotes));
            }
        }

        public async void ShowMorePatientNotes()
        {
            await _navigationService.PushPopupAsync<DetailedOrderNotes>(OrderDetail.PatientNotes);
        }

        private ICommand _finishLoadingCommand;

        public ICommand FinishLoadingCommand
        {
            get
            {
                return _finishLoadingCommand ?? (_finishLoadingCommand = new Command(FinishLoading));
            }
        }

        private async void FinishLoading()
        {
            await _navigationService.NavigateToAsync<CompleteOrderScreen>(OrderDetail);
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

        private ICommand _undoScanCommand;

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
            var itemToUndo = await _databaseService.GetAsync(i => i.ID == ID);
            await _navigationService.PushPopupAsync<UndoPopUp>(itemToUndo);
        }

        private ICommand _callContactCommand;

        public ICommand CallContactCommand
        {
            get
            {
                return _callContactCommand ?? (_callContactCommand = new Command(CallContact));
            }
        }

        private async void CallContact()
        {
            _dialer.OpenDialerWithNumber(OrderDetail.ContactNumber.ToString());
            await _navigationService.PushPopupAsync<CallReport>(OrderDetail.ContactNumber.ToString());
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
            if (item == null)
                return;

            try
            {
                _itemToScan = (ScannableOrderList)item;
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

        private ICommand _addManuallyCommand;

        public ICommand AddManuallyCommand
        {
            get
            {
                return _addManuallyCommand ?? (_addManuallyCommand = new Command(AddManually));
            }
        }

        private async void AddManually(object item)
        {
            if (item == null)
                return;

            try
            {
                _itemToScan = (ScannableOrderList)item;
                await _navigationService.PushPopupAsync<ManualScannerPopup>(_itemToScan);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                return;
            }
        }

        private async void AddInventoryInDatabase(Inventory inventory)
        {
            try
            {
                if (ScannedItemValidator.ValidateScannedItemWithId(_itemToScan.ItemId, inventory))
                {
                    var itemsWithAssetTag = await _databaseService.GetManyAsync(sci => sci.AssetTag.ToLower() == inventory.AssetTagNumber.ToLower()
                                                                          || sci.SerialNumber.ToLower() == inventory.SerialNumber.ToLower()
                                                                          || sci.LotNumber.ToLower() == inventory.LotNumber.ToLower());
                    if (itemsWithAssetTag.Count == 0)
                    {
                        if (_itemToScan.OrderLineItemId > 0 && !string.IsNullOrEmpty(_itemToScan.DispatchType))
                        {
                            var scannedItem = new ScanItem
                            {
                                AssetTag = inventory.AssetTagNumber,
                                LotNumber = inventory.LotNumber,
                                SerialNumber = inventory.SerialNumber,
                                IsScanned = true,
                                ItemId = inventory.ItemId,
                                OrderLineItemId = _itemToScan.OrderLineItemId,
                                DispatchType = _itemToScan.DispatchType,
                                QuantityScanned = inventory.Count,
                                OrderId = OrderDetail.OrderID
                            };

                            await _databaseService.SaveAsync(scannedItem);

                            OrderLineItemsGroup.FirstOrDefault(ol => ol.OrderLineItemId == _itemToScan.OrderLineItemId).Add(scannedItem);
                            TotalItemsScanned += inventory.Count;
                        }
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

        private async Task<IEnumerable<OrderLineItem>> GetMoveOrdeLineItems()
        {
            var pendingOrder = await _pendingOrderRepository.GetAsync(po =>
                        po.OrderId == OrderDetail.OrderID
                        && po.OrderTypeId == OrderDetail.OrderTypeId
                    );
            var orderLineItems = OrderDetail.OrderLineItems;
            if (pendingOrder.IsMovePickupComplete)
            {
                orderLineItems = orderLineItems.Where(oli => oli.ActionId == (int)OrderTypes.Delivery);
            }
            else
            {
                orderLineItems = orderLineItems.Where(oli => oli.ActionId == (int)OrderTypes.Pickup);
            }
            return orderLineItems;
        }

        private async void GetOrderLineItems()
        {
            try
            {
                IsLoading = true;

                ShippingAddress = CommonUtility.AddressToString(OrderDetail.ShippingAddress);

                var dbItemsList = await _databaseService.GetManyAsync(item => item.OrderId == OrderDetail.OrderID);
                var fulfilledItemsList = await _dispatchService.GetFullfilledOrderItem(OrderDetail.OrderID);

                var groupList = new ObservableCollection<ScannableOrderList>();

                var orderLineItems = OrderDetail.OrderLineItems;
                if (OrderDetail.OrderTypeId == (int)OrderTypes.Patient_Move)
                {
                    orderLineItems = await GetMoveOrdeLineItems();
                }

                foreach (var item in orderLineItems)
                {
                    var group = dbItemsList.Where(di => di.OrderLineItemId == item.Id).ToList();

                    group.AddRange(fulfilledItemsList.Where(fi => fi.OrderLineItemId == item.Id).Select(i =>
                    {
                        return new ScanItem
                        {
                            AssetTag = i.AssetTag,
                            LotNumber = i.LotNumber,
                            SerialNumber = i.SerialNumber,
                            QuantityScanned = i.Quantity,
                            IsCompleted = true
                        };
                    }));

                    groupList.Add(new ScannableOrderList(
                        group.ToList(),
                        item,
                        OrderDetail.PatientUuId,
                        item.ItemCount - fulfilledItemsList.Where(fi => (fi.ItemName == item.Item.Name && fi.OrderLineItemId == item.Id)).Sum(fi => fi.Quantity)
                        ));
                }

                OrderLineItemsGroup = groupList;
                TotalItemsToScan = groupList.Sum(gl => gl.QuantityToFulfill);
                TotalItemsScanned = dbItemsList.Where(di => di.OrderId == OrderDetail.OrderID).Sum(di => di.QuantityScanned);
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                return;
            }
        }
    }
}
