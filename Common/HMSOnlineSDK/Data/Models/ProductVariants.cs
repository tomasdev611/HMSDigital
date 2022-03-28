using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class ProductVariants
    {
        public ProductVariants()
        {
            Items = new HashSet<Items>();
            QuantityTrackedItemsCopy1 = new HashSet<QuantityTrackedItemsCopy1>();
        }

        public int Id { get; set; }
        public int? ManufacturerId { get; set; }
        public string ManufacturerModel { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ProductId { get; set; }

        public virtual Manufacturers Manufacturer { get; set; }
        public virtual ProductsOld Product { get; set; }
        public virtual ICollection<Items> Items { get; set; }
        public virtual ICollection<QuantityTrackedItemsCopy1> QuantityTrackedItemsCopy1 { get; set; }
    }
}
