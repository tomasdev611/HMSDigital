using Hms2BillingSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hms2BillingSDK.Repositories.Interfaces
{
    public interface IHms2BillingContractItemsRepository : IRepository<TblContractInventory>
    {
        Task<IEnumerable<ContractInventory>> GetContractItemsByContract(Expression<Func<ContractInventory, bool>> where);
    }
}
