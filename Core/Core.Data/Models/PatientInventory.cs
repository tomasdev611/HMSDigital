using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class PatientInventory
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ItemCount { get; set; }
        public int? OrderHeaderId { get; set; }
        public int? HospiceId { get; set; }
        public int? HospiceLocationId { get; set; }
        public Guid PatientUuid { get; set; }
        public Guid? DeliveryAddressUuid { get; set; }
        public int StatusId { get; set; }
        public int? InventoryId { get; set; }
        public int? OrderLineItemId { get; set; }
        public Guid? DataBridgeRunUuid { get; set; }
        public DateTime? DataBridgeRunDateTime { get; set; }
        public bool IsExceptionFulfillment { get; set; }
        public string AdditionalField1 { get; set; }
        public int? AdditionalField2 { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Items Item { get; set; }
        public virtual OrderHeaders OrderHeader { get; set; }
        public virtual OrderLineItems OrderLineItem { get; set; }
        public virtual InventoryStatusTypes Status { get; set; }
    }
}
