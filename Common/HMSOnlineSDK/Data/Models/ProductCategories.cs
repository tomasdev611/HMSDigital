using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class ProductCategories
    {
        public ProductCategories()
        {
            InverseParentCategory = new HashSet<ProductCategories>();
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }
        public string WarehouseName { get; set; }
        public string CatalogName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ProductCategories ParentCategory { get; set; }
        public virtual ICollection<ProductCategories> InverseParentCategory { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
