using System;
using MobileApp.DataBaseAttributes;
using Xamarin.Forms;

namespace MobileApp.Utils.TemplateSelector
{
    public class InventoryDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SerializedInventoryTemplate { get; set; }
        public DataTemplate StandAloneInventoryTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var scanItem = (ScanItem)item;
            var isStandAlone = string.IsNullOrEmpty(scanItem.AssetTag) && string.IsNullOrEmpty(scanItem.SerialNumber) && string.IsNullOrEmpty(scanItem.LotNumber);
            return isStandAlone ? StandAloneInventoryTemplate : SerializedInventoryTemplate;
        }
    }
}
