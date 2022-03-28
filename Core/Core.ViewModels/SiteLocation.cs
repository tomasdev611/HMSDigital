using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class SiteLocation : Site
    {
        public List<SiteLocation> Vehicles { get; set; }
    }
}
