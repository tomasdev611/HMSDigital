using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSItemCategory
    {
        public string Name { get; set; }

        [JsonProperty("internalCategoryId")]
        public int NetSuiteCategoryId { get; set; }

        public IEnumerable<NSItemSubCategory> Categories { get; set; }
    }
}
