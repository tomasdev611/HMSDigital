using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories
{
    public class ItemImageRepository : RepositoryBase<ItemImages>, IItemImageRepository
    {
        public ItemImageRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        { 
        }
    }
}
