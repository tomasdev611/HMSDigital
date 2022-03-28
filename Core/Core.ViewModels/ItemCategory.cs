using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class ItemCategory : ItemCategoryRequest
    {
        public int Id { get; set; }

        public IEnumerable<ItemSubCategory> ItemSubCategories { get; set; }
    }
}
