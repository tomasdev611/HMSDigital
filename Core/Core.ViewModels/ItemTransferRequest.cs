using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class ItemTransferRequest : ItemTransferCreateRequest
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public Item Item { get; set; }

        public SiteMember DestinationSiteMember { get; set; }
    }
}
