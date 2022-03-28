using System;
using System.Collections.Generic;
using MobileApp.ViewModels;

namespace MobileApp.Models
{
    public class EnumerationList
    {
        public IEnumerable<LookUp> AddressTypes { get; set; }

        public IEnumerable<LookUp> PhoneNumberTypes { get; set; }

        public IEnumerable<LookUp> InventoryLocationTypes { get; set; }

        public IEnumerable<LookUp> InventoryStatusTypes { get; set; }

        public IEnumerable<LookUp> FileStorageTypes { get; set; }

        public IEnumerable<LookUp> ResourceTypes { get; set; }

        public IEnumerable<LookUp> TransferRequestStatusTypes { get; set; }

        public IEnumerable<LookUp> DispatchInstructionStatusTypes { get; set; }

        public IEnumerable<LookUp> DispatchInstructionTypes { get; set; }
    }
}
