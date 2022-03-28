using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class InventoryRequest
    {
        public int ItemId { get; set; }

        public string SerialNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public string LotNumber { get; set; }

        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public int QuantityAvailable
        {
            get { return _count; }
            set { _count = value; }
        }

        public int? CurrentLocationId { get; set; }
    }
}
