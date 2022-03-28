using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Exceptions;
using MobileApp.Interface;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Utils;
using Refit;
using Xamarin.Forms;

namespace MobileApp.Service
{
    public class DispatchService
    {
        private readonly IDispatchApi _dispatchApi;

        private readonly CurrentLocation _currentLocation;

        private readonly IToastMessage _toastMessageService;

        public DispatchService()
        {
            _currentLocation = new CurrentLocation();
            _toastMessageService = DependencyService.Get<IToastMessage>();
            _dispatchApi = RestService.For<IDispatchApi>(HMSHttpClientFactory.GetCoreHttpClient());
        }

        public async Task<SiteLoadList> GetSiteLoadListAsync()
        {
            try
            {
                var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();
                var currentvehicleId = await UserDetailsUtils.GetUsersCurrentVehicleId();

                var loadlist = await _dispatchApi.GetSiteLoadListAsync(currentSiteId ?? 0, "vehicleId==" + currentvehicleId);
                if (!loadlist.IsSuccessStatusCode)
                {
                    return null;
                }
                return loadlist.Content;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SendPickupRequestAsync(IEnumerable<DispatchItems> dispatchItemList, int requestId, string requestType, int vehicleId)
        {
            try
            {
                var location = await _currentLocation.GetCurrentLocationAsync();

                if (location == null)
                {
                    _toastMessageService.ShowToast("Not able to fetch the current location. Please check location settings");
                    return false;
                }

                var pickupResponse = await _dispatchApi.SendPickupRequestAsync(new DispatchMovementRequest
                {
                    RequestId = requestId,
                    RequestType = requestType,
                    AllowPartialDispatch = true,
                    Latitude = (decimal)location.Latitude,
                    Longitude = (decimal)location.Longitude,
                    DispatchItems = dispatchItemList,
                    VehicleId = vehicleId
                });
                return pickupResponse.IsSuccessStatusCode;
            }
            catch (PermissionRequiredException prex)
            {
                throw prex;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SendFullfillOrderRequestAsync(IEnumerable<FulfillmentItems> fulfillmentItems, OrderData orderData, string partialFulfillmentReason, bool isExceptionFulfillment = false)
        {
            try
            {
                var location = await _currentLocation.GetCurrentLocationAsync();

                if (location == null)
                {
                    _toastMessageService.ShowToast("Not able to fetch the current location. Please check location settings");
                    return false;
                }

                var dropResponse = await _dispatchApi.SendFullfillOrderRequestAsync(new OrderFulfillmentRequest
                {
                    FulfillmentItems = fulfillmentItems,
                    OrderId = orderData.OrderID,
                    FulfillmentStartDateTime = orderData.FulfillmentStartDateTime,
                    FulfillmentStartAtLongitude = orderData.FulfillmentStartAtLongitude,
                    FulfillmentStartAtLatitude = orderData.FulfillmentStartAtLatitude,
                    FulfillmentEndDateTime = DateTime.UtcNow,
                    FulfillmentEndAtLatitude = (decimal)location.Latitude,
                    FulfillmentEndAtLongitude = (decimal)location.Longitude,
                    PartialFulfillmentReason = partialFulfillmentReason,
                    IsExceptionFulfillment = isExceptionFulfillment
                });
                return dropResponse.IsSuccessStatusCode;
            }
            catch (PermissionRequiredException prex)
            {
                throw prex;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderFulfillmentLineItem>> GetFullfilledOrderItem(int orderId)
        {
            try
            {
                var response = await _dispatchApi.GetFullfillOrderItemsAsync(orderId);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content.Records;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatus(OrderStatusUpdateRequest orderStatusUpdateRequest)
        {
            try
            {
                var response = await _dispatchApi.UpdateOrderStatus(orderStatusUpdateRequest);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }
    }
}