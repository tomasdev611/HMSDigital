using System.Collections.Generic;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteHmsLogResponse : PaginatedBase
    {
        public IEnumerable<NetSuiteHmsLog> Results { get; set; }
    }
}
