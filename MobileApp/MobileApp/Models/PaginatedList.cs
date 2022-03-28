using System;
using System.Collections.Generic;

namespace MobileApp.Models
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
