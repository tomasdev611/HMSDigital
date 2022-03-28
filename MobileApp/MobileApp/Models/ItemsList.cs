using System.Collections.ObjectModel;
using MobileApp.DataBaseAttributes;

namespace MobileApp.Models
{
    public class ItemsList
    {
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public string ItemName { get; set; }

        public string DispatchType { get; set; }

        public string AssetTag { get; set; }

        public string SerialNumber { get; set; }

        public string LotNumber { get; set; }

        public bool IsSerialized { get; set; }

        public bool IsAssetTagged { get; set; }

        public bool IsLotNumbered { get; set; }

        public int OrderLineItemId { get; set; }

        public ObservableCollection<ScanItem> Items { get; set; }

        public bool IsPending { get; set; }
    }
}
