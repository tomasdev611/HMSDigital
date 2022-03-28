using System;
using Newtonsoft.Json;

namespace MobileApp.Models
{
    public class Token
    {
        [JsonProperty("idToken")]
        public string IdToken { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
