using Newtonsoft.Json.Linq;

namespace HMSDigital.Core.ViewModels
{
    public class CoreOrderLineItemRequest
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int ItemCount { get; set; }

        public string Action { get; set; }

        public JArray EquipmentSettings { get; set; }

        public string SerialNumber { get; set; }

        public string LotNumber { get; set; }

        public string AssetTagNumber { get; set; }
    }
}
