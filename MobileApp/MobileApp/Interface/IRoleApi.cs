using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Models;
using Refit;

namespace MobileApp.Interface
{
    public interface IRoleApi
    {
        [Get("/api/roles")]
        Task<ApiResponse<IEnumerable<Role>>> GetRolePermissions();
    }
}
