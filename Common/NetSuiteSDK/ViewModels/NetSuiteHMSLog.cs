using System;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteHmsLog
    {
        public int Ns_id { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string LogType { get; set; }

        public string Api { get; set; }

        public string Method { get; set; }

        public string Transaction { get; set; }

        public string Entity { get; set; }

        public string Item { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }
    }
}
