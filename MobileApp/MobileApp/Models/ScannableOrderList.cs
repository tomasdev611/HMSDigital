using MobileApp.DataBaseAttributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MobileApp.Models
{
    public class ScannableOrderList : ScannableLoadList
    {
        public string RequestedAssetTag { get; set; }

        public string RequestedSerialNumber { get; set; }

        public string RequestedLotNumber { get; set; }

        public string DispatchType { get; set; }

        public int DispatchTypeId { get; set; }

        public bool IsCompleted { get; set; }

        public int QuantityToFulfill { get; set; }

        public Guid PatientUuId { get; set; }

        public ScannableOrderList(
            List<ScanItem> list,
            OrderLineItem orderLineItem,
            Guid patientUuid,
            int quantityToFulfill) : base(list, orderLineItem)
        {
            RequestedAssetTag = orderLineItem.AssetTagNumber;
            RequestedLotNumber = orderLineItem.LotNumber;
            RequestedSerialNumber = orderLineItem.SerialNumber;
            DispatchType = orderLineItem.Action;
            DispatchTypeId = orderLineItem.ActionId;
            IsCompleted = orderLineItem.Status == "Completed";
            QuantityToFulfill = quantityToFulfill;
            PatientUuId = patientUuid;
        }
    }
}

