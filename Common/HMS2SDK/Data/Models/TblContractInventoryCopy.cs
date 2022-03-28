
namespace HMS2SDK.Data.Models
{
    public partial class TblContractInventoryCopy
    {
        public int InvctrId { get; set; }
        public int? ContractId { get; set; }
        public int? InventoryId { get; set; }
        public int Perdiem { get; set; }
        public decimal? RentalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public int OrderScreen { get; set; }
        public int? NumIncluded { get; set; }
        public int NoApprovalRequired { get; set; }
    }
}
