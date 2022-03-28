
namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryVendorPricing
    {
        public int PricingId { get; set; }
        public int? VendorId { get; set; }
        public int? InventoryId { get; set; }
        public decimal? VendorPrice { get; set; }
        public string VendorModel { get; set; }
        public string UnitOfMeasure { get; set; }
        public int? Inactive { get; set; }
    }
}
