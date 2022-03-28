using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSAddInventoryResponse
    {
        public int NetsuiteItemId { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        public int Quantity { get; set; }
    }
}
