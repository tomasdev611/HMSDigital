using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.Data.Enums
{
    [Flags]
    public enum GlobalFilters
    {
        Site = 1,
        Hospice = 2,
        HospiceLocation = 4,
        All = Site | Hospice | HospiceLocation
    }
}
