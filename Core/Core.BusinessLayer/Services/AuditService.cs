using Audit.AzureTableStorage.ConfigurationApi;
using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Config;
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class AuditService : IAuditService
    {
        private readonly IMapper _mapper;

        private readonly IPaginationService _paginationService;

        private readonly IDispatchAuditLogRepository _dispatchAuditLogRepository;

        private readonly SystemLogConfig _systemLogConfig;

        private readonly ILogger<AuditService> _logger;

        private readonly IUsersRepository _usersRepository;

        public AuditService(IMapper mapper,
            IOptions<SystemLogConfig> systemLogConfigOptions,
            IPaginationService paginationService,
            IDispatchAuditLogRepository dispatchAuditLogRepository,
            ILogger<AuditService> logger,
            IUsersRepository usersRepository)
        {
            _mapper = mapper;
            _paginationService = paginationService;
            _dispatchAuditLogRepository = dispatchAuditLogRepository;
            _systemLogConfig = systemLogConfigOptions.Value;
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<AzureLogResponse<ViewModels.AuditLog>> GetAuditLogs(APILogRequest apiLogRequest)
        {
            if (!Enum.TryParse(apiLogRequest.APILogType, true, out AuditTypes type))
            {
                throw new ValidationException($"Invalid Audit Log type: ({apiLogRequest.APILogType})");
            }
            var tableClient = GetCloudTableClient();
            var table = tableClient.GetTableReference(type.ToString() + "AuditLog");
            var filter = PrepareAuditLogFilterQuery(apiLogRequest);
            var query = new TableQuery<AuditLogAzureTable>().Where(filter).Take(apiLogRequest.PageSize);

            try
            {
                var auditLogsResponse = await table.ExecuteQuerySegmentedAsync(query, apiLogRequest.ContinuationToken);
                var auditLogs = _mapper.Map<IEnumerable<ViewModels.AuditLog>>(auditLogsResponse.Results);
                var userIds = auditLogs.Select(a => a.UserId);

                var users = await _usersRepository.GetManyAsync(u => userIds.Contains(u.Id));

                foreach (var auditLog in auditLogs)
                {
                    auditLog.User = _mapper.Map<User>(users.FirstOrDefault(u => u.Id == auditLog.UserId));
                }
                return new AzureLogResponse<ViewModels.AuditLog>()
                {
                    APILogs = auditLogs,
                    ContinuationToken = auditLogsResponse.ContinuationToken
                };
            }
            catch (StorageException se)
            {
                if (se.Message.Equals("Not Found"))
                {
                    return new AzureLogResponse<ViewModels.AuditLog>();
                }

                _logger.LogError($"Error Occurred while getting Audit Logs :{se.Message}");
                throw new ValidationException(se.Message);
            }
        }

        public async Task<PaginatedList<ViewModels.AuditLog>> GetAlldispatchUpdateAuditLogs(SieveModel sieveModel)
        {
            _dispatchAuditLogRepository.SieveModel = sieveModel;
            var totalRecords = await _dispatchAuditLogRepository.GetCountAsync(dl => true);
            var auditModels = await _dispatchAuditLogRepository.GetAllAsync();
            var audits = _mapper.Map<IEnumerable<ViewModels.AuditLog>>(auditModels);
            return _paginationService.GetPaginatedList(audits, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<IEnumerable<ViewModels.UserAuditCsvReport>> GetAllUsersAuditAsCsvReport(SieveModel SieveModel)
        {
            var tableClient = GetCloudTableClient();
            CloudTable table = tableClient.GetTableReference("UsersAuditLog");

            var query = new TableQuery<AuditEventTableEntity>();
            var apiLogs = await table.ExecuteQuerySegmentedAsync(query, null);

            var auditLogs = apiLogs.Results.Select(r => JsonConvert.DeserializeObject<ViewModels.UserAuditLog>(r.AuditEvent));

            var UserAuditCsvReportList = new List<ViewModels.UserAuditCsvReport>();
            ViewModels.UserAuditCsvReport userAuditCsvReport;
            foreach (var auditLog in auditLogs)
            {
                if (auditLog.AuditData != null && auditLog.AuditData.Count() > 0)
                {
                    foreach (var audit in auditLog.AuditData)
                    {
                        userAuditCsvReport = _mapper.Map<ViewModels.UserAuditCsvReport>(auditLog);
                        userAuditCsvReport.ColumnName = audit.ColumnName;
                        userAuditCsvReport.OriginalValue = audit.OriginalValue;
                        userAuditCsvReport.NewValue = audit.NewValue;
                        UserAuditCsvReportList.Add(userAuditCsvReport);
                    }
                }
                else
                {
                    userAuditCsvReport = _mapper.Map<ViewModels.UserAuditCsvReport>(auditLog);
                    UserAuditCsvReportList.Add(userAuditCsvReport);
                }
            }
            return UserAuditCsvReportList;
        }

        public async Task AddUsersAudit(ViewModels.AuditLog userAuditLog)
        {
            var tableClient = GetCloudTableClient();
            var table = tableClient.GetTableReference("UsersAuditLog");
            var entity = new Common.ViewModels.AuditLogAzureTable()
            {
                AuditEvent = JsonConvert.SerializeObject(userAuditLog.AuditData),
                RowKey = DateTime.UtcNow.Ticks.ToString(),
                PartitionKey = userAuditLog.EntityId.ToString(),
                AuditAction = userAuditLog.AuditAction,
                UserId = userAuditLog.UserId,
                EntityId = userAuditLog.EntityId,
                AuditDate = userAuditLog.AuditDate,
                ClientIpAddress = userAuditLog.ClientIpaddress
            };
            var insertOperation = TableOperation.InsertOrMerge(entity);
            await table.ExecuteAsync(insertOperation);
        }

        private CloudTableClient GetCloudTableClient()
        {
            var storageAccount = CloudStorageAccount.Parse(_systemLogConfig.ConnectionString);
            return storageAccount.CreateCloudTableClient();
        }

        private string PrepareAuditLogFilterQuery(APILogRequest apiLogRequest)
        {
            var filter = "";

            if (apiLogRequest.FromDate != null && apiLogRequest.ToDate != null
                && apiLogRequest.FromDate != DateTime.MinValue && apiLogRequest.ToDate != DateTime.MinValue)
            {
                if (apiLogRequest.FromDate == apiLogRequest.ToDate)
                {
                    apiLogRequest.ToDate = apiLogRequest.ToDate.AddDays(1);
                }
                var fromDate = TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, apiLogRequest.FromDate);
                var toDate = TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, apiLogRequest.ToDate);
                filter = CombineFilters(fromDate, toDate);
            }

            if (apiLogRequest.EntityId != null)
            {
                var entityIdFilter = TableQuery.GenerateFilterConditionForInt("EntityId", QueryComparisons.Equal, (int)apiLogRequest.EntityId);
                filter = CombineFilters(filter, entityIdFilter);
            }

            if (apiLogRequest.UserId != null)
            {
                var userIdFilter = TableQuery.GenerateFilterConditionForInt("UserId", QueryComparisons.Equal, (int)apiLogRequest.UserId);
                filter = CombineFilters(filter, userIdFilter);
            }

            if (!string.IsNullOrEmpty(apiLogRequest.ActionType))
            {
                var actionTypeFilter = TableQuery.GenerateFilterCondition("AuditAction", QueryComparisons.Equal, apiLogRequest.ActionType);
                filter = CombineFilters(filter, actionTypeFilter);
            }

            return filter;
        }

        private string CombineFilters(string baseFilter, string newFilter, string tableOperator = TableOperators.And)
        {
            if (string.IsNullOrEmpty(baseFilter))
            {
                return newFilter;
            }
            return TableQuery.CombineFilters(baseFilter, tableOperator, newFilter);
        }
    }

}
