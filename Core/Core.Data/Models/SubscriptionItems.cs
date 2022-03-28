using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class SubscriptionItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? NetSuiteSubscriptionItemId { get; set; }
        public int? SubscriptionId { get; set; }
        public int? NetSuiteSubscriptionId { get; set; }
        public int? NetSuiteCustomerId { get; set; }
        public int? HospiceId { get; set; }
        public int? NetSuiteItemId { get; set; }
        public int? ItemId { get; set; }
        public string ItemDescription { get; set; }
        public int? NetSuiteRateTypeId { get; set; }
        public string RateType { get; set; }
        public bool? InvertNegativeQuantity { get; set; }
        public bool? UseAlternateTermMultiplier { get; set; }
        public double? OverageRate { get; set; }
        public int? NetSuiteRatePlanId { get; set; }
        public string RatePlan { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? BillInArrears { get; set; }
        public bool? RatingScheduleBillInArrears { get; set; }
        public int? NetSuiteChargeScheduleUsedId { get; set; }
        public string ChargeScheduleUsed { get; set; }
        public int? NetSuiteCurrencyId { get; set; }
        public string Currency { get; set; }
        public int? NetSuiteExcludeChargesFromBillingWhenId { get; set; }
        public string ExcludeChargesFromBillingWhen { get; set; }
        public bool? ExcludeFromOrderLevelMinimumCommitment { get; set; }
        public int? NetSuiteInheritChargeScheduleFromId { get; set; }
        public string InheritChargeScheduleFrom { get; set; }
        public int? NetSuiteProRationTypeId { get; set; }
        public string ProRationType { get; set; }
        public int? RatingPriority { get; set; }
        public bool? ChargeIncludedUnits { get; set; }
        public bool? CreditRebillPriorNewCountCharges { get; set; }
        public bool? PushFutureCountCharges { get; set; }
        public bool? AdjustmentExcludePriorToEffectiveDate { get; set; }
        public bool? ApplyAdjustmentsRetroactively { get; set; }
        public int? NetSuiteRenewalItemId { get; set; }
        public string RenewalItem { get; set; }
        public bool? ApplyPrepaidToAllSubscriptionItems { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public DateTime? NetSuiteLastFetchedDateTime { get; set; }
        public bool? IsInactive { get; set; }

        public virtual Hospices Hospice { get; set; }
        public virtual Items Item { get; set; }
        public virtual Subscriptions Subscription { get; set; }
    }
}
