using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
using MobileApp.Pages.Screen.InventoryManagement;

namespace MobileApp.ViewModels.InventoryManagement
{
    public class InventoryTransferMainViewModel : BaseViewModel
    {
        #region Private Properties

        private readonly IInventoryService _inventoryService;
        private bool _isEmptyView = false;
        private string _searchText;
        private ObservableRangeCollection<TransferOrder> _pendingReceiveOrders;
        private TransferOrder _selectedTransferOrder;
        private ICommand _refreshCommand;
        private ICommand _selectedTransferOrderCommand;
        private ICommand _createTransferOrderCommand;
        private bool _isRefreshing = true;

        private List<TransferOrder> _currentOrders;

        #endregion

        #region Public Properties

        /// <summary>
        /// Validation for Empty Content in the View
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
                    if (PendingReceiveOrders == null)
                    {
                        PendingReceiveOrders = new ObservableRangeCollection<TransferOrder>();
                    }

                    PendingReceiveOrders.Clear();

                    var values = _currentOrders.Where(x => x.TransferOrderId.ToString().Contains(SearchText.ToLower())).ToList();
                    if (values != null && values.Count > 0)
                    {
                        PendingReceiveOrders.ReplaceRange(values);
                    }
                }
                else
                {
                    PendingReceiveOrders.ReplaceRange(_currentOrders);
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection of Purchase Orders
        /// </summary>
        public ObservableRangeCollection<TransferOrder> PendingReceiveOrders
        {
            get
            {
                return _pendingReceiveOrders;
            }
            set
            {
                _pendingReceiveOrders = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Purchase Order
        /// </summary>
        public TransferOrder SelectedTransferOrder
        {
            get
            {
                return _selectedTransferOrder;
            }
            set
            {
                _selectedTransferOrder = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get the Purchase Orders
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new Command(GetIntialData));
            }
        }

        /// <summary>
        /// Get the Purchase Orders
        /// </summary>
        public ICommand SelectedTransferOrderCommand
        {
            get
            {
                return _selectedTransferOrderCommand ?? (_selectedTransferOrderCommand = new Command(OnSelectedTransferOrderCommand));
            }
        }

        /// <summary>
        /// Create Transfer Order Command
        /// </summary>
        public ICommand CreateTransferOrderCommand
        {
            get
            {
                return _createTransferOrderCommand ?? (_createTransferOrderCommand = new Command(OnCreateTransferOrderCommand));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public InventoryTransferMainViewModel() : base()
        {
            _inventoryService = new InventoryManagementService();

            PageTitle = "Inventory Transfer";
        }

        /// <summary>
        /// Initialize ViewModel with Data
        /// </summary>
        /// <param name="data"></param>
        public override void InitializeViewModel(object data = null)
        {
            GetIntialData();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Initial Data from the services
        /// </summary>
        private async void GetIntialData()
        {
            IsRefreshing = true;

            try
            {
                var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();
                if (!currentSiteId.HasValue)
                {
                    IsRefreshing = false;
                    IsEmptyView = true;
                    return;
                }

                var orders = await _inventoryService.GetPendingTransferOrders(currentSiteId.Value, false);

                IsEmptyView = !orders.Any();

                if (PendingReceiveOrders == null)
                    PendingReceiveOrders = new ObservableRangeCollection<TransferOrder>();
                else
                    PendingReceiveOrders.Clear();

                foreach (var item in orders)
                {
                    if (String.IsNullOrEmpty(item.TransferOrderStatus))
                    {
                        item.TransferOrderStatus = "Pending Receive";
                    }
                    PendingReceiveOrders.Add(item);
                }

                _currentOrders = PendingReceiveOrders.ToList();
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
            }

            IsRefreshing = false;
        }

        /// <summary>
        /// Action to Navigate to the PO Receipt Page
        /// </summary>
        private async void OnSelectedTransferOrderCommand()
        {
            await _navigationService.NavigateToAsync<InventoryReceivePage>(SelectedTransferOrder);
            SelectedTransferOrder = null;
        }

        /// <summary>
        /// Create new Transfer Order
        /// </summary>
        /// <param name="obj"></param>
        private async void OnCreateTransferOrderCommand(object obj)
        {
            await _navigationService.NavigateToAsync<CreateTransferOrderPage>();
        }

        #endregion
    }
}
