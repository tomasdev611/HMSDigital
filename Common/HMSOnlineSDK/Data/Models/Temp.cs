using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class Temp
    {
        public string InvCode { get; set; }
        public string Description { get; set; }
        public string AssetOrQuantity { get; set; }
        public string SerialNumRequired { get; set; }
        public string LotNumRequired { get; set; }
        public string Disposable { get; set; }
        public string DescriptionProper { get; set; }
        public int QuantityTracked { get; set; }
        public int SerialNumBool { get; set; }
        public int LotNumBool { get; set; }
        public int DisposableBool { get; set; }
    }
}
