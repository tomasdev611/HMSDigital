using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class LoadlistItem
    {
        public int ItemId { get; set; }

        public Item Item { get; set; }

        public int ItemCount { get; set; }

        public string DispatchType { get; set; }

        public string DispatchStatus { get; set; }
    }
}
