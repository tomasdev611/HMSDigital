using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class Products
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string WarehouseName { get; set; }
        public string CatalogName { get; set; }
        public string InvCode { get; set; }
        public string Type { get; set; }
        public bool? Active { get; set; }
        public bool? AssetTagRequired { get; set; }
        public bool? SerialNumRequired { get; set; }
        public bool? LotNumRequired { get; set; }
        public bool? FieldCleanAllowed { get; set; }
        public sbyte? IsSoftGood { get; set; }
        public bool? QuantityTracked { get; set; }
        public string QuantityTrackedAssetTag { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ProductCategories Category { get; set; }
    }
}
