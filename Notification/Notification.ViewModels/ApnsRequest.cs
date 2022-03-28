using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Notification.ViewModels
{
    public class ApnsRequest
    {
        [JsonProperty("aps")]
        public Aps Aps { get; set; }
    }
}
