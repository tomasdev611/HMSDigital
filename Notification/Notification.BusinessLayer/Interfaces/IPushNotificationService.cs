using HMSDigital.Notification.ViewModels;
using System.Threading.Tasks;

namespace HMSDigital.Notification.BusinessLayer.Interfaces
{
    public interface IPushNotificationService
    {
        Task DeviceRegistration(DeviceRegister deviceRegisterRequest);

        Task<DeviceRegister> GetDeviceRegistrationByDeviceId(string deviceId);

        Task DeleteDeviceRegistration(string deviceId);
    }
}
