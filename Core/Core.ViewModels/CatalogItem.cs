using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class CatalogItem
    {
        public Item Item { get; set; }

        public double? Rate { get; set; }

        public IEnumerable<string> ItemImageUrls { get; set; }

        public IEnumerable<string> EquipmentSettingFields { get; set; }
    }
}
