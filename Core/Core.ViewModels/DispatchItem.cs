using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class DispatchItem
    {
        public int Count { get; set; }

        public string SerialNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public string LotNumber { get; set; }

        public int? ItemId { get; set; }

        public int OrderLineItemId { get; set; }
    }
}
