using System;
using MobileApp.Interface.Services;
using MobileApp.Models;
using MobileApp.Service;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using MobileApp.Pages.Screen.PurchaseOrder;
using MobileApp.Utils;
using System.Collections.Generic;
using MvvmHelpers;

namespace MobileApp.ViewModels.InventoryManagement
{
    public class InventoryReceiveViewModel : BaseViewModel
    {
        #region Private Properties

        private readonly IInventoryService _inventoryService;

        private bool _isEmptyView = false;
        private bool _isOpenSheet = false;
        private bool _canCompleteReceipt = false;
        private TransferOrder _transferOrder;
        private string _searchText;
        private bool _isRefreshing = true;

        private ObservableRangeCollection<PurchaseOrderLineItemModel> _lineItemsList;
        private PurchaseOrderLineItemModel _selectedLineItem;
        private List<PurchaseOrderLineItemModel> _originalLineItemsList;
        private bool _isFulfillOrder;

        private ICommand _closeSheetCommand;
        private ICommand _selectedReceiptCommand;
        private ICommand _cancelCommand;
        private ICommand _completeReceiptCommand;
        private ICommand _receiveLineItemCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// Validate if the View is Empty
        /// </summary>
        public bool IsEmptyView
        {
            get
            {
                return _isEmptyView;
            }
            set
            {
                _isEmptyView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Validate if the Sheet View is Open
        /// </summary>
        public bool IsOpenSheet
        {
            get
            {
                return _isOpenSheet;
            }
            set
            {
                _isOpenSheet = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Validate if can complete the oorder
        /// </summary>
        public bool CanCompleteReceipt
        {
            get
            {
                return _canCompleteReceipt;
            }
            set
            {
                _canCompleteReceipt = value;
                OnPropertyChanged();
            }
        }

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

        public TransferOrder TransferOrder
        {
            get
            {
                return _transferOrder;
            }
            set
            {
                _transferOrder = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Line Item
        /// </summary>
        public PurchaseOrderLineItemModel SelectedLineItem
        {
            get
            {
                return _selectedLineItem;
            }
            set
            {
                _selectedLineItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection of Purchase Orders
        /// </summary>
        public ObservableRangeCollection<PurchaseOrderLineItemModel> LineItemsList
        {
            get
            {
                return _lineItemsList;
            }
            set
            {
                _lineItemsList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Search Text
        /// </summary>
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                if (!String.IsNullOrEmpty(SearchText) && SearchText.Length > 0)
                {
                    if (LineItemsList == null)
                    {
                        LineItemsList = new ObservableRangeCollection<PurchaseOrderLineItemModel>();
                    }

                    LineItemsList.Clear();

                    var values = _originalLineItemsList.Where(x => x.ItemDescription.ToLower().Contains(SearchText.ToLower())).ToList();
                    if (values != null && values.Count > 0)
                    {
                        LineItemsList.ReplaceRange(values);
                    }
                }
                else
                {
                    LineItemsList.ReplaceRange(_originalLineItemsList);
                }
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
                return _closeSheetCommand ?? (_closeSheetCommand = new Command(CloseSheetCommandExecute));
            }
        }

        /// <summary>
        /// Select the Line Item in the list
        /// </summary>
        public ICommand ReceiveLineItemCommand
        {
            get
            {
                return _receiveLineItemCommand ?? (_receiveLineItemCommand = new Command<PurchaseOrderLineItemModel>(OnSelectedReceiptExecute));
            }
        }

        /// <summary>
        /// Get the Cancel Command
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new Command(ExecuteCancelCommand));
            }
        }

        /// <summary>
        /// Complete Receipt Command
        /// </summary>
        public ICommand CompleteReceiptCommand
        {
            get
            {
                return _completeReceiptCommand ?? (_completeReceiptCommand = new Command(ExecuteCompleteReceiptCommand));
            }
        }

        #endregion

        public InventoryReceiveViewModel() : base()
        {
            _inventoryService = new InventoryManagementService();
            PageTitle = "Inventory Receive";
        }

        public override void InitializeViewModel(object data = null)
        {
            TransferOrder = (TransferOrder)data;
            GetIntialData();
        }

        private void CloseSheetCommandExecute()
        {
            IsOpenSheet = false;
        }

        private async void ExecuteCancelCommand()
        {
            await _navigationService.NavigateBackAsync();
            SelectedLineItem = null;
        }

        private async void ExecuteCompleteReceiptCommand()
        {
            if (!CanCompleteReceipt)
            {
                return;
            }

            var request = new FulfillReceiveOrderRequest()
            {
                OrderItemLines = TransferOrder.OrderLineItems,
                IsFulfill = _isFulfillOrder,
                NetSuitePurchaseOrderId = TransferOrder.NetSuiteTransferOrderId
            };

            var orders = await _inventoryService.FulfillReceiveOrder(TransferOrder.NetSuiteTransferOrderId, request);
            if (orders != null)
            {
                if (orders.TransferOrderStatus == "completed")
                {
                    (App.Current as App).NavigationBackParameter = orders;
                }
            }

            await _navigationService.NavigateBackAsync();
        }

        /// <summary>
        /// Retrieve the initial data for the detail view
        /// </summary>
        private void GetIntialData()
        {
            IsRefreshing = true;

            if (TransferOrder.OrderLineItems != null && TransferOrder.OrderLineItems.Count() > 0)
            {
                if (LineItemsList == null)
                {
                    LineItemsList = new ObservableRangeCollection<PurchaseOrderLineItemModel>();
                }
                else
                {
                    LineItemsList.Clear();
                }

                // Add to the list the information for the Line Items
                foreach (var item in TransferOrder.OrderLineItems)
                {
                    LineItemsList.Add(item);
                }

                _originalLineItemsList = LineItemsList.ToList();

                CanCompleteReceipt = _originalLineItemsList.Any(x => x.QuantityRecieved > 0);
                OnPropertyChanged(nameof(LineItemsList));
            }

            IsRefreshing = false;
        }

        /// <summary>
        /// Action to Navigate to Line Item Receive
        /// </summary>
        private async void OnSelectedReceiptExecute(PurchaseOrderLineItemModel selectedLineItem)
        {
            if (selectedLineItem == null)
            {
                return;
            }

            // Navigate to the detail
            if (selectedLineItem.IsSerial)
            {
                await _navigationService.NavigateToAsync<SerializedItemsPage>(selectedLineItem);
            }
            else
            {
                await _navigationService.NavigateToAsync<QuantityItemPage>(selectedLineItem);
            }

            SelectedLineItem = null;
        }
    }
}
