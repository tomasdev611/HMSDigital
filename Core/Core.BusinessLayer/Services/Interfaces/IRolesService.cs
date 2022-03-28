using System.Collections.Generic;
using System.Threading.Tasks;
using HMSDigital.Core.ViewModels;
using Sieve.Models;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IRolesService
    {
        Task<IEnumerable<Role>> GetAllRoles(SieveModel sieveModel);

        Task<Role> GetRoleById(int id);

        Task<IEnumerable<UserRole>> AddUserRole(int userId, UserRoleBase userRoleRequest);

        Task<IEnumerable<UserRole>> RemoveUserRole(int userId, int userRoleId);

        Task<IEnumerable<UserRole>> RemoveUserRoles(int userId, string resourceType, int resourceId);

    }
}
