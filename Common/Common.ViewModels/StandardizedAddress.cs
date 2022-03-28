using Newtonsoft.Json;

namespace HMSDigital.Common.ViewModels
{
    public class StandardizedAddress
    {
        public string RecordID { get; set; }
        
        public string Results { get; set; }
        
        public string FormattedAddress { get; set; }
        
        public string Organization { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public string AddressLine3 { get; set; }
        
        public string AddressLine4 { get; set; }
        
        public string AddressLine5 { get; set; }
        
        public string AddressLine6 { get; set; }
        
        public string AddressLine7 { get; set; }
        
        public string AddressLine8 { get; set; }
        
        public string SubPremises { get; set; }
        
        public string DoubleDependentLocality { get; set; }
        
        public string DependentLocality { get; set; }
        
        [JsonProperty("city")]
        public string City { get; set; }
        
        [JsonProperty("countyname")]
        public string County { get; set; }
        
        [JsonProperty("state")]
        public string State { get; set; }
        
        public string PostalCode { get; set; }
        
        public string AddressType { get; set; }
        
        public string AddressKey { get; set; }
        
        public string SubNationalArea { get; set; }
        
        [JsonProperty("countryname")]
        public string Country { get; set; }
        
        public string CountryISO3166_1_Alpha2 { get; set; }
        
        public string CountryISO3166_1_Alpha3 { get; set; }
        
        public string CountryISO3166_1_Numeric { get; set; }
        
        public string Thoroughfare { get; set; }
        
        public string ThoroughfarePreDirection { get; set; }
        
        public string ThoroughfareLeadingType { get; set; }
        
        public string ThoroughfareName { get; set; }
        
        public string ThoroughfareTrailingType { get; set; }
        
        public string ThoroughfarePostDirection { get; set; }
        
        public string DependentThoroughfare { get; set; }
        
        public string DependentThoroughfarePreDirection { get; set; }
        
        public string DependentThoroughfareLeadingType { get; set; }
        
        public string DependentThoroughfareName { get; set; }
        
        public string DependentThoroughfareTrailingType { get; set; }
        
        public string DependentThoroughfarePostDirection { get; set; }
        
        public string Building { get; set; }
        
        public string PremisesType { get; set; }
        
        public string PremisesNumber { get; set; }
        
        public string SubPremisesType { get; set; }
        
        public string SubPremisesNumber { get; set; }

        public string PostBox { get; set; }
        
        public string Latitude { get; set; }
        
        public string Longitude { get; set; }

        [JsonProperty("plus4")]
        public string Plus4Code { get; set; }
    }
}
