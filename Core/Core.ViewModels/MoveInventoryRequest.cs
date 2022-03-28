namespace HMSDigital.Core.ViewModels
{
    public class MoveInventoryRequest
    {
        public int DestinationLocationId { get; set; }

        public string SerialNumber { get; set; }

        public string AssetTagNumber { get; set; }

        public string ItemNumber { get; set; }

    }
}
