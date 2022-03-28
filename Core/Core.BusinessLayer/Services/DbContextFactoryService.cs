using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Config;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class DbContextFactoryService : IDbContextFactoryService
    {
        private readonly DBContextConfig _dbContextConfig;

        public DbContextFactoryService(IOptions<DBContextConfig> dbContextOptions)
        {
            _dbContextConfig = dbContextOptions.Value;
        }

        public async Task<Data.HMSDigitalAuditContext> GetDBContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<Data.HMSDigitalAuditContext>();
            optionsBuilder.UseSqlServer(_dbContextConfig.HMSDigitalDbConnection);

            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.Items.Add(Claims.IGNORE_GLOBAL_FILTER, GlobalFilters.All);

            return new Data.HMSDigitalAuditContext(optionsBuilder.Options, httpContextAccessor);
        }
    }
}
