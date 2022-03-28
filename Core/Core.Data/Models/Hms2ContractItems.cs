using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Hms2ContractItems
    {
        public int Id { get; set; }
        public int Hms2ContractItemId { get; set; }
        public int Hms2ContractId { get; set; }
        public int Hms2ItemId { get; set; }
        public int ContractId { get; set; }
        public bool IsPerDiem { get; set; }
        public decimal? RentalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public bool ShowOnOrderScreen { get; set; }
        public bool NoApprovalRequired { get; set; }
        public int? ItemId { get; set; }

        public virtual Hms2Contracts Contract { get; set; }
        public virtual Items Item { get; set; }
    }
}
