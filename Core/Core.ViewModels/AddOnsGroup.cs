using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class AddOnsGroup : AddOnsConfigBase
    {
        public IEnumerable<AddOnsGroupProduct> AddOnGroupProducts { get; set; }
    }
}
