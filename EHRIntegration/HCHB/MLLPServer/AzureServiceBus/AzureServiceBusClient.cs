using Azure.Messaging.ServiceBus;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.AzureServiceBus.Config;
using HMSDigital.EHRIntegration.HCHB.MLLPServer.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.EHRIntegration.HCHB.MLLPServer.AzureServiceBus
{    
    public class AzureServiceBusClient : IAzureServiceBusClient
    {
        private AzureServiceBusConfig _asbConfig { get; set; }
        private ServiceBusClient _asbClient { get; set; }
        private ServiceBusSender _asbSender { get; set; }
        private ILogger<AzureServiceBusClient> _logger { get; set; }

        public AzureServiceBusClient(IOptions<AzureServiceBusConfig> asbConfig, ILogger<AzureServiceBusClient> logger) 
        {
            _asbConfig = asbConfig.Value;
            _logger = logger;
        }        

        public async Task AddMessageBatch<T>(IEnumerable<T> messages) where T : class
        {
            try
            {
                CreateClient();
                CreateSender();

                // create a batch 
                using ServiceBusMessageBatch messageBatch = await _asbSender.CreateMessageBatchAsync();

                foreach (var message in messages)
                {
                    EventId eventId = new EventId();

                    var sbMessage = new ServiceBusMessage(JsonConvert.SerializeObject(message));

                    sbMessage.ContentType = "application/json";

                    if (!messageBatch.TryAddMessage(sbMessage))
                    {
                        // if it is too large for the batch
                       _logger.LogError(eventId, "The message is too large to fit in the batch.", message);
                    }
                }

                // Use the producer client to send the batch of messages to the Service Bus queue
                await _asbSender.SendMessagesAsync(messageBatch);
            }
            catch (Exception e) 
            {                                
                _logger.LogError(new EventId(), e.Message, e.InnerException);
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await _asbClient.DisposeAsync();
                await _asbSender.DisposeAsync();
            }
        }

        private void CreateClient()
        {
            _asbClient = new ServiceBusClient(_asbConfig.ConnectionString);
        }

        private void CreateSender()
        {
            _asbSender = _asbClient.CreateSender(_asbConfig.QueueName);
        }
    }
}
