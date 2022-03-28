using HMSDigital.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories.Interfaces
{
    public interface IContractRecordsRepository : IRepository<ContractRecords>
    {
        Task<IEnumerable<ContractRecords>> GetContractRecordsAsync(Expression<Func<ContractRecords, bool>> where);
    }
}
