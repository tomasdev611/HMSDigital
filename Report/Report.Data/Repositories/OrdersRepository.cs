using HMSDigital.Report.Data.Models;
using HMSDigital.Report.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Report.Data.Repositories
{
    public class OrdersRepository: RepositoryBase<OrdersMetric>, IOrdersRepository
    {        

        public OrdersRepository(HMSReportAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {
        }       
    }
}
