using SQLite;

namespace MobileApp.DataBaseAttributes
{
    public class ScanItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public bool IsScanned { get; set; }

        public string SerialNumber { get; set; }

        public string DispatchType { get; set; }

        public int DispatchTypeId { get; set; }

        public string AssetTag { get; set; }

        public string LotNumber { get; set; }

        public int OrderLineItemId { get; set; }

        public int ItemId { get; set; }

        public int QuantityScanned { get; set; }

        public bool IsCompleted { get; set; }

        public int VehicleId { get; set; }

        public int OrderId { get; set; }
    }
}
