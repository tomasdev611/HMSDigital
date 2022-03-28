namespace HospiceSource.Digital.Patient.SDK.ViewModels
{
    public class PatientInventory
    {
        public string PatientUuid { get; set; }

        public int OrderHeaderId { get; set; }

        public int NetSuiteOrderId { get; set; }

        public int NetSuiteOrderLineItemId { get; set; }

        public int ItemId { get; set; }

        public int NetSuiteItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public string ItemNumber { get; set; }

        public string CogsAccountName { get; set; }

        public int InventoryId { get; set; }

        public int NetSuiteInventoryId { get; set; }

        public string SerialNumber { get; set; }

        public string LotNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public int Quantity { get; set; }
    }
}
