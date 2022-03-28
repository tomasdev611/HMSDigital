using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels.NetSuite
{
    public class NSItemSubCategory
    {
        public string Name { get; set; }

        [JsonProperty("internalCategoryId")]
        public int NetSuiteSubCategoryId { get; set; }
    }
}
