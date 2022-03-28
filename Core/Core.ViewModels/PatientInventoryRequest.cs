using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class PatientInventoryRequest
    {
        public string ItemNumber { get; set; }

        public string SerialNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public string LotNumber { get; set; }

        public int Quantity { get; set; }

        public int HospiceId { get; set; }

        public int HospiceLocationId { get; set; }

        public Guid DataBridgeRunUuid { get; set; }

        public DateTime? DataBridgeRunDateTime { get; set; }

        public int Hms2Id { get; set; }
    }
}
