using RestEase;
using System.Threading.Tasks;

namespace NotificationSDK.Interfaces
{
    public interface INotificationAPI
    {
        [Header("Authorization", "Bearer")]
        string BearerToken { get; set; }

        [Post("api/notification")]
        Task PostNotification([Body] NotificationPostRequest notificationPostRequest);
    }
}
