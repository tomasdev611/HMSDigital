using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class ItemCategoryMapping
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? ItemCategoryId { get; set; }

        public virtual Items Item { get; set; }
        public virtual ItemCategories ItemCategory { get; set; }
    }
}
