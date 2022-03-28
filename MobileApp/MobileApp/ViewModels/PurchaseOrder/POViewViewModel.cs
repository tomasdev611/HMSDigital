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

namespace MobileApp.ViewModels.PurchaseOrder
{
    public class POViewViewModel : BaseViewModel
    {
        #region Private Properties

        private readonly IPurchaseOrderService _purchaseOrderService;
        private bool _isEmptyView = false;
        private string _searchText;
        private ObservableRangeCollection<PurchaseOrderModel> _purchaseOrders;
        private PurchaseOrderModel _selectedPurchaseOrder;
        private ICommand _refreshCommand;
        private ICommand _selectedPurchaseOrderCommand;
        private bool _isRefreshing = true;
        private bool _isInitialized = false;

        private List<PurchaseOrderModel> _currentOrders;

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
                if(!String.IsNullOrEmpty(SearchText) && SearchText.Length > 0)
                {
                    if(PurchaseOrders == null)
                    {
                        PurchaseOrders = new ObservableRangeCollection<PurchaseOrderModel>();
                    }

                    PurchaseOrders.Clear();

                    var values = _currentOrders.Where(x => x.PurchaseOrderNumber.ToString().ToLower().Contains(SearchText.ToLower())).ToList();
                    if(values != null && values.Count > 0)
                    {
                        PurchaseOrders.ReplaceRange(values);
                    }
                }
                else
                {
                    PurchaseOrders.ReplaceRange(_currentOrders);
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection of Purchase Orders
        /// </summary>
        public ObservableRangeCollection<PurchaseOrderModel> PurchaseOrders
        {
            get
            {
                return _purchaseOrders;
            }
            set
            {
                _purchaseOrders = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected Purchase Order
        /// </summary>
        public PurchaseOrderModel SelectedPurchaseOrder
        {
            get
            {
                return _selectedPurchaseOrder;
            }
            set
            {
                _selectedPurchaseOrder = value;
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
        public ICommand SelectedPurchaseOrderCommand
        {
            get
            {
                return _selectedPurchaseOrderCommand ?? (_selectedPurchaseOrderCommand = new Command(OnSeletedPurchaseOrder));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public POViewViewModel() : base()
        {
            _purchaseOrderService = new InventoryManagementService();

            PageTitle = Strings.PurchaseOrderTitle;
        }

        /// <summary>
        /// Initialize ViewModel with Data
        /// </summary>
        /// <param name="data"></param>
        public override void InitializeViewModel(object data = null)
        {
            if(!_isInitialized)
            {
                GetIntialData();
                _isInitialized = true;
            }
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
                if(!currentSiteId.HasValue)
                {
                    IsRefreshing = false;
                    IsEmptyView = true;
                    return;
                }

                var orders = await _purchaseOrderService.GetPurchaseOrders(currentSiteId.Value);

                IsEmptyView = !orders.Any();

                if (PurchaseOrders == null)
                    PurchaseOrders = new ObservableRangeCollection<PurchaseOrderModel>();
                else
                    PurchaseOrders.Clear();

                foreach (var item in orders)
                {
                    PurchaseOrders.Add(item);
                }

                _currentOrders = PurchaseOrders.ToList();
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
        private async void OnSeletedPurchaseOrder()
        {
            await _navigationService.NavigateToAsync<POReceiptPage>(SelectedPurchaseOrder);
            SelectedPurchaseOrder = null;
        }

        #endregion
    }
}