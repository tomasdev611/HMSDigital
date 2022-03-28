using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.Data.Enums
{
    public enum OrderLineItemStatusTypes
    {
        Planned = 10,
        Scheduled = 11,
        Staged = 12,
        OnTruck = 13,
        OutForFulfillment = 14,
        Completed = 15,
        BackOrdered = 16,
        Cancelled = 17,
        Partial_Fulfillment = 21,
        PreLoad = 22,
        Loading_Truck = 23,
        Enroute = 24,
        OnSite = 25
    }
}