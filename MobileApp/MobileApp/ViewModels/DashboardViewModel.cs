using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Assets.Enums;
using MobileApp.DataBaseAttributes.Repositories;
using MobileApp.Exceptions;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Pages.CommonPages;
using MobileApp.Pages.PopUpMenu;
using MobileApp.Pages.Screen.CommonPages;
using MobileApp.Service;
using MobileApp.Utils;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly OrdersService _ordersService;

        private readonly DispatchService _dispatchService;

        private readonly DriverService _driverService;

        private readonly CurrentLocation _currentLocation;

        private readonly PendingOrderRepository _pendingOrderRepository;

        private readonly Dialer _dialer;


        public DashboardViewModel() : base()
        {
            _ordersService = new OrdersService();
            _dispatchService = new DispatchService();
            _currentLocation = new CurrentLocation();
            _driverService = new DriverService();
            _pendingOrderRepository = new PendingOrderRepository();
            _dialer = new Dialer();
        }

        public override void InitializeViewModel(object data = null)
        {
            GetIntialData();
        }

        private bool _isEmptyView = false;

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

        private bool _multipleLoadAllowed = false;

        public bool MultipleLoadAllowed
        {
            get
            {
                return _multipleLoadAllowed;
            }
            set
            {
                _multipleLoadAllowed = value;
                OnPropertyChanged();
            }
        }

        private bool _mobileFulfillAllowed = false;

        public bool MobileFulfillAllowed
        {
            get
            {
                return _mobileFulfillAllowed;
            }
            set
            {
                _mobileFulfillAllowed = value;
                OnPropertyChanged();
            }
        }

        private bool _shouldShowFlashMessage;

        public bool ShouldShowFlashMessage
        {
            get
            {
                return _shouldShowFlashMessage;
            }
            set
            {
                _shouldShowFlashMessage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LoadList> _loadList;

        public ObservableCollection<LoadList> LoadList
        {
            get
            {
                return _loadList;
            }
            set
            {
                _loadList = value;
                OnPropertyChanged();
            }
        }

        private LoadListCounts _loadListCounts;

        public LoadListCounts LoadListCounts
        {
            get
            {
                return _loadListCounts;
            }
            set
            {
                _loadListCounts = value;
                OnPropertyChanged();
            }
        }

        private string _siteAddress;

        public string SiteAddress
        {
            get
            {
                return _siteAddress;
            }
            set
            {
                _siteAddress = value;
                OnPropertyChanged();
            }
        }

        private LayoutState _currentState = LayoutState.Loading;

        public LayoutState CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                OnPropertyChanged();
            }
        }

        private SiteDetail _siteDetails;

        public SiteDetail SiteDetails
        {
            get
            {
                return _siteDetails;
            }
            set
            {
                _siteDetails = value;
                OnPropertyChanged();
            }
        }

        private OrderData _currentOrder;

        public OrderData CurrentOrder
        {
            get
            {
                return _currentOrder;
            }
            set
            {
                _currentOrder = value;
                OnPropertyChanged();
            }
        }

        private SiteLoadList _siteLoadList;

        public SiteLoadList SiteLoadList
        {
            get
            {
                return _siteLoadList;
            }
            set
            {
                _siteLoadList = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<OrderData> _currentOrdersList;

        public IEnumerable<OrderData> CurrentOrdersList
        {
            get
            {
                return _currentOrdersList;
            }
            set
            {
                _currentOrdersList = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing = true;

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

        private bool _isDriverOnSite;

        public bool IsDriverOnSite
        {
            get
            {
                return _isDriverOnSite;
            }
            set
            {
                _isDriverOnSite = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoadlist;

        public bool IsLoadList
        {
            get
            {
                return _isLoadlist;
            }
            set
            {
                _isLoadlist = value;
                OnPropertyChanged();
            }
        }

        private int _ordersCount;

        public int OrdersCount
        {
            get
            {
                return _ordersCount;
            }
            set
            {
                _ordersCount = value;
                OnPropertyChanged();
            }
        }

        private int _loadedItemsCount = 0;

        public int LoadeditemsCount
        {
            get
            {
                return _loadedItemsCount;
            }
            set
            {
                _loadedItemsCount = value;
                OnPropertyChanged();
            }
        }

        private OrderProgress _orderProgress;

        public OrderProgress OrderProgress
        {
            get
            {
                return _orderProgress;
            }
            set
            {
                _orderProgress = value;
                OnPropertyChanged();
            }
        }

        private Driver _driverDetails;

        public Driver DriverDetails
        {
            get
            {
                return _driverDetails;
            }
            set
            {
                _driverDetails = value;
                OnPropertyChanged();
            }
        }

        private string _vehicleCvn;

        public string VehicleCvn
        {
            get
            {
                return _vehicleCvn;
            }
            set
            {
                _vehicleCvn = value;
                OnPropertyChanged();
            }
        }

        private ICommand _showLoadListDetailsCommand;

        public ICommand ShowLoadListDetailsCommand
        {
            get
            {
                return _showLoadListDetailsCommand ?? (_showLoadListDetailsCommand = new Command(ShowLoadListDetails));
            }
        }

        public async void ShowLoadListDetails()
        {
            if (SiteLoadList != null && SiteLoadList.Loadlists != null)
            {
                var items = LoadListUtils.GetLoadList(SiteLoadList.Loadlists);
                if(items != null)
                {
                    await _navigationService.NavigateToAsync<ItemsListView>(items);
                }
            }
        }

        private ICommand _beginLoadingCommand;

        public ICommand BeginLoadingCommand
        {
            get
            {
                return _beginLoadingCommand ?? (_beginLoadingCommand = new Command(BeginLoading));
            }
        }

        private async void BeginLoading()
        {
            if (SiteLoadList != null && SiteLoadList.Loadlists != null)
            {
                await UpdateCurrentOrderStatus(SiteLoadList.Loadlists.FirstOrDefault().Orders.Select(o => o.Id).ToList(), null, (int)OrderHeaderStatusTypes.Loading_Truck);
                await _navigationService.NavigateToAsync<LoadListScan>(SiteLoadList.Loadlists.FirstOrDefault());
            }
        }

        private ICommand _navigateToMapsCommand;

        public ICommand NavigateToMapsCommand
        {
            get
            {
                return _navigateToMapsCommand ?? (_navigateToMapsCommand = new Command(NavigateToMapsAsync));
            }
        }

        private void NavigateToMapsAsync()
        {
            var location = new Location(SiteDetails.Latitude, SiteDetails.Longitude);
            LaunchMap(location);
        }

        private ICommand _startOrderNavigationCommand;

        public ICommand StartOrderNavigationCommand
        {
            get
            {
                return _startOrderNavigationCommand ?? (_startOrderNavigationCommand = new Command(StartOrderNavigation));
            }
        }

        public ICommand CallContactCommand
        {
            get
            {
                return new Command<object>((x) =>
                {
                    CallContact(x);
                });
            }
        }

        private async void CallContact(object number)
        {
            try
            {
                if (number == null)
                    return;

                var phone = number.ToString();

                if (!String.IsNullOrEmpty(phone))
                {
                    _dialer.OpenDialerWithNumber(phone);
                    await _navigationService.PushPopupAsync<CallReport>(phone);
                }
                else if (CurrentOrder?.ContactNumber > 0)
                {
                    _dialer.OpenDialerWithNumber(CurrentOrder?.ContactNumber.ToString());
                    await _navigationService.PushPopupAsync<CallReport>(CurrentOrder?.ContactNumber.ToString());
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                _toastMessageService.ShowToast("Phone permssions required to make a call");
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Not able to open dailer at this moment");
            }
        }

        private void StartOrderNavigation()
        {
            var location = new Location((double)CurrentOrder.ShippingAddress.Latitude, (double)CurrentOrder.ShippingAddress.Longitude);
            LaunchMap(location);
        }

        private ICommand _confirmArrivalCommand;

        public ICommand ConfirmArrivalCommand
        {
            get
            {
                return _confirmArrivalCommand ?? (_confirmArrivalCommand = new Command(ConfirmArrival));
            }
        }

        private async void ConfirmArrival()
        {
            await UpdateCurrentOrderStatus(new List<int> { CurrentOrder.OrderID }, (int)OrderHeaderStatusTypes.OnSite, (int)OrderHeaderStatusTypes.OnSite);

            CurrentOrder.StatusId = (int)OrderHeaderStatusTypes.OnSite;
            IsDriverOnSite = true;
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
            await _navigationService.PushPopupAsync<DetailedOrderNotes>(CurrentOrder.OrderNotes);
        }

        private ICommand _checkOrdersCommand;

        public ICommand CheckOrdersCommand
        {
            get
            {
                return _checkOrdersCommand ?? (_checkOrdersCommand = new Command(CheckOrders));
            }
        }

        private async void CheckOrders()
        {
            var ordersList = new ListGroup<OrderData>("Orders", CurrentOrdersList.ToList(), CurrentOrdersList.Count());
            await _navigationService.NavigateToAsync<OrdersViewScreen>(ordersList);
        }

        private ICommand _checkRouteCommand;

        public ICommand CheckRouteCommand
        {
            get
            {
                return _checkRouteCommand ?? (_checkRouteCommand = new Command(CheckRoute));
            }
        }

        private async void CheckRoute()
        {
            await _navigationService.PushPopupAsync<RouteLookup>(SiteDetails);
        }

        private ICommand _beginDispatchCommand;

        public ICommand BeginDispatchCommand
        {
            get
            {
                return _beginDispatchCommand ?? (_beginDispatchCommand = new Command(BeginDispatch));
            }
        }

        private async void BeginDispatch()
        {
            try
            {
                var location = await _currentLocation.GetCurrentLocationAsync();

                CurrentOrder.FulfillmentStartDateTime = DateTime.UtcNow;
                CurrentOrder.FulfillmentStartAtLatitude = (decimal)location.Latitude;
                CurrentOrder.FulfillmentStartAtLongitude = (decimal)location.Longitude;

                await _navigationService.NavigateToAsync<OrderListScan>(CurrentOrder);
            }
            catch (PermissionRequiredException prex)
            {
                _toastMessageService.ShowToast(prex.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Unable to proceed, Please check yout internet connection");
            }
        }

        private ICommand _navigateToOrdersListCommand;

        public ICommand NavigateToOrdersListCommand
        {
            get
            {
                return _navigateToOrdersListCommand ?? (_navigateToOrdersListCommand = new Command(NavigateToOrdersList));
            }
        }

        public async void NavigateToOrdersList(object type)
        {
            var ordersType = (string)type;
            IEnumerable<OrderData> ordersList;
            switch (ordersType)
            {
                case "Delivery":
                    ordersList = await GetDeliveryOrders();
                    break;
                case "Pickup":
                    ordersList = await GetPickupOrdes();
                    break;
                case "Completed":
                    ordersList = CurrentOrdersList.Where(co => co.IsCompleted || co.IsExceptionFulfillment);
                    break;
                default:
                    ordersList = CurrentOrdersList;
                    break;
            }
            if (ordersList.Count() > 0)
            {
                var ordersListGroup = new ListGroup<OrderData>(ordersType, ordersList.ToList(), ordersList.Count());
                await _navigationService.NavigateToAsync<OrdersViewScreen>(ordersListGroup);
            }
        }

        private ICommand _loadTruckCommand;

        public ICommand LoadTruckCommand
        {
            get
            {
                return _loadTruckCommand ?? (_loadTruckCommand = new Command(LoadTruck));
            }
        }

        private async void LoadTruck(object listItemSelectedArgs)
        {
            try
            {
                var menuItem = ((listItemSelectedArgs as ItemTappedEventArgs)?.Item as LoadList);
                var loadListItem = new VehicleLoadlist
                {
                    Items = menuItem.Items,
                    Orders = menuItem.Orders,
                    TotalInventoryCount = menuItem.TotalInventoryCount,
                    TotalItemCount = menuItem.TotalItemCount,
                    Vehicle = menuItem.Vehicle,
                    VehicleId = menuItem.VehicleId
                };

                await _dispatchService.UpdateOrderStatus(new OrderStatusUpdateRequest()
                {
                    OrderIds = loadListItem.Orders.Select(o => o.Id),
                    StatusId = null,
                    DispatchStatusId = (int)OrderHeaderStatusTypes.Loading_Truck
                });

                await _navigationService.NavigateToAsync<LoadListScan>(loadListItem);
            }
            catch (BadRequestException ex)
            {
                _toastMessageService.ShowToast(ex.Message);

            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error while updating order status");
            }
        }

        private ICommand _getDataCommand;

        public ICommand GetDataCommand
        {
            get
            {
                return _getDataCommand ?? (_getDataCommand = new Command(GetIntialData));
            }
        }

        private async void GetIntialData()
        {
            UpdateCurrentState(LayoutState.Loading);

            if (await RolePermissionUtils.CheckPermission(PermissionName.Vehicle, PermissionAccess.MULTIPLE_LOAD))
            {
                MultipleLoadAllowed = true;
                GetMultipleVehiclesLoadList();
            }

            if (await RolePermissionUtils.CheckPermission(PermissionName.Orders, PermissionAccess.MOBILE_FULFILL))
            {
                MobileFulfillAllowed = true;
                GetDataForOrderFullfill();
            }

            if (!MultipleLoadAllowed && !MobileFulfillAllowed)
            {
                UpdateCurrentState(LayoutState.Empty);
                IsRefreshing = false;
            }
        }

        private async void GetMultipleVehiclesLoadList()
        {
            try
            {
                SiteLoadList = await _dispatchService.GetSiteLoadListAsync();
                if (SiteLoadList != null)
                {
                    LoadListCounts = LoadListUtils.GetSiteLoadListCounts(SiteLoadList);

                    if (SiteLoadList.Loadlists != null)
                    {
                        LoadList = new ObservableCollection<LoadList>(SiteLoadList.Loadlists.Select(item =>
                        {
                            var loadlist = new LoadList
                            {
                                DriverName = string.IsNullOrEmpty(item.Vehicle.CurrentDriverName) ? "Driver not assigned yet" : $"For {item.Vehicle.CurrentDriverName}",
                                PickupTime = "",
                                Items = item.Items,
                                Orders = item.Orders,
                                TotalInventoryCount = item.TotalInventoryCount,
                                TotalItemCount = item.TotalItemCount,
                                Vehicle = item.Vehicle,
                                VehicleId = item.VehicleId,
                            };
                            return loadlist;
                        }));
                    }

                }
            }
            catch (BadRequestException ex)
            {
                _toastMessageService.ShowToast(ex.Message);

            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error while updating order status");
            }
            finally
            {
                IsRefreshing = false;
                UpdateCurrentState(LayoutState.None);
            }
        }

        private async void GetDataForOrderFullfill()
        {
            try
            {
                if (ShouldShowFlashMessage)
                {
                    DriverDetails = await _driverService.GetDriverUserDetailsAsync();
                    if (DriverDetails.CurrentVehicleId == null)
                    {
                        UpdateCurrentState(LayoutState.Empty);
                        return;
                    }
                    VehicleCvn = DriverDetails.CurrentVehicle.Cvn;
                }
                var getSiteLoadListTask = _dispatchService.GetSiteLoadListAsync();
                var getSiteDetailsTask = SiteDetailsUtil.GetCurrentSiteDetailsAsync();
                var getOrderListTask = _ordersService.GetOrderListAsync();

                await Task.WhenAll(getSiteLoadListTask, getSiteDetailsTask, getOrderListTask);

                SiteLoadList = getSiteLoadListTask.Result;
                SiteDetails = getSiteDetailsTask.Result;
                CurrentOrdersList = getOrderListTask.Result;

                var pendingOrders = await _pendingOrderRepository.GetAllAsync();

                if (SiteLoadList != null
                && SiteLoadList.Loadlists != null
                && (pendingOrders == null
                || pendingOrders.Count == 0))
                {
                    if (SiteDetails != null && CurrentOrdersList.Count() != 0)
                    {
                        PopulateLoadlistData();
                        UpdateCurrentState(LayoutState.None);
                    }
                    else if (CurrentOrdersList.Count() == 0)
                    {
                        CurrentOrder = null;
                        UpdateCurrentState(LayoutState.None);
                    }
                }
                else if (CurrentOrdersList.Count() != 0)
                {
                    if (CurrentOrdersList.All(ol => ol.IsCompleted))
                    {
                        CurrentOrder = null;
                        UpdateCurrentState(LayoutState.None);
                    }

                    PopulateOrderData();
                    UpdateCurrentState(LayoutState.None);
                }
                else
                {
                    UpdateCurrentState(LayoutState.Empty);
                }
            }
            catch (BadRequestException bex)
            {
                _toastMessageService.ShowToast(bex.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                IsRefreshing = false;
                return;
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void UpdateCurrentState(LayoutState layoutState)
        {
            CurrentState = layoutState;
            OnPropertyChanged(nameof(IsEmptyView));
            if (CurrentState == LayoutState.Empty)
            {
                IsEmptyView = true;
                CurrentState = LayoutState.None;
            }
            else
            {
                IsEmptyView = false;
            }
        }

        private async void PopulateOrderData()
        {
            IsLoadList = false;

            if (SiteLoadList != null && SiteLoadList.Loadlists != null)
            {
                var currentVehicleLoadlist = SiteLoadList.Loadlists.FirstOrDefault();
                var orderToLoad = currentVehicleLoadlist.Orders.Select(o => o.Id);
                IsLoadList = CurrentOrdersList.Any(x => orderToLoad.Contains(x.OrderID));
                PopulateLoadlistData();

                return;
            }

            var incompleteOrderList = CurrentOrdersList.Where(o => !o.IsCompleted && !o.IsExceptionFulfillment);
            if (incompleteOrderList.Any())
            {
                var pendingOrdersList = await _pendingOrderRepository.UpdatePendingOrders(incompleteOrderList);

                pendingOrdersList = pendingOrdersList.Where(o => !o.IsProcessed);
                if (!pendingOrdersList.Any())
                {
                    if (SiteLoadList != null && SiteLoadList.Loadlists != null)
                    {
                        PopulateLoadlistData();
                    }
                    await _pendingOrderRepository.DeleteAllAsync();
                    await _navigationService.RemoveLastFromBackStackAsync();
                    return;
                }

                CurrentOrder = CurrentOrdersList.FirstOrDefault(ol =>
                !ol.IsCompleted && !ol.IsExceptionFulfillment
                && pendingOrdersList.Any(o =>
                    o.OrderId == ol.OrderID
                    && o.OrderTypeId == ol.OrderTypeId
                    )
               );
            }
            else
            {
                CurrentOrder = null;
            }

            InitOrderProgressData();
            await UpdateOrderStatuses();
        }

        private async Task UpdateOrderStatuses()
        {
            if (CurrentOrder == null)
            {
                return;
            }

            IsDriverOnSite = false;
            var orderIds = CurrentOrdersList?
                .Where(co => co.StatusId == (int)OrderHeaderStatusTypes.Scheduled || co.StatusId == (int)OrderHeaderStatusTypes.OnTruck)
                .Select(co => co.OrderID);

            // Update Orders status from Scheduled or OnTruck to OutForFulfillment
            if (orderIds.Any())
            {
                await UpdateCurrentOrderStatus(orderIds, (int)OrderHeaderStatusTypes.OutForFulfillment, (int)OrderHeaderStatusTypes.OutForFulfillment);
            }

            if (orderIds.Contains(CurrentOrder.OrderID))
            {
                CurrentOrder.StatusId = (int)OrderHeaderStatusTypes.OutForFulfillment;
            }

            if (CurrentOrder.StatusId == (int)OrderHeaderStatusTypes.Partial_Fulfillment
            || CurrentOrder.StatusId == (int)OrderHeaderStatusTypes.OutForFulfillment)
            {
                var orderUpdateSuccess = await UpdateCurrentOrderStatus(new List<int> { CurrentOrder.OrderID }, (int)OrderHeaderStatusTypes.Enroute, (int)OrderHeaderStatusTypes.Enroute);

                if (orderUpdateSuccess)
                {
                    CurrentOrder.StatusId = (int)OrderHeaderStatusTypes.Enroute;
                }
                UpdateDriverLocation();
            }
            if (CurrentOrder.StatusId == (int)OrderHeaderStatusTypes.OnSite)
            {
                IsDriverOnSite = true;
            }
        }

        private async void InitOrderProgressData()
        {
            OrderProgress = new OrderProgress();

            var deliveryOrders = await GetDeliveryOrders();
            OrderProgress.DeliveryOrdersCount = deliveryOrders.Count();
            OrderProgress.DeliveryItemsCount = deliveryOrders
                .SelectMany(dor => dor.OrderLineItems)
                .Where(oli => oli.Action == "Delivery")
                .Sum(dli => dli.ItemCount);

            var pickupOrders = await GetPickupOrdes();
            OrderProgress.PickupOrdersCount = pickupOrders.Count();
            OrderProgress.PickupItemsCount = pickupOrders
                .SelectMany(p => p.OrderLineItems)
                .Where(pli => pli.Action == "Pickup")
                .Sum(pli => pli.ItemCount);

            var completedOrders = CurrentOrdersList.Where(co => co.IsCompleted || co.IsExceptionFulfillment);
            OrderProgress.CompletedOrdersCount = completedOrders.Count();
            OrderProgress.CompletedItemsCount = completedOrders.Sum(c => c.ItemCount);
        }

        private async Task<IEnumerable<OrderData>> GetDeliveryOrders()
        {
            var pendingOrdersList = await _pendingOrderRepository?.GetManyAsync(po => !po.IsProcessed);
            var pendingOrderIds = pendingOrdersList?.Select(o => o.OrderId);
            var orderTypesWithDelivery = OrderDataUtils.GetOrderTypesWithDelivery();
            if (orderTypesWithDelivery == null)
            {
                return new List<OrderData>();
            }

            return CurrentOrdersList.Where(co => orderTypesWithDelivery.Contains(co.OrderTypeId)
                                                    && !co.IsCompleted
                                                    && pendingOrderIds.Contains(co.OrderID));
        }

        private async Task<IEnumerable<OrderData>> GetPickupOrdes()
        {
            var pendingOrdersList = await _pendingOrderRepository?.GetManyAsync(po => !po.IsProcessed);
            var pendingOrderIds = pendingOrdersList.Select(o => o.OrderId);
            var orderTypesWithPickup = OrderDataUtils.GetOrderTypesWithPickup();
            return CurrentOrdersList.Where(co => orderTypesWithPickup.Contains(co.OrderTypeId)
                                                    && !co.IsCompleted
                                                    && pendingOrderIds.Contains(co.OrderID));
        }

        private void PopulateLoadlistData()
        {
            CurrentOrdersList = CurrentOrdersList.Where(co => !co.IsCompleted && !co.IsExceptionFulfillment);
            OrdersCount = CurrentOrdersList.Count();
            SiteDetails.ItemsCount = OrdersCount;
            IsLoadList = true;
        }

        private async void LaunchMap(Location location)
        {
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            await Map.OpenAsync(location, options);
        }

        private async void UpdateDriverLocation()
        {
            try
            {
                await _driverService.SendDriverLocation();
            }
            catch (BadRequestException bre)
            {
                _toastMessageService.ShowToast(bre.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error while updating current location");
            }
        }

        private async Task<bool> UpdateCurrentOrderStatus(IEnumerable<int> orderIds, int? orderStatusId, int? dispatchStatusId)
        {
            try
            {
                var orderStatusUpdateReq = new OrderStatusUpdateRequest()
                {
                    OrderIds = orderIds,
                    StatusId = orderStatusId,
                    DispatchStatusId = dispatchStatusId
                };
                return await _dispatchService.UpdateOrderStatus(orderStatusUpdateReq);
            }
            catch (BadRequestException bre)
            {
                _toastMessageService.ShowToast(bre.Message);
                return false;
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error while updating order status");
                return false;
            }
        }
    }
}