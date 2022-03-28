using HMSDigital.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories.Interfaces
{
    public interface IHMS2ContractItemRepository : IRepository<Hms2ContractItems>
    {
        Task<IEnumerable<Hms2ContractItems>> GetContractItemsAsync(Expression<Func<Hms2ContractItems, bool>> where);
    }
}
