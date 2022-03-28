using System;

namespace HMSDigital.Core.ViewModels
{
    public class OrderLocation
    {
        public int OrderId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}
