using MobileApp.DataBaseAttributes;
using MobileApp.Methods;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MobileApp.Models
{
    public class ScannableLoadList : ScannableList
    {
        public bool IsStandAlone { get; set; }

        public int QuantityToLoad { get; set; }

        public int ItemId { get; set; }

        public IDictionary<string, string> EquipmentSettings { get; set; }

        public int OrderLineItemId { get; set; }

        public ScannableLoadList(
            List<ScanItem> list,
            OrderLineItem orderLineItem) : base(list, orderLineItem.Item.Name)
        {
            IsStandAlone = !(orderLineItem.Item.IsSerialized || orderLineItem.Item.IsLotNumbered || orderLineItem.Item.IsAssetTagged);
            ItemId = orderLineItem.ItemId;
            EquipmentSettings = CommonUtility.ConvertJArrayToDictionary(orderLineItem.EquipmentSettings);
            OrderLineItemId = orderLineItem.Id;
            QuantityToLoad = orderLineItem.ItemCount;
        }
    }
}

