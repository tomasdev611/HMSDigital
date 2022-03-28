using System;
using MobileApp.Assets.Constants;
using MobileApp.Interface;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileApp.Methods
{
    public class Dialer
    {
        private readonly PermissionHandler _permissionHandler;

        private readonly IToastMessage _toastMessageService;

        private readonly ISettingsHelper _settingsHelper;

        public Dialer()
        {
            _permissionHandler = new PermissionHandler();
            _toastMessageService = DependencyService.Get<IToastMessage>();
            _settingsHelper = DependencyService.Get<ISettingsHelper>();
        }
        public async void OpenDialerWithNumber(string phoneNumber)
        {
            var hasPhonePermission = await _permissionHandler.RequestPermissionAsync(PermissionConstants.PHONE_PERMISSION);
            if (!hasPhonePermission)
            {
                _toastMessageService.ShowToast("Phone permssions required to make a call");
                _settingsHelper.OpenAppSettings();

                return;
            }

            try
            {
                PhoneDialer.Open(phoneNumber);
            }
            catch (ArgumentNullException)
            {
                _toastMessageService.ShowToast("Phone number should not be empty");
            }
            catch (FeatureNotSupportedException)
            {
                _toastMessageService.ShowToast("Phone permssions required to make a call");
            }
            catch(Exception ex)
            {
                _toastMessageService.ShowToast("Not able to open dailer at this moment");
            }
        }
    }
}
