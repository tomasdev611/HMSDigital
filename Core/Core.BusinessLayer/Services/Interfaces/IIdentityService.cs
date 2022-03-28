using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<User> GetUser(string userId);

        Task<User> CreateUser(UserMinimal userCreateRequest);

        Task<User> UpdateUser(string userId, UserMinimal usersUpdateRequest);

        Task SetUserPassword(string userId, UserPasswordRequest userPasswordRequest);

        Task ChangeUserPassword(DTOs.ChangePasswordRequest changePasswordRequest);

        Task<User> EnableUser(string userId);

        Task<User> DisableUser(string userId);

        Task<string> GetPasswordResetLink();

        Task<User> VerifyUserAttribute(string userId, string attribute);

        Task<SignInResponse> SignIn(string username, string password);

        Task<SignInResponse> RefreshToken(string refreshToken);
    }
}
