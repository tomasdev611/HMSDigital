using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Common.BusinessLayer.Enums;
using HMSDigital.Common.BusinessLayer.Extensions;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services
{
    public class FHIRQueueService<T> : IFHIRQueueService<T>
    {
        private readonly FHIRConfig _fhirConfig;

        private readonly ILogger<FHIRQueueService<T>> _logger;

        public FHIRQueueService(IOptions<FHIRConfig> fhirConfigOptions, ILogger<FHIRQueueService<T>> logger)
        {
            _fhirConfig = fhirConfigOptions.Value;
            _logger = logger;
        }

        public async Task QueueCreateRequest(T resource)
        {
            var request = GetQueueRequest(resource, FHIRQueueRequestTypes.Create);
            await QueueRequest(request);
        }

        public async Task QueueUpdateRequest(T resource)
        {
            var request = GetQueueRequest(resource, FHIRQueueRequestTypes.Update);
            await QueueRequest(request);
        }

        public async Task QueueCreateRequestList(IEnumerable<T> resources)
        {
            var requests = resources.Select(x => GetQueueRequest(x, FHIRQueueRequestTypes.Create)).ToList();
            await QueueRequestList(requests);
        }

        public async Task QueueUpdateRequestList(IEnumerable<T> resources)
        {
            var requests = resources.Select(x => GetQueueRequest(x, FHIRQueueRequestTypes.Update)).ToList();
            await QueueRequestList(requests);
        }

        public async Task QueueRequest(FHIRQueueRequest<T> request)
        {
            try
            {
                var queueClient = GetQueueClient();
                await queueClient.SendAsync(GetQueueMessage(request));
                await queueClient.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while inserting new message on Service Bus Queue:{ex.Message}");
                throw;
            }
        }

        public async Task QueueRequestList(IEnumerable<FHIRQueueRequest<T>> queueRequests)
        {
            try
            {
                var queueMessages = queueRequests.Select(x => GetQueueMessage(x)).ToList();
                var messageChunks = queueMessages.Split(100);

                var queueClient = GetQueueClient();
                var serviceBusTasksQuery = messageChunks.Select(chunk => queueClient.SendAsync(chunk.ToList()));
                var serviceBusTasks = serviceBusTasksQuery.ToList();
                await Task.WhenAll(serviceBusTasks);
                await queueClient.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while inserting new message on Service Bus Queue:{ex.Message}");
                throw;
            }
        }

        private IQueueClient GetQueueClient()
        {
            var sbConnectionBuilder = new ServiceBusConnectionStringBuilder(_fhirConfig.QueueConnectionString);
            return new QueueClient(sbConnectionBuilder);
        }

        private FHIRQueueRequest<T> GetQueueRequest(T resource, FHIRQueueRequestTypes requestType)
        {
            var request = new FHIRQueueRequest<T>();
            request.RequestType = (int)requestType;
            request.Resource = resource;
            return request;
        }

        private Message GetQueueMessage(FHIRQueueRequest<T> queueRequest)
        {
            var serializedRequest = JsonConvert.SerializeObject(queueRequest);
            return new Message(Encoding.UTF8.GetBytes(serializedRequest));
        }
    }
}
