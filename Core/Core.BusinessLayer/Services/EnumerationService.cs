using HMSDigital.Common.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class EnumerationService : IEnumerationService
    {
        public EnumerationService()
        {
        }

        public Enumeration GetAllEnumerations()
        {
            return new Enumeration()
            {
                AddressTypes = GetValues<AddressType>(),
                PhoneNumberTypes = GetValues<PhoneNumberType>(),
                InventoryStatusTypes = GetValues<InventoryStatusTypes>(),
                FileStorageTypes = GetValues<FileStorageTypes>(),
                ResourceTypes = GetValues<ResourceTypes>(),
                TransferRequestStatusTypes = GetValues<TransferRequestStatusTypes>(),
                OrderTypes = GetValues<OrderTypes>(),
                OrderHeaderStatusTypes = GetValues<OrderHeaderStatusTypes>(),
                OrderLineItemStatusTypes = GetValues<OrderLineItemStatusTypes>()
            };
        }

        private IEnumerable<LookUp> GetValues<T>()
        {
            var lookUpValues = new List<LookUp>();
            foreach (var itemType in Enum.GetValues(typeof(T)))
            {
                lookUpValues.Add(new LookUp()
                {
                    Name = Enum.GetName(typeof(T), itemType),
                    Id = (int)itemType
                });
            }
            return lookUpValues;
        }
    }
}
