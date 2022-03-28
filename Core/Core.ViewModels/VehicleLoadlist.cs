using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class VehicleLoadlist
    {
        public int VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public IEnumerable<LoadlistItem> Items { get; set; }

        public IEnumerable<OrderHeader> Orders { get; set; }

        public IEnumerable<ItemTransferRequest> TransferRequests { get; set; }

        public int TotalItemCount { get; set; }

        public int TotalInventoryCount { get; set; }

        public int TotalOrderCount { get; set; }
    }
}
