using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class ItemSubCategories
    {
        public ItemSubCategories()
        {
            ItemSubCategoryMapping = new HashSet<ItemSubCategoryMapping>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int? NetSuiteSubCategoryId { get; set; }

        public virtual ItemCategories Category { get; set; }
        public virtual ICollection<ItemSubCategoryMapping> ItemSubCategoryMapping { get; set; }
    }
}
