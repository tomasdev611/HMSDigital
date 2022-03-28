using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class ItemSubCategoryMapping
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? ItemSubCategoryId { get; set; }

        public virtual Items Item { get; set; }
        public virtual ItemSubCategories ItemSubCategory { get; set; }
    }
}
