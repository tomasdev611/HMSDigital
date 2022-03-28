using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class HospiceContactRequest
    {
        [JsonProperty("internalId")]
        public int NetSuiteCustomerContactId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        [JsonProperty("jobTitle")]
        public string Designation { get; set; }

        public bool IsAdmin { get; set; }

        public string UpdatedByUserEmail { get; set; }
    }
}
