using System;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteHmsDispatch
    {
        public DateTime? SODate { get; set; }

        public int? CustomerLocationId { get; set; }

        public string HmsStatus { get; set; }

        public int? Qty { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int? PatientId { get; set; }

        public string HmsOrderType { get; set; }

        public int? NetsuiteItemId { get; set; }

        public int? NetsuiteWarehouseId { get; set; }

        public DateTime? ConsumableDeliveryDate { get; set; }

        public int? OrderId { get; set; }

        public int? HospiceId { get; set; }

        public string PatientGuid { get; set; }

        public DateTime? ExpDeliveryDate { get; set; }

        public int? TotalItemsOrdered { get; set; }

        public int? TotalItemDelivered { get; set; }

        public int? NSInternalItemId { get; set; }

        public string StatusCd { get; set; }

        public DateTime? HmsActualDeliveryDate { get; set; }

        public string DeliveredBy { get; set; }

        public string ReasonCd { get; set; }

        public DateTime? CreatedDt { get; set; }

        public string CreatedBy { get; set; }

        public string PricingArea { get; set; }

        public string PriceLevel { get; set; }

        public int? ServiceItem { get; set; }

        public int? CustomerParent { get; set; }

        public int? scaDeliveryTransactionId { get; set; }

        public int? customerId { get; set; }

        public DateTime? HmsDeliveryDate { get; set; }

        public DateTime? HmsPickupRequestDate { get; set; }

        public string LineUniqueId { get; set; }

        public string HmsDeliveryOrderType { get; set; }

        public string HmsOrderStatus { get; set; }

        public string PickUpRequestOrderType { get; set; }

        public string HmsPickupOrderStatus { get; set; }

        public int? ScaPickupTranOrderId { get; set; }

        public DateTime? PickupDate { get; set; }

        public string SOLineUniqueId { get; set; }

        public string SerielLotNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public int? NSDispatchId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsInactive { get; set; }

    }
}
