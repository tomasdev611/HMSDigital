using System;
using System.Collections.Generic;

namespace MobileApp.Models
{
    public class OrderFulfillmentLineItem
    {  public int Id { get; set; }
        
        public int OrderHeaderId { get; set; }
        
        public int OrderLineItemId { get; set; }

        public string ItemName { get; set; }
        
        public int Quantity { get; set; }
        
        public string AssetTag { get; set; }
        
        public string LotNumber { get; set; }
        
        public string SerialNumber { get; set; }
        
        public string OrderType { get; set; }
        
        public string DeliveredStatus { get; set; }
      }
}
