using Newtonsoft.Json;
using System.Collections.Generic;

namespace HMSDigital.Common.ViewModels
{
    public class SmartyStreetsSuggestionList
    {
        [JsonProperty("suggestions")]
        public List<SmartyStreetsSuggestion> Suggestions { get; set; }
    }
}
