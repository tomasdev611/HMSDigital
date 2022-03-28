using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class ItemCategories
    {
        public ItemCategories()
        {
            ItemCategoryMapping = new HashSet<ItemCategoryMapping>();
            ItemSubCategories = new HashSet<ItemSubCategories>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? NetSuiteCategoryId { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime DeletedDateTime { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Users DeletedByUser { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<ItemCategoryMapping> ItemCategoryMapping { get; set; }
        public virtual ICollection<ItemSubCategories> ItemSubCategories { get; set; }
    }
}
