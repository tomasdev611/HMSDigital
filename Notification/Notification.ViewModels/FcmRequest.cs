using Newtonsoft.Json;

namespace HMSDigital.Notification.ViewModels
{
    public class FcmRequest
    {
        [JsonProperty("notification")]
        public FcmNotification Notification { get; set; }
    }
}
