using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class AddOnsGroupProduct
    {
        public int GroupId { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public double? Rate { get; set; }

        public IEnumerable<string> ItemImageUrls { get; set; }
    }
}
