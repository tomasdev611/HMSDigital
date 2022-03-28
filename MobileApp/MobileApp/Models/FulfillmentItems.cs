namespace MobileApp.Models
{
    public class FulfillmentItems : DispatchItems
    {
        public string FulfillmentType { get; set; }

        public int OrderLineItemId { get; set; }
    }
}