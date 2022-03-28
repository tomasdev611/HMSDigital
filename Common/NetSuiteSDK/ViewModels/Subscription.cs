using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class Subscription
    {
        [JsonProperty("internalid")]
        public ValueObj<string> NetSuiteSubscriptionId { get; set; }

        [JsonProperty("customer_internal_id")]
        public ValueObj<string> NetSuiteCustomerId { get; set; }

        [JsonProperty("name")]
        public ValueObj<string> Name { get; set; }

        [JsonProperty("customer")]
        public ValueTextObj<string> Customer { get; set; }

        [JsonProperty("bill_to_customer")]
        public ValueTextObj<string> BillToCustomer { get; set; }

        [JsonProperty("bill_to_entity")]
        public ValueTextObj<string> BillToEntity { get; set; }

        [JsonProperty("consolidate_billing")]
        public ValueObj<bool> ConsolidateBilling { get; set; }

        [JsonProperty("start_date")]
        public ValueObj<string> StartDate { get; set; }

        [JsonProperty("end_date")]
        public ValueObj<string> EndDate { get; set; }

        [JsonProperty("currency")]
        public ValueTextObj<string> Currency { get; set; }

        [JsonProperty("billing_profile")]
        public ValueTextObj<string> BillingProfile { get; set; }

        [JsonProperty("charge_schedule")]
        public ValueTextObj<string> ChargeSchedule { get; set; }

        [JsonProperty("inherit_charge_schedule_from_master_contract")]
        public ValueObj<bool> InheritChargeScheduleFromMasterContract { get; set; }

        [JsonProperty("renewal_template")]
        public ValueTextObj<string> RenewalTemplate { get; set; }

        [JsonProperty("enable_line_item_shipping")]
        public ValueTextObj<string> EnableLineItemShipping { get; set; }

        [JsonProperty("date_created")]
        public ValueObj<string> Created { get; set; }

        [JsonProperty("last_modified")]
        public ValueObj<string> LastModified { get; set; }

        [JsonProperty("inactive")]
        public ValueObj<bool> IsInactive { get; set; }
    }
}
