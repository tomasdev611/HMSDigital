using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Report.BusinessLayer.Interfaces;
using HMSDigital.Report.Data.Models;
using HMSDigital.Report.Data.Repositories.Interfaces;
using HMSDigital.Report.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Report.BusinessLayer.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IPatientsRepository _patientsRepository;
        private readonly HttpContext _httpContext;

        public ReportService(ILogger<ReportService> logger,
                            IOrdersRepository ordersRepository,
                            IUsersRepository usersRepository,
                            IPatientsRepository patientsRepository,
                            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _ordersRepository = ordersRepository;
            _usersRepository = usersRepository;
            _patientsRepository = patientsRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ReportListResponse> GetOrdersMetric(SieveModel sieveModel, string groupedBy, bool ignoreFilter = false)
        {
            _ordersRepository.SieveModel = sieveModel;

            var predicate = await GetOrdersPredicate(ignoreFilter);
            var reportData = await _ordersRepository.GetAllAsync(predicate);
            var groupType = groupedBy.Substring(groupedBy.IndexOf('=') + 1, groupedBy.Length - groupedBy.IndexOf('=') - 1);

            var groupedData = reportData.GroupBy(GetGroupKey(groupType))
                .Select(q => new ReportData
                {
                    Label = GetPropValue(q.First(), groupType),
                    Value = q.Select(x => x.OrderHeaderId).Distinct().Count()
                }).ToList();

            return new ReportListResponse
            {
                Data = groupedData.OrderByDescending(x => x.Value).ToList(),
                TotalCount = reportData.Count(),
                TableHeader = new List<ReportTableHeader>()
                {
                    new ReportTableHeader { Label = "N°", Field = "id" },
                    new ReportTableHeader { Label = groupType, Field = "label" },
                    new ReportTableHeader { Label = "Orders", Field = "value" },
                }
            };
        }

        public async Task<ReportValueResponse> GetPatientsMetric(string filters, bool ignoreFilter = false)
        {
            var reportData = await _patientsRepository.GetPatientsByStatus(filters);

            return new ReportValueResponse
            {
                Value = reportData,
                Type = null
            };
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return -1;
            }
            return int.Parse(userIdClaim.Value);
        }

        private async Task<ExpressionStarter<OrdersMetric>> GetOrdersPredicate(bool ignoreFilter = false)
        {
            var predicate = PredicateBuilder.New<OrdersMetric>(false);

            if (ignoreFilter)
            {
                return predicate.Or(o => true);
            }

            var userId = GetLoggedInUserId();

            var siteIds = await _usersRepository.GetSiteAccessByUserId(userId);
            if (siteIds.Contains("*"))
            {
                return predicate.Or(o => true);
            }
            predicate = predicate.Or(o => siteIds.Select(int.Parse).ToList().Contains(o.SiteId));

            var hospiceIds = await _usersRepository.GetHospiceAccessByUserId(userId);
            if (hospiceIds.Contains("*"))
            {
                return predicate;
            }
            predicate = predicate.Or(o => hospiceIds.Select(int.Parse).Contains(o.HospiceId));

            var hospiceLocationIds = await _usersRepository.GetHospiceLocationAccessByUserId(userId);
            if (hospiceLocationIds.Contains("*"))
            {
                return predicate;
            }
            predicate = predicate.And(o => hospiceLocationIds.Select(int.Parse).Contains(o.HospiceLocationId));

            return predicate;
        }

        private static string GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null).ToString();
        }

        private static Func<OrdersMetric, string> GetGroupKey(string property)
        {
            var parameter = Expression.Parameter(typeof(OrdersMetric));
            var body = Expression.Property(parameter, property);
            return Expression.Lambda<Func<OrdersMetric, string>>(body, parameter).Compile();
        }
    }
}
