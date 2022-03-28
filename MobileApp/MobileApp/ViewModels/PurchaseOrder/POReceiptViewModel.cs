using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MobileApp.Interface.Services;
using MobileApp.Models;
using MobileApp.Service;
using System.Linq;
using MobileApp.Utils;
using System.Windows.Input;
using Xamarin.Forms;
using MvvmHelpers;
using System.Collections.Generic;
using MobileApp.Pages.Screen.PurchaseOrder;

namespace MobileApp.ViewModels.PurchaseOrder
{
    public class POReceiptViewModel : BaseViewModel
    {
        #region Private Properties

        private readonly IPurchaseOrderService _purchaseOrderService;

        private bool _isEmptyView = false;
        private bool _isOpenSheet = false;
        private bool _canCompleteReceipt = false;
        private PurchaseOrderModel _purchaseOrder;
        private string _searchText;
        private bool _isRefreshing = true;
        private ObservableRangeCollection<PurchaseOrderReceiptModel> _receiptsList;
        private PurchaseOrderReceiptModel _selectedReceipt;

        private ObservableRangeCollection<PurchaseOrderLineItemModel> _lineItemsList;
        private PurchaseOrderLineItemModel _selectedLineItem;
        private List<PurchaseOrderLineItemModel> _originalLineItemsList;

        private ICommand _cancelCommand;
        private ICommand _receiveLineItemCommand;
        private ICommand _viewReceiptCommand;
        private ICommand _attatchReceiptCommand;
        private ICommand _completeReceiptCommand;

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
        /// Validate if the View is Empty
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

        public PurchaseOrderModel PurchaseOrder
        {
            get
            {
                return _purchaseOrder;
            }
            set
            {
                _purchaseOrder = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Purchase Order
        /// </summary>
        public PurchaseOrderReceiptModel SelectedReceipt
        {
            get
            {
                return _selectedReceipt;
            }
            set
            {
                _selectedReceipt = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection of Purchase Orders
        /// </summary>
        public ObservableRangeCollection<PurchaseOrderReceiptModel> ReceiptsList
        {
            get
            {
                return _receiptsList;
            }
            set
            {
                _receiptsList = value;
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

        public POReceiptViewModel() : base()
        {
            _purchaseOrderService = new InventoryManagementService();
            PageTitle = Strings.PurchaseOrderReceiptTitle;
        }

        public override void InitializeViewModel(object data = null)
        {
            PurchaseOrder = (PurchaseOrderModel)data;
            GetIntialData();
        }

        private async void ExecuteCancelCommand()
        {
            await _navigationService.NavigateBackAsync();
            SelectedReceipt = null;
        }

        private async void ExecuteCompleteReceiptCommand()
        {
            if(!CanCompleteReceipt)
            {
                return;
            }

            var request = new ReceivePurchaseOrderRequest()
            {
                ImageUrls = null,
                ItemLines = LineItemsList,
                NetSuitePurchaseOrderId = PurchaseOrder.InternalId
            };

            var orders = await _purchaseOrderService.ReceivePurchaseOrder(PurchaseOrder.InternalId, request);
            if (orders != null)
            {
                if (orders.Status == "completed")
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

            if(PurchaseOrder.PurchaseOrderLineItems != null && PurchaseOrder.PurchaseOrderLineItems.Count() > 0)
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
                foreach (var item in PurchaseOrder.PurchaseOrderLineItems)
                {
                    LineItemsList.Add(item);
                }

                _originalLineItemsList = LineItemsList.ToList();

                CanCompleteReceipt = _originalLineItemsList.Any(x => x.QuantityRecieved > 0);
                OnPropertyChanged(nameof(LineItemsList));
            }

            IsRefreshing = false;
        }

        private async Task GetPurchaseOrderReceipts()
        {
            // Get the Purchase Order Receipts
            try
            {
                var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();
                if (!currentSiteId.HasValue)
                {
                    IsRefreshing = false;
                    IsEmptyView = true;
                    return;
                }

                var orders = await _purchaseOrderService.GetPurchaseOrderReceipts(PurchaseOrder.PurchaseOrderNumber, currentSiteId.Value);

                if (ReceiptsList == null)
                    ReceiptsList = new ObservableRangeCollection<PurchaseOrderReceiptModel>();
                else
                    ReceiptsList.Clear();

                foreach (var item in orders)
                {
                    ReceiptsList.Add(item);
                }
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
            }
        }

        /// <summary>
        /// Action to Navigate to the PO Receipt Page
        /// </summary>
        private async void OnSelectedReceiptExecute(PurchaseOrderLineItemModel selectedLineItem)
        {
            if(selectedLineItem == null)
            {
                return;
            }

            // Navigate to the detail
            if(selectedLineItem.IsSerial)
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