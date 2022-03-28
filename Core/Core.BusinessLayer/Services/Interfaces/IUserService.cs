using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetMyUser();

        Task<User> UpdateMyUser(UserMinimal userMinimalRequest);

        Task<UserProfilePicture> UpdateProfilePicture(int userId, UserPictureFileRequest userProfilePictureRequest);

        Task<UserProfilePicture> ConfirmProfilePictureUpload(int userId);

        Task<UserProfilePicture> GetProfilePicture(int userId);

        Task RemoveProfilePicture(int userId);

        Task<User> GetUserById(int userId);

        Task<PaginatedList<User>> GetAllUsers(SieveModel sieveModel);

        Task<PaginatedList<User>> SearchUsers(string searchQuery, SieveModel sieveModel);

        Task<User> CreateUser(UserMinimal userCreateRequest);

        Task<IEnumerable<User>> CreateBulkUser(IEnumerable<UserCreateRequest> userCreateRequests);

        Task<User> EnableUser(int userId);

        Task<User> DisableUser(int userId);

        Task<User> UpdateUser(int userId, UserMinimal usersRequest);

        Task<IEnumerable<UserRole>> GetUserRoles(int userId);

        Task SetUserPassword(int userId, UserPasswordRequest userPasswordRequest);

        Task ChangeUserPassword(DTOs.ChangePasswordRequest userPasswordRequest);

        Task<IEnumerable<Site>> GetUserSites(int userId);

        Task SendPasswordResetLink(int userId, NotificationBase resetPasswordNotification);

        Task SendConfirmationCode(int userId, string email, long phoneNumber, int countryCode, string verifyAttribute);

        Task<User> VerifyCode(int userId, string email, long phoneNumber, VerifyCodeRequest verifyCodeRequest);

        Task<SignInResponse> SignIn(LoginRequest loginRequest);

        Task<IEnumerable<UserRole>> GetLoggedInUserRoles();

        Task SendOtp(string email);

        Task ResetUserPassword(ResetPasswordRequest resetPasswordRequest);
    }
}
