using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class Enumeration
    {
        public IEnumerable<LookUp> AddressTypes { get; set; }

        public IEnumerable<LookUp> PhoneNumberTypes { get; set; }

        public IEnumerable<LookUp> InventoryStatusTypes { get; set; }

        public IEnumerable<LookUp> FileStorageTypes { get; set; }

        public IEnumerable<LookUp> ResourceTypes { get; set; }

        public IEnumerable<LookUp> TransferRequestStatusTypes { get; set; }

        public IEnumerable<LookUp> DispatchInstructionTypes { get; set; }

        public IEnumerable<LookUp> OrderTypes { get; set; }

        public IEnumerable<LookUp> OrderHeaderStatusTypes { get; set; }

        public IEnumerable<LookUp> OrderLineItemStatusTypes { get; set; }
    }
}
