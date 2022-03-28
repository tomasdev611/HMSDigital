using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSItemRequest
    {
        public string Type { get; set; }

        [JsonProperty("displayName")]
        public string Name { get; set; }

        public string ItemNumber { get; set; }

        public IEnumerable<NSItemCategory> Categories { get; set; }
                
        public string Description { get; set; }
        
        public bool IsAssetTagged { get; set; }
        
        public decimal Depreciation { get; set; }
        
        public decimal AverageCost { get; set; }
        
        public string CogsAccountName { get; set; }
        
        public decimal AvgDeliveryProcessingTime { get; set; }
        
        public decimal AvgPickUpProcessingTime { get; set; }
        
        public string CreatedByUserEmail { get; set; }

        [JsonProperty("internalItemId")]
        public int NetSuiteItemId { get; set; }

        public IEnumerable<NSInventory> Inventory { get; set; }

        public bool IsSerialized { get; set; }

        public bool IsLotNumbered { get; set; }

        public bool IsInactive { get; set; }

        public bool IsDME { get; set; }

        public bool IsConsumable { get; set; }
    }
}
