using Newtonsoft.Json;

namespace HMSDigital.Notification.ViewModels
{
    public class FcmNotification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
