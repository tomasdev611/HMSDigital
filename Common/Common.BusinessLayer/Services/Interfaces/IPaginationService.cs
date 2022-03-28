using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Common.BusinessLayer.Services.Interfaces
{
    public interface IPaginationService
    {
        PaginatedList<T> GetPaginatedList<T>(IEnumerable<T> records, long totalRecordCount, int? page, int? pageSize);
    }
}
