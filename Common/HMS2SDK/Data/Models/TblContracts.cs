using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblContracts
    {
        public int ContractId { get; set; }
        public string ContractName { get; set; }
        public string ContractNumber { get; set; }
        public int? HospiceId { get; set; }
        public decimal? PerDiemRate { get; set; }
        public int? CustomerId { get; set; }
        public string UtilizationItem1 { get; set; }
        public string UtilizationItem2 { get; set; }
        public string UtilizationItem3 { get; set; }
        public string UtilizationItem4 { get; set; }
        public string UtilizationItem5 { get; set; }
        public string UtilizationItem6 { get; set; }
        public string UtilizationItem7 { get; set; }
        public string UtilizationItem8 { get; set; }
        public string UtilizationItem9 { get; set; }
        public int? UtilizationQty1 { get; set; }
        public int? UtilizationQty2 { get; set; }
        public int? UtilizationQty3 { get; set; }
        public int? UtilizationQty4 { get; set; }
        public int? UtilizationQty5 { get; set; }
        public int? UtilizationQty6 { get; set; }
        public int? UtilizationQty7 { get; set; }
        public int? UtilizationQty8 { get; set; }
        public int? UtilizationQty9 { get; set; }
        public decimal? OverageFee1 { get; set; }
        public decimal? OverageFee2 { get; set; }
        public decimal? OverageFee3 { get; set; }
        public decimal? OverageFee4 { get; set; }
        public decimal? OverageFee5 { get; set; }
        public decimal? OverageFee6 { get; set; }
        public decimal? OverageFee7 { get; set; }
        public decimal? OverageFee8 { get; set; }
        public decimal? OverageFee9 { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? UtilizationPctg1 { get; set; }
        public decimal? UtilizationPctg2 { get; set; }
        public decimal? UtilizationPctg3 { get; set; }
        public decimal? UtilizationPctg4 { get; set; }
        public decimal? UtilizationPctg5 { get; set; }
        public decimal? UtilizationPctg6 { get; set; }
        public decimal? UtilizationPctg7 { get; set; }
        public decimal? UtilizationPctg8 { get; set; }
        public decimal? UtilizationPctg9 { get; set; }
        public string MileageItem { get; set; }
        public int? MaxMileage { get; set; }
        public string Filename { get; set; }
        public decimal? SalesTaxRate { get; set; }
        public int? DetailBilling { get; set; }
        public string Comments { get; set; }
        public int? DmeId { get; set; }
        public int? CombineTanks { get; set; }
        public int? Bill2ndConc { get; set; }
        public int? DeliveryCharge { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public int? UpdateUserId { get; set; }
    }
}
