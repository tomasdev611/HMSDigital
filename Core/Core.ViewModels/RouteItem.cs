using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class RouteItem
    {
        public int SequenceNumber { get; set; }

        public Address Address { get; set; }

        public int OrderHeaderId { get; set; }
    }
}
