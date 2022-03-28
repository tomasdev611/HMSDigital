using MobileApp.Interface;
using MobileApp.iOS.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceTokenService))]
namespace MobileApp.iOS.Service
{
    public class DeviceTokenService : IDeviceTokenService
    {
        public static string DeviceToken { get; set; }
        public string GetDeviceToken()
        {
            return DeviceToken;
        }
    }
}