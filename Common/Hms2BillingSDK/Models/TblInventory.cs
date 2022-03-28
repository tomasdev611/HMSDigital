using System;
using System.Collections.Generic;

#nullable disable

namespace Hms2BillingSDK.Models
{
    public partial class TblInventory
    {
        public int InventoryId { get; set; }
        public string Description { get; set; }
        public string InvCode { get; set; }
        public string DbmCode { get; set; }
        public int? CategoryId { get; set; }
        public int Active { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal? DefaultSalePrice { get; set; }
        public decimal? DefaultRentPrice { get; set; }
        public int? AssetTagRequired { get; set; }
        public int? Concentrator { get; set; }
        public int? Disposable { get; set; }
        public string IncludedInvCodes { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? Inventoried { get; set; }
        public decimal? Cost { get; set; }
        public int? Depreciation { get; set; }
        public int? Donated { get; set; }
        public int? ChooseQuantity { get; set; }
        public int? OxygenSetting { get; set; }
        public int? CpapSetting { get; set; }
        public int? BipapSetting { get; set; }
        public int? LotNumRequired { get; set; }
        public int? GlId { get; set; }
        public int? TimeDelivery { get; set; }
        public int? TimePickup { get; set; }
        public int? SerialNumRequired { get; set; }
        public int? BariItem { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public int? CapexItem { get; set; }
        public int? QuantityLimit { get; set; }
        public bool RegMaintRequired { get; set; }
        public int? MaintInterval { get; set; }
    }
}
