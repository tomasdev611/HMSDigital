using System;
using System.Collections.Generic;


namespace HMSDigital.Core.ViewModels
{
    public partial class Hms2ContractItem
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

        public Item Item { get; set; }
    }
}
