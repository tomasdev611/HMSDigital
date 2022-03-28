using System;
using System.Linq;
using System.Threading.Tasks;
using MobileApp.Assets.Constants;
using MobileApp.Interface;
using MobileApp.Models;
using MobileApp.Pages.Screen.CommonPages;
using MobileApp.Utils;
using Plugin.PushNotification;
using Refit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.Service
{
    public class NotificationService
    {
        private readonly StorageService _storageService;

        private readonly AuthService _authService;

        private readonly INotificationService _notificationService;

        private IDeviceTokenService _deviceTokenService;

        public NotificationService()
        {
            _notificationService = RestService.For<INotificationService>(HMSHttpClientFactory.GetNotificationHttpClient());
            _deviceTokenService = DependencyService.Get<IDeviceTokenService>();

            _storageService = new StorageService();
            _authService = new AuthService();
        }

        public async Task RegisterDeviceAsync()
        {
            try
            {
                var deviceToken = _deviceTokenService.GetDeviceToken();

                if (!string.IsNullOrEmpty(deviceToken))
                {
                    var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();

                    var deviceRegistrationRequest = new DeviceRegisterRequest()
                    {
                        DeviceId = deviceToken,
                        Platform = DeviceInfo.Platform.ToString(),
                        CurrentSiteId = currentSiteId
                    };

                    var registrationResponse = await _notificationService.RegisterNotificationServiceAsync(deviceRegistrationRequest);
                    if (registrationResponse.IsSuccessStatusCode)
                    {
                        _storageService.AddToStorage(StorageConstants.DEVICE_TOKEN, deviceToken, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeregisterDeviceAsync()
        {
            try
            {
                var deviceId = _deviceTokenService.GetDeviceToken();

                if (!string.IsNullOrEmpty(deviceId))
                {
                    await _notificationService.DeRegisterNotificationServiceAsync(deviceId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static async void NotificationClickAction(object s, PushNotificationResponseEventArgs arguments)
        {
            if (!arguments.Data.Any())
            {
                return;
            }
            try
            {
                var navigationService = App.NavigationService;

                var currentOrdersList = await new OrdersService().GetOrderListAsync();
                var ordersList = new ListGroup<OrderData>("Orders", currentOrdersList.ToList(), currentOrdersList.Count());

                await navigationService.NavigateToAsync<OrdersViewScreen>(ordersList);
            }
            catch
            {
                throw;
            }
        }

        public async void RefershDeviceRegistration(string currentDeviceToken)
        {
            if (!IsDeviceTokenChanged(currentDeviceToken))
            {
                return;
            }

            var isAuthenthicated = await _authService.IsAuthenticated();
            if (isAuthenthicated)
            {
                try
                {
                    await RegisterDeviceAsync();
                }
                catch { }
            }
        }

        private bool IsDeviceTokenChanged(string currentDeviceToken)
        {
            var deviceTokenFromStorage = _storageService.GetFromStorage(StorageConstants.DEVICE_TOKEN, true);

            return !string.Equals(deviceTokenFromStorage, currentDeviceToken);
        }
    }
}
