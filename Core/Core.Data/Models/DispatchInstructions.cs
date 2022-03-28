using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class DispatchInstructions
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int? SequenceNumber { get; set; }
        public int? OrderHeaderId { get; set; }
        public int? TransferRequestId { get; set; }
        public DateTime? DispatchStartDateTime { get; set; }
        public DateTime? DispatchEndDateTime { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual OrderHeaders OrderHeader { get; set; }
        public virtual ItemTransferRequests TransferRequest { get; set; }
        public virtual Users UpdatedByUser { get; set; }
        public virtual Sites Vehicle { get; set; }
    }
}
