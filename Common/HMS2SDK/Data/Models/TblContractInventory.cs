using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblContractInventory
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
        public int? HzOnly { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public int? UpdateUserId { get; set; }
    }
}
