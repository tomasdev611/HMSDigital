using System.Collections.Generic;

namespace HospiceSource.Digital.NetSuite.SDK.ViewModels
{
    public class NetSuiteHMSDispatchResponse : PaginatedBase
    {
        public IEnumerable<NetSuiteHmsDispatch> Results { get; set; }
    }
}
