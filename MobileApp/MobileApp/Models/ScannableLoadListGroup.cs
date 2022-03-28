using MobileApp.DataBaseAttributes;
using MobileApp.Methods;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MobileApp.Models
{
    public class ScannableLoadListGroup : ScannableList
    {
        public bool IsStandAlone { get; set; }

        public int QuantityToLoad { get; set; }

        public int ItemId { get; set; }

        public List<string> EquipmentSettings { get; set; }

        public int OrderLineItemId { get; set; }

        public ScannableLoadListGroup(
            List<ScanItem> list,
            LoadlistItem loadListItem,
            List<JArray> equipmentSettings ) : base(list, loadListItem.Item.Name)
        {
            IsStandAlone = !(loadListItem.Item.IsSerialized || loadListItem.Item.IsLotNumbered || loadListItem.Item.IsAssetTagged);
            ItemId = loadListItem.ItemId;
            EquipmentSettings = new List<string>();
            if(equipmentSettings != null)
            {
                foreach(var equipementSetting in equipmentSettings)
                {
                    var settingsDict = CommonUtility.ConvertJArrayToDictionary(equipementSetting);
                    foreach(var setting in settingsDict)
                    {
                        EquipmentSettings.Add($"{setting.Key} : {setting.Value}");
                    }
                }
            }
            QuantityToLoad = loadListItem.ItemCount;
        }
    }
}

