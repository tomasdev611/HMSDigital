using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Items
    {
        public Items()
        {
            AddOnGroupProducts = new HashSet<AddOnGroupProducts>();
            AddOnGroups = new HashSet<AddOnGroups>();
            ContractRecords = new HashSet<ContractRecords>();
            EquipmentSettingsConfig = new HashSet<EquipmentSettingsConfig>();
            Hms2ContractItems = new HashSet<Hms2ContractItems>();
            Inventory = new HashSet<Inventory>();
            ItemCategoryMapping = new HashSet<ItemCategoryMapping>();
            ItemImageFiles = new HashSet<ItemImageFiles>();
            ItemImages = new HashSet<ItemImages>();
            ItemSubCategoryMapping = new HashSet<ItemSubCategoryMapping>();
            ItemTransferRequests = new HashSet<ItemTransferRequests>();
            OrderFulfillmentLineItems = new HashSet<OrderFulfillmentLineItems>();
            OrderLineItems = new HashSet<OrderLineItems>();
            PatientInventory = new HashSet<PatientInventory>();
            SubscriptionItems = new HashSet<SubscriptionItems>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ItemNumber { get; set; }
        public bool IsAssetTagged { get; set; }
        public decimal? Depreciation { get; set; }
        public decimal? AverageCost { get; set; }
        public string CogsAccountName { get; set; }
        public decimal? AvgDeliveryProcessingTime { get; set; }
        public decimal? AvgPickUpProcessingTime { get; set; }
        public int? NetSuiteItemId { get; set; }
        public bool IsSerialized { get; set; }
        public bool IsLotNumbered { get; set; }
        public bool IsConsumable { get; set; }
        public bool IsDme { get; set; }
        public bool IsInactive { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime DeletedDateTime { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Users DeletedByUser { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual ICollection<AddOnGroupProducts> AddOnGroupProducts { get; set; }
        public virtual ICollection<AddOnGroups> AddOnGroups { get; set; }
        public virtual ICollection<ContractRecords> ContractRecords { get; set; }
        public virtual ICollection<EquipmentSettingsConfig> EquipmentSettingsConfig { get; set; }
        public virtual ICollection<Hms2ContractItems> Hms2ContractItems { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<ItemCategoryMapping> ItemCategoryMapping { get; set; }
        public virtual ICollection<ItemImageFiles> ItemImageFiles { get; set; }
        public virtual ICollection<ItemImages> ItemImages { get; set; }
        public virtual ICollection<ItemSubCategoryMapping> ItemSubCategoryMapping { get; set; }
        public virtual ICollection<ItemTransferRequests> ItemTransferRequests { get; set; }
        public virtual ICollection<OrderFulfillmentLineItems> OrderFulfillmentLineItems { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItems { get; set; }
        public virtual ICollection<PatientInventory> PatientInventory { get; set; }
        public virtual ICollection<SubscriptionItems> SubscriptionItems { get; set; }
    }
}
