using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class ContractRecords
    {
        public int Id { get; set; }
        public int NetSuiteContractRecordId { get; set; }
        public int? NetSuiteCustomerId { get; set; }
        public int? NetSuiteSubscriptionId { get; set; }
        public int? NetSuiteBillingItemId { get; set; }
        public int? NetSuiteRelatedItemId { get; set; }
        public int? HospiceId { get; set; }
        public int? HospiceLocationId { get; set; }
        public int? ItemId { get; set; }
        public double Rate { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public bool RiskCapEligible { get; set; }
        public bool ShowOnOrderScreen { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Hospices Hospice { get; set; }
        public virtual HospiceLocations HospiceLocation { get; set; }
        public virtual Items Item { get; set; }
        public virtual Users UpdatedByUser { get; set; }
    }
}
