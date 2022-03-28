using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class DispatchRecordRequest
    {
        public string Action { get; set; }

        public int? NetSuiteDispatchRecordId { get; set; }

        public DateTime OriginalSalesOrderDateTime { get; set; }

        public string HmsStatus { get; set; }

        public int? Quantity { get; set; }

        public DateTime EffectiveDateTime { get; set; }

        public int? NetSuiteCustomerId { get; set; }

        public string HmsOrderType { get; set; }

        public int? NetsuiteItemId { get; set; }

        public int? NetsuiteWarehouseId { get; set; }

        public DateTime ConsumableDeliveryDateTime { get; set; }

        public int? OrderId { get; set; }

        public int? HospiceId { get; set; }

        public Guid PatientGuid { get; set; }

        public DateTime ExpectedDeliveryDateTime { get; set; }

        public string TotalItemsOrdered { get; set; }

        public string TotalItemDelivered { get; set; }

        public string StatusCd { get; set; }

        public DateTime HmsActualDeliveryDateTime { get; set; }

        public string DeliveredBy { get; set; }

        public string ReasonCd { get; set; }

        public DateTime OrderCreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public string PricingArea { get; set; }

        public string PriceLevel { get; set; }

        public int? ServiceItem { get; set; }

        public string CustomerParent { get; set; }

        public int? ScaDeliveryTransactionId { get; set; }

        public int? NetSuiteContractingCustomerId { get; set; }

        public DateTime HmsDeliveryDateTime { get; set; }

        public DateTime HmsPickupRequestDateTime { get; set; }

        public string LineUniqueId { get; set; }

        public string HmsdeliveryOrderType { get; set; }

        public string HmsorderStatus { get; set; }

        public string PickupRequestOrderType { get; set; }

        public string HmsPickupOrderStatus { get; set; }

        public int? ScaPickupTransactionOrderId { get; set; }

        public DateTime ActualPickupDateTime { get; set; }

        public string SalesOrderLineUniqueId { get; set; }

        public string SerialOrLotNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool IsInactive { get; set; }
    }
}
