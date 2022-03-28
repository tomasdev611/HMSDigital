using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services.Interfaces
{
    public interface IFHIRQueueService<T>
    {
        Task QueueCreateRequest(T resource);
        Task QueueUpdateRequest(T resource);
        Task QueueCreateRequestList(IEnumerable<T> resources);
        Task QueueUpdateRequestList(IEnumerable<T> resources);
    }
}
