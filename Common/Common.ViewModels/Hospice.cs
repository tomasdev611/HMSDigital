using System;
using System.Collections.Generic;

namespace HMSDigital.Common.ViewModels
{
    public class Hospice
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<HospiceLocation> HospiceLocations { get; set; }

        public int NetSuiteCustomerId { get; set; }

        public Address Address { get; set; }

        public PhoneNumberMinimal PhoneNumber { get; set; }

        public bool IsCreditOnHold { get; set; }

        public int? CreditHoldByUserId { get; set; }

        public string CreditHoldByUserName { get; set; }

        public DateTime? CreditHoldDateTime { get; set; }

        public string CreditHoldNote { get; set; }
    }
}
