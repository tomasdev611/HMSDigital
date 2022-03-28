using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.ViewModels
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Records { get; set; }

        public long TotalRecordCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public long TotalPageCount { get; set; }
    }
}
