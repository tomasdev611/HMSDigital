using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.EHRIntegration.HCHB.MLLPServer.Contracts
{
    public interface IAzureServiceBusClient
    {
        Task AddMessageBatch<T>(IEnumerable<T> messages) where T : class;
    }
}
