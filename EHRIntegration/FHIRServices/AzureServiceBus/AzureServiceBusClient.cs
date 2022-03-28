using Azure.Messaging.ServiceBus;
using HMSDigital.EHRIntegration.FHIRServices.AzureServiceBus.Contracts;
using HMSDigital.EHRIntegration.FHIRServices.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.EHRIntegration.FHIRServices.AzureServiceBus
{    
    public class AzureServiceBusClient : IAzureServiceBusClient
    {
        private AzureServiceBusConfig _asbConfig { get; set; }
        private ServiceBusClient _asbClient { get; set; }
        private ServiceBusSender _asbSender { get; set; }

        public AzureServiceBusClient(IOptions<AzureServiceBusConfig> asbConfig) 
        {
            _asbConfig = asbConfig.Value;                        
        }

        private void CreateClient() 
        {
            if (_asbClient == null || _asbClient.IsClosed)
                _asbClient = new ServiceBusClient(_asbConfig.ConnectionString);
        }

        public void CreateSender(string queueName) 
        {            
            CreateClient();
            
            _asbSender = _asbClient.CreateSender(queueName);
        }

        public async Task AddMessageBatch<T>(IEnumerable<T> messages) where T : class
        {
            try
            {                

                // create a batch 
                using ServiceBusMessageBatch messageBatch = await _asbSender.CreateMessageBatchAsync();

                foreach (var m in messages)
                {
                    var sbMessage = new ServiceBusMessage(JsonConvert.SerializeObject(m));

                    sbMessage.ContentType = "application/json";

                    if (!messageBatch.TryAddMessage(sbMessage))
                    {
                        // if it is too large for the batch
                        throw new Exception($"The message {m} is too large to fit in the batch.");
                    }
                }

                // Use the producer client to send the batch of messages to the Service Bus queue
                await _asbSender.SendMessagesAsync(messageBatch);
            }
            catch (Exception e) 
            { 
                
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await _asbClient.DisposeAsync();
                await _asbSender.DisposeAsync();
            }
        }
    }
}
