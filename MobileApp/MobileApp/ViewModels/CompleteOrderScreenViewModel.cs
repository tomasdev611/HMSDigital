using MobileApp.Assets.Enums;
using MobileApp.DataBaseAttributes;
using MobileApp.DataBaseAttributes.Repositories;
using MobileApp.Exceptions;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Pages.PopUpMenu;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class CompleteOrderScreenViewModel : BaseViewModel
    {
        private readonly DatabaseService<ScanItem> _databaseService;

        private readonly DispatchService _dispatchService;

        private readonly Dialer _dialer;

        private readonly PendingOrderRepository _pendingOrderRepository;

        public CompleteOrderScreenViewModel() : base()
        {
            _databaseService = new DatabaseService<ScanItem>();
            _dispatchService = new DispatchService();
            _dialer = new Dialer();
            _pendingOrderRepository = new PendingOrderRepository();
        }

        public override void InitializeViewModel(object data = null)
        {
            OrderData = (OrderData)data;
            InitializeCompleteOrderData();
        }

        private OrderData _orderData;

        public OrderData OrderData
        {
            get
            {
                return _orderData;
            }
            set
            {
                _orderData = value;
                OnPropertyChanged();
            }
        }

        private bool _isPartialFullfillment;

        public bool IsPartialFullfillment
        {
            get
            {
                return _isPartialFullfillment;
            }
            set
            {
                _isPartialFullfillment = value;
                OnPropertyChanged();
            }
        }

        private bool _isPickupOrder;

        public bool IsPickupOrder
        {
            get
            {
                return _isPickupOrder;
            }
            set
            {
                _isPickupOrder = value;
                OnPropertyChanged();
            }
        }

        private bool _isExceptionFulfillment = false;

        public bool IsExceptionFulfillment
        {
            get
            {
                return _isExceptionFulfillment;
            }
            set
            {
                _isExceptionFulfillment = value;
                OnPropertyChanged();
            }
        }

        private string _partialFulfillmentReason;

        public string PartialFulfillmentReason
        {
            get
            {
                return _partialFulfillmentReason;
            }
            set
            {
                _partialFulfillmentReason = value;
                OnPropertyChanged();
            }
        }

        private int _pickupCount;

        public int PickupCount
        {
            get
            {
                return _pickupCount;
            }
            set
            {
                _pickupCount = value;
                OnPropertyChanged();
            }
        }

        private int _dropCount;

        public int DropCount
        {
            get
            {
                return _dropCount;
            }
            set
            {
                _dropCount = value;
                OnPropertyChanged();
            }
        }

        private bool _isItemPick;

        public bool IsItemPick
        {
            get
            {
                return _isItemPick;
            }
            set
            {
                _isItemPick = value;
                OnPropertyChanged();
            }
        }

        private bool _isItemDrop;

        public bool IsItemDrop
        {
            get
            {
                return _isItemDrop;
            }
            set
            {
                _isItemDrop = value;
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

        private PendingOrder pendingOrder;

        public async void InitializeCompleteOrderData()
        {
            var scannedItems = await _databaseService.GetManyAsync(il => il.OrderId == OrderData.OrderID);
            PickupCount = scannedItems.Where(item => item.DispatchType == "Pickup").Sum(si => si.QuantityScanned);
            IsItemPick = PickupCount > 0;

            DropCount = scannedItems.Where(item => item.DispatchType == "Delivery").Sum(si => si.QuantityScanned);
            IsItemDrop = DropCount > 0;

            var fulfilledItemsList = await _dispatchService.GetFullfilledOrderItem(OrderData.OrderID);
            pendingOrder = await _pendingOrderRepository.GetAsync(po => po.OrderId == OrderData.OrderID
                                                                        && po.OrderTypeId == OrderData.OrderTypeId);
            var totalItemCount = OrderData.OrderLineItems.Sum(ol => ol.ItemCount);

            switch (OrderData.OrderTypeId)
            {
                case (int)OrderTypes.Delivery:
                case (int)OrderTypes.Respite:
                    fulfilledItemsList = fulfilledItemsList.Where(fi => string.Equals(fi.OrderType, "delivery", StringComparison.OrdinalIgnoreCase));
                    break;
                case (int)OrderTypes.Pickup:
                    IsPickupOrder = true;
                    fulfilledItemsList = fulfilledItemsList.Where(fi => string.Equals(fi.OrderType, "pickup", StringComparison.OrdinalIgnoreCase));
                    break;
                case (int)OrderTypes.Patient_Move:
                    if (pendingOrder != null && pendingOrder.IsMovePickupComplete)
                    {
                        fulfilledItemsList = fulfilledItemsList.Where(fi => string.Equals(fi.OrderType, "delivery", StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        fulfilledItemsList = fulfilledItemsList.Where(fi => string.Equals(fi.OrderType, "pickup", StringComparison.OrdinalIgnoreCase));
                        totalItemCount = OrderData.OrderLineItems.Where(ol => ol.ActionId == (int)OrderTypes.Pickup).Sum(ol => ol.ItemCount);
                    }
                    break;

            }
            var fulfilledItemCount = (fulfilledItemsList.Sum(fl => fl.Quantity)) + PickupCount + DropCount;

            if (fulfilledItemCount < totalItemCount)
            {
                IsPartialFullfillment = true;
            }

        }

        private ICommand _callContactPersonCommand;

        public ICommand CallContactPersonCommand
        {
            get
            {
                return _callContactPersonCommand ?? (_callContactPersonCommand = new Command(CallContactPerson));
            }
        }

        public async void CallContactPerson()
        {
            _dialer.OpenDialerWithNumber(_orderData.ContactNumber.ToString());
            await _navigationService.PushPopupAsync<CallReport>(_orderData.ContactNumber.ToString());
        }

        private ICommand _completeOrderCommand;

        public ICommand CompleteOrderCommand
        {
            get
            {
                return _completeOrderCommand ?? (_completeOrderCommand = new Command(CompleteOrder));
            }
        }

        private async Task<IEnumerable<FulfillmentItems>> GetDispatchItems()
        {
            var itemList = await _databaseService.GetManyAsync(il => il.OrderId == OrderData.OrderID);
            return itemList.Select(i =>
                 new FulfillmentItems
                 {
                     Count = i.QuantityScanned > 0 ? i.QuantityScanned : 1,
                     SerialNumber = i.SerialNumber ?? "",
                     FulfillmentType = i.DispatchType,
                     AssetTagNumber = i.AssetTag,
                     LotNumber = i.LotNumber,
                     ItemId = i.ItemId,
                     OrderLineItemId = i.OrderLineItemId
                 });
        }

        private async Task ChangePendingOrderStatus()
        {
            if (pendingOrder != null)
            {
                if (OrderData.OrderTypeId == (int)OrderTypes.Patient_Move)
                {
                    if (pendingOrder.IsMovePickupComplete)
                    {
                        pendingOrder.IsProcessed = true;
                    }
                    else
                    {
                        pendingOrder.IsMovePickupComplete = true;
                    }
                    if (IsPartialFullfillment || !pendingOrder.IsProcessed)
                    {
                        await _pendingOrderRepository.UpdateAsync(pendingOrder);
                    }
                    else if (pendingOrder.IsProcessed)
                    {
                        await _pendingOrderRepository.DeleteAsync(pendingOrder);
                    }
                }
                else
                {
                    if (IsPartialFullfillment)
                    {
                        pendingOrder.IsProcessed = true;
                        await _pendingOrderRepository.UpdateAsync(pendingOrder);
                    }
                    else
                    {
                        await _pendingOrderRepository.DeleteAsync(pendingOrder);
                    }
                }
            }
        }

        public async void CompleteOrder()
        {
            if (IsPartialFullfillment && string.IsNullOrEmpty(PartialFulfillmentReason))
            {
                _toastMessageService.ShowToast("Please Enter reason for partial fullfillment");
                return;
            }

            IsLoading = true;
            try
            {
                var dispatchItemsList = await GetDispatchItems();
                var dispatchResult = await _dispatchService.SendFullfillOrderRequestAsync(dispatchItemsList.ToList(), OrderData, PartialFulfillmentReason, IsExceptionFulfillment);
                if (dispatchResult)
                {
                    await ChangePendingOrderStatus();
                    await _databaseService.DeleteAllAsync();
                    await _navigationService.NavigateToAsync<DriverBreakScreen>(OrderData);
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
    }
}