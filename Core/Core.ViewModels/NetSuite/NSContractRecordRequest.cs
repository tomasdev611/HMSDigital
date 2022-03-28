using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSContractRecordRequest
    {
        public int? NetSuiteCustomerId { get; set; }
        
        public int NetSuiteContractRecordId { get; set; }
        
        public int? NetSuiteSubscriptionId { get; set; }
        
        public double? Rate { get; set; }
        
        public DateTime? EffectiveStartDate { get; set; }
        
        public DateTime? EffectiveEndDate { get; set; }
        
        public bool RiskCapEligible { get; set; }
        
        public bool ShowOnOrderScreen { get; set; }
        
        public int? NetSuiteBillingItemId { get; set; }
        
        public int? NetSuiteRelatedItemId { get; set; }
        
        public string CreatedByUserEmail { get; set; }
    }
}
