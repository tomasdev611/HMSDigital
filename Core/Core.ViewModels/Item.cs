using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class Item : ItemRequest
    {
        public int Id { get; set; }

        public IEnumerable<ItemCategory> Categories { get; set; }

        public IEnumerable<ItemSubCategory> SubCategories { get; set; }

        public int NetSuiteItemId { get; set; }

        public bool IsDME { get; set; }

        public bool IsConsumable { get; set; }

        public IEnumerable<EquipmentSettingConfig> EquipmentSettingsConfig { get; set; }

        public IEnumerable<AddOnsGroup> AddOnGroups { get; set; }

    }
}
