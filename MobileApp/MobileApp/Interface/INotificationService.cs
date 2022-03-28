using System.Threading.Tasks;
using MobileApp.Models;
using Refit;

namespace MobileApp.Interface
{
    public interface INotificationService
    {
        [Post("/api/push-notification/device-register")]
        Task<ApiResponse<string>> RegisterNotificationServiceAsync([Body] DeviceRegisterRequest deviceRegisterRequest);

        [Delete("/api/push-notification/device-register/{deviceId}")]
        Task<ApiResponse<string>> DeRegisterNotificationServiceAsync(string deviceId);
    }
}
