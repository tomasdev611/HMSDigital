using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class SubscriptionItem
    {
        [JsonProperty("internalid")]
        public ValueObj<string> NetSuiteSubscriptionItemId { get; set; }

        [JsonProperty("name")]
        public ValueObj<string> Name { get; set; }

        [JsonProperty("subscription")]
        public ValueTextObj<string> Subscription { get; set; }

        [JsonProperty("rate_type")]
        public ValueTextObj<string> RateType { get; set; }

        [JsonProperty("invert_negative_quantity")]
        public ValueObj<bool> InvertNegativeQuantity { get; set; }

        [JsonProperty("use_alternate_term_multiplier")]
        public ValueObj<bool> UseAlternateTermMultiplier { get; set; }

        [JsonProperty("overage_rate")]
        public ValueObj<string> OverageRate { get; set; }

        [JsonProperty("rate_plan")]
        public ValueTextObj<string> RatePlan { get; set; }

        [JsonProperty("start_date")]
        public ValueObj<string> StartDate { get; set; }

        [JsonProperty("end_date")]
        public ValueObj<string> EndDate { get; set; }

        [JsonProperty("bill_in_arrears")]
        public ValueObj<bool> BillInArrears { get; set; }

        [JsonProperty("rating_schedule_bill_in_arrears")]
        public ValueObj<bool> RatingScheduleBillInArrears { get; set; }

        [JsonProperty("charge_schedule_used")]
        public ValueTextObj<string> ChargeScheduleUsed { get; set; }

        [JsonProperty("currency")]
        public ValueTextObj<string> Currency { get; set; }

        [JsonProperty("customer")]
        public ValueTextObj<string> Customer { get; set; }

        [JsonProperty("exclude_charges_from_billing_when")]
        public ValueTextObj<string> ExcludeChargesFromBillingWhen { get; set; }

        [JsonProperty("exclude_from_order_level_minimum_commitment")]
        public ValueObj<bool> ExcludeFromOrderLevelMinimumCommitment { get; set; }

        [JsonProperty("inherit_charge_schedule_from")]
        public ValueTextObj<string> InheritChargeScheduleFrom { get; set; }

        [JsonProperty("item")]
        public ValueTextObj<string> Item { get; set; }

        [JsonProperty("item_description")]
        public ValueTextObj<string> ItemDescription { get; set; }

        [JsonProperty("proration_type")]
        public ValueTextObj<string> ProRationType { get; set; }

        [JsonProperty("rating_priority")]
        public ValueObj<string> RatingPriority { get; set; }

        [JsonProperty("create_charges_for_included_units")]
        public ValueObj<bool> ChargeIncludedUnits { get; set; }

        [JsonProperty("credit_rebill_prior_new_count_charges")]
        public ValueObj<bool> CreditRebillPriorNewCountCharges { get; set; }

        [JsonProperty("push_future_count_charges")]
        public ValueObj<bool> PushFutureCountCharges { get; set; }

        [JsonProperty("adjustment_exclude_prior_to_effective_date")]
        public ValueObj<bool> AdjustmentExcludePriorToEffectiveDate { get; set; }

        [JsonProperty("apply_adjustments_retroactively")]
        public ValueObj<bool> ApplyAdjustmentsRetroactively { get; set; }

        [JsonProperty("renewal_item")]
        public ValueTextObj<string> RenewalItem { get; set; }

        [JsonProperty("apply_prepaid_to_all_subscription_items")]
        public ValueObj<bool> ApplyPrepaidToAllSubscriptionItems { get; set; }

        [JsonProperty("date_created")]
        public ValueObj<string> Created { get; set; }

        [JsonProperty("last_modified")]
        public ValueObj<string> LastModified { get; set; }

        [JsonProperty("inactive")]
        public ValueObj<bool> IsInactive { get; set; }
    }
}
