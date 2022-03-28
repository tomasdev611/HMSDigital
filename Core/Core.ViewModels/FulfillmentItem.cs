namespace HMSDigital.Core.ViewModels
{
    public class FulfillmentItem
    {
        public int Count { get; set; }

        public string SerialNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public string LotNumber { get; set; }

        public int? ItemId { get; set; }

        public int OrderLineItemId { get; set; }

        public string FulfillmentType { get; set; }  // pickup or delivery
    }
}
