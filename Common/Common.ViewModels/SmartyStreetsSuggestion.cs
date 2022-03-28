using Newtonsoft.Json;

namespace HMSDigital.Common.ViewModels
{
    public class SmartyStreetsSuggestion
    {
        [JsonProperty("street_line")]
        public string StreetLine { get; set; }
        [JsonProperty("secondary")]
        public string Secondary { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }
        [JsonProperty("entries")]
        public int Entries { get; set; }
    }
}
