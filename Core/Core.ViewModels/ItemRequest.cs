using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class ItemRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ItemNumber { get; set; }

        public string CogsAccountName { get; set; }

        public decimal? Depreciation { get; set; }

        public decimal? AverageCost { get; set; }

        public decimal? AvgDeliveryProcessingTime { get; set; }

        public decimal? AvgPickUpProcessingTime { get; set; }

        public int CategoryId { get; set; }

        public bool IsSerialized { get; set; }

        public bool IsAssetTagged { get; set; }

        public bool IsLotNumbered { get; set; }
    }
}
