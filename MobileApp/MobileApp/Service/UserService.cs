using System.Threading.Tasks;
using MobileApp.Interface;
using MobileApp.Models;
using Refit;

namespace MobileApp.Service
{
    public class UserService
    {
        private readonly IUserApi _userApi;

        public UserService()
        {
            _userApi = RestService.For<IUserApi>(HMSHttpClientFactory.GetCoreHttpClient());
        }

        public async Task<User> GetUserDetailsAsync()
        {
            try
            {
                var response = await _userApi.GetMyUserDetailsAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content;
            }
            catch
            {
                throw;
            }
        }
    }
}
