using System;
using Newtonsoft.Json.Linq;

namespace MobileApp.Models
{
    public class OrderLineItem
    {
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }

        public int SiteId { get; set; }

        public int ItemId { get; set; }

        public int ItemCount { get; set; }

        public string Status { get; set; }

        public int StatusId { get; set; }

        public string Action { get; set; }

        public int ActionId { get; set; }

        public DateTime RequestedStartDateTime { get; set; }

        public DateTime RequestedEndDateTime { get; set; }

        public Item Item { get; set; }

        public Address DeliveryAddress { get; set; }

        public Address PickupAddress { get; set; }

        public Site Site { get; set; }

        public string SerialNumber { get; set; }

        public string LotNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public JArray EquipmentSettings { get; set; }
    }
}
