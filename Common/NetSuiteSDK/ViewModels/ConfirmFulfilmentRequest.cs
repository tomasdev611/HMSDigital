using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class ConfirmFulfilmentRequest
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("patientUuid")]
        public string PatientUuid { get; set; }

        [JsonProperty("netSuiteCustomerId")]
        public int NetSuiteCustomerId { get; set; }

        [JsonProperty("netSuiteTransactionId")]
        public int NetSuiteTransactionId { get; set; }

        [JsonProperty("dispatchOnly")]
        public bool DispatchOnly { get; set; }

        [JsonProperty("items")]
        public IEnumerable<FulfilmentItem> Items { get; set; }
    }
}
