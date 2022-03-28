using HMSDigital.Common.ViewModels;
using Newtonsoft.Json;
using System;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class AddressRequest : AddressMinimal
    {
        public string Phone { get; set; }  // this isn't the correct place to have phone

        [JsonProperty("addressUUID")]
        public Guid AddressUuid { get; set; }

        [JsonProperty("zip")]
        public override int ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}
