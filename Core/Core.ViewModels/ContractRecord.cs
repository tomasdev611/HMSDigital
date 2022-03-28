using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class ContractRecord : NSContractRecordRequest
    {
        public int Id { get; set; }

        public int? HospiceId { get; set; }

        public int? HospiceLocationId { get; set; }

        public int? ItemId { get; set; }

        public Hospice Hospice { get; set; }

        public HospiceLocation HospiceLocation { get; set; }

        public Item Item { get; set; }
    }
}
