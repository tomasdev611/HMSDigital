using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IUserAccessService
    {
        Task<bool> CanAccessUser(int userId);

        Task<bool> CanAccessHospice(int hospiceId);

        Task<bool> CanAccessSite(int siteId);

        Task<bool> CanAccessHospiceLocation(int hospiceLocationId);

        bool ValidateLoggedInUser(int userId);
    }
}
