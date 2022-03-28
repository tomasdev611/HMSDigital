using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class ApproverRequest
    {
        [JsonProperty("internalCustomerId")]
        public int NetSuiteCustomerId { get; set; }

        public IEnumerable<int> Approvers { get; set; } //Array of integer represent here NetSuiteContactId

    }
}
