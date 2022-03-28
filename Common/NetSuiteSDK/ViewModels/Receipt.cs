using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class Receipt
    {
        public DateTime DateCreated { get; set; }

        public int ReceiptId { get; set; }

        public IEnumerable<ReceiptLineItem> ItemLines { get; set; }
    }
}
