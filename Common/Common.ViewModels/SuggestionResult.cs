using Newtonsoft.Json;
using System.Collections.Generic;

namespace HMSDigital.Common.ViewModels
{
    public class SuggestionResult
    {
        [JsonProperty("Address1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("Locality")]
        public string City { get; set; }
        public string CityAccepted { get; set; }
        public string CityNotAccepted { get; set; }

        [JsonProperty("AdministrativeArea")]
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountrySubdivisionCode { get; set; }
        public string AddressKey { get; set; }
        public string SuiteName { get; set; }
        public int SuiteCount { get; set; }
        public List<string> SuiteList { get; set; }
        public List<string> PlusFour { get; set; }
        public string MAK { get; set; }
        public string BaseMAK { get; set; }
        public string CountryName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
