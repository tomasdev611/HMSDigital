using System.Collections.Generic;
using System.Linq;
using System;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;

namespace HMSDigital.Common.BusinessLayer.Services
{
    public class PaginationService : IPaginationService
    {
        public PaginationService()
        {
        }
        public PaginatedList<T> GetPaginatedList<T>(IEnumerable<T> records, long totalRecordCount, int? page, int? pageSize)
        {
            var recordCount = records.Count();
            var totalPageCount = recordCount > 0 ? Math.Ceiling((double)totalRecordCount / (pageSize ?? recordCount)) : 0;
            return new PaginatedList<T>()
            {
                Records = records,
                TotalRecordCount = totalRecordCount,
                PageNumber = recordCount > 0 ? page ?? 1 : page ?? 0,
                PageSize = pageSize ?? recordCount,
                TotalPageCount = (long)totalPageCount
            };
        }
       
    }
}
