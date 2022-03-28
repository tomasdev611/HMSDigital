using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class ItemTransferCreateRequest
    {
        public int SourceLocationId { get; set; }

        public int DestinationLocationId { get; set; }

        public int? DestinationSiteMemberId { get; set; }

        public int ItemCount { get; set; }
    }
}
