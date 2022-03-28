using HMSOnlineSDK.Data.Models;
using HMSOnlineSDK.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSOnlineSDK.Data.Repositories
{
    public class SitesRepository : RepositoryBase<Sites>, ISitesRepository
    {
        public SitesRepository(HMSOnlineContext dbContext) : base(dbContext)
        {
        }
    }
}
