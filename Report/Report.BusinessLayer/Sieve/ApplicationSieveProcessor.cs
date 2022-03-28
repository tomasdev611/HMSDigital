using HMSDigital.Report.Data.Models;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace HMSDigital.Report.BusinessLayer.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods sieveCustomFilterMethods) : base(options, customSortMethods, sieveCustomFilterMethods)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<OrdersMetric>(p => p.CreatedDate).CanFilter().CanSort();
            mapper.Property<OrdersMetric>(p => p.OrderTypeId).CanFilter();
            mapper.Property<OrdersMetric>(p => p.StatusId).CanFilter();

            return mapper;
        }
    }
}
