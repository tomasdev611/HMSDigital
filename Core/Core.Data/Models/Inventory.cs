using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            PatientInventory = new HashSet<PatientInventory>();
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public string SerialNumber { get; set; }
        public int QuantityAvailable { get; set; }
        public int? QuantityOnHand { get; set; }
        public int StatusId { get; set; }
        public int CurrentLocationId { get; set; }
        public int? NetSuiteInventoryId { get; set; }
        public string LotNumber { get; set; }
        public DateTime? LotExpirationDate { get; set; }
        public string AssetTagNumber { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime DeletedDateTime { get; set; }
        public string AdditionalField1 { get; set; }
        public int? AdditionalField2 { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Users DeletedByUser { get; set; }
        public virtual Items Item { get; set; }
        public virtual InventoryStatusTypes Status { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<PatientInventory> PatientInventory { get; set; }
    }
}
