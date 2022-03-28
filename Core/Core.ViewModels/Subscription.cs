using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class Subscription
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? NetSuiteSubscriptionId { get; set; }

        public int? NetSuiteCustomerId { get; set; }

        public int? HospiceId { get; set; }

        public int? NetSuiteBillToCustomerId { get; set; }

        public string BillToCustomer { get; set; }

        public int? NetSuiteBillToEntityId { get; set; }

        public string BillToEntity { get; set; }

        public bool? ConsolidateBilling { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? NetSuiteCurrencyId { get; set; }

        public string Currency { get; set; }

        public int? NetSuiteBillingProfileId { get; set; }

        public string BillingProfile { get; set; }

        public int? NetSuiteChargeScheduleId { get; set; }

        public string ChargeSchedule { get; set; }

        public bool? InheritChargeScheduleFromMasterContract { get; set; }

        public int? NetSuiteRenewalTemplateId { get; set; }

        public string RenewalTemplate { get; set; }

        public int? NetSuiteEnableLineItemShippingId { get; set; }

        public string EnableLineItemShipping { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public DateTime? NetSuiteLastFetchedDateTime { get; set; }

        public bool? IsInactive { get; set; }

        public virtual Hospice Hospice { get; set; }

        public virtual IEnumerable<SubscriptionItem> SubscriptionItems { get; set; }
    }
}
