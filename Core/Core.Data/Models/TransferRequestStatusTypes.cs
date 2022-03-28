using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class TransferRequestStatusTypes
    {
        public TransferRequestStatusTypes()
        {
            ItemTransferRequests = new HashSet<ItemTransferRequests>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ItemTransferRequests> ItemTransferRequests { get; set; }
    }
}
