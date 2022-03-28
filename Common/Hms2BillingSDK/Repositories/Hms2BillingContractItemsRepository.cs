using Hms2BillingSDK.Models;
using Hms2BillingSDK.Repositories.Interfaces;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace Hms2BillingSDK.Repositories
{
    public class Hms2BillingContractItemsRepository : RepositoryBase<TblContractInventory>, IHms2BillingContractItemsRepository
    {
        public Hms2BillingContractItemsRepository(Hms2BillingContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public async Task<IEnumerable<ContractInventory>> GetContractItemsByContract(Expression<Func<ContractInventory, bool>> where)
        {
            var entities = _dbContext.TblContractInventory
                            .Join(_dbContext.TblInventory, 
                                  ci => ci.InventoryId, 
                                  i => i.InventoryId, 
                                  (ci, i) => new ContractInventory 
                                  {
                                      InvctrId = ci.InvctrId,
                                      ContractId = ci.ContractId,
                                      InventoryId = ci.InventoryId,
                                      Perdiem = ci.Perdiem,
                                      RentalPrice = ci.RentalPrice,
                                      SalePrice = ci.SalePrice,
                                      OrderScreen = ci.OrderScreen,
                                      NoApprovalRequired = ci.NoApprovalRequired,
                                      ItemNumber = i.InvCode
                                  })
                            .Where(where);

            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
