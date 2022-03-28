using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class WarehouseRequest
    {
        public int NetSuiteLocationId { get; set; }

        public int? ParentNetSuiteLocationId { get; set; }

        public string Name { get; set; }

        public string LocationType { get; set; }

        public int? SiteCode { get; set; }

        public string Vin { get; set; }

        public string Cvn { get; set; }

        public string LicensePlate { get; set; }

        public decimal Capacity { get; set; }

        public decimal Length { get; set; }

        public AddressRequest Address { get; set; }

        public IEnumerable<PhoneNumberReqeust> PhoneNumbers { get; set; }

        public string CreatedByUserEmail { get; set; }

        public string UpdatedByUserEmail { get; set; }
    }
}
