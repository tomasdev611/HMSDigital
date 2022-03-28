using HMSDigital.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories.Interfaces
{
    public interface IUsersRepository : IRepository<Users>
    {
        Task<Users> GetByCognitoUserId(string cognitoUserId);

        Task<IEnumerable<string>> GetSiteAccessByUserId(int userId);

        Task<IEnumerable<string>> GetHospiceAccessByUserId(int userId);

        Task<IEnumerable<string>> GetHospiceLocationAccessByUserId(int userId);
    }
}
