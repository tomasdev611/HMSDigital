
namespace HMS2SDK.Data.Models
{
    public partial class TblPurchaseOrderItems
    {
        public int PoitemId { get; set; }
        public int? PoId { get; set; }
        public int? PricingId { get; set; }
        public int? Quantity { get; set; }
        public string PoitemNotes { get; set; }
        public decimal? ActualPrice { get; set; }
        public int? QuantityReceived { get; set; }
        public int? Processed { get; set; }
        public int? PurchaseReason { get; set; }
        public int? SpecRequestCustomer { get; set; }
        public int? SpecRequestOff { get; set; }
        public string SpecRequestNotes { get; set; }
        public int? SpecRequestBari { get; set; }
        public int? GrowthCustomer { get; set; }
        public int? GrowthCensus { get; set; }
        public int? GrowthReason { get; set; }
        public string GrowthNotes { get; set; }
        public int? MainReason { get; set; }
        public string MainWoNums { get; set; }
        public int? MainSpend { get; set; }
        public int? MainDeliveries { get; set; }
        public int? MainPickups { get; set; }
        public string MainNotes { get; set; }
        public int? BariCustomer { get; set; }
        public string BariNotes { get; set; }
    }
}
