using HMSDigital.Common.ViewModels;
using System;

namespace HMSDigital.Core.ViewModels
{
    public class PatientInventory
    {
        public int Id { get; set; }

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

        public bool IsDME { get; set; }

        public bool IsConsumable { get; set; }

        public int InventoryId { get; set; }

        public int NetSuiteInventoryId { get; set; }

        public string SerialNumber { get; set; }

        public string LotNumber { get; set; }

        public string AssetTagNumber { get; set; }

        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public int Quantity
        {
            get { return _count; }
            set { _count = value; }
        }

        public int DeletedByUserId { get; set; }

        public DateTime DeletedDateTime { get; set; }

        public string DeletedByUserName { get; set; }

        public Guid DeliveryAddressUuid { get; set; }

        public Address DeliveryAddress { get; set; }

        public bool IsExceptionFulfillment { get; set; }

        public bool IsPartOfExistingPickup { get; set; }

        public int ExistingPickupCount { get; set; }
    }
}
