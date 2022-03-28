using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.EHRIntegration.FHIRServices.AzureServiceBus.Contracts
{
    public interface IAzureServiceBusClient
    {
        Task AddMessageBatch<T>(IEnumerable<T> messages) where T : class;
        void CreateSender(string queueName);
    }
}
