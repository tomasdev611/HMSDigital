using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services.Interfaces
{
    public interface IPermissionsService
    {
        Task<IEnumerable<string>> GetAllPermissions();

        Task<IEnumerable<string>> GetAllPermissionVerbs();

    }
}
