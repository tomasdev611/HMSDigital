using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Notification.ViewModels
{
    public class Aps
    {
        [JsonProperty("alert")]
        public string Alert { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }
    }
}
