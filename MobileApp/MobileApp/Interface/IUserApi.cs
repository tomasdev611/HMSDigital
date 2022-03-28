using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Models;
using Refit;

namespace MobileApp.Interface
{
    public interface IUserApi
    {
        [Get("/api/users/me")]
        Task<ApiResponse<User>> GetMyUserDetailsAsync();

        [Get("/api/enumerations")]
        Task<ApiResponse<Dictionary<string, IEnumerable<LookUp>>>> GetEnumerationAsync();

        [Post("/api/users/me/send-otp")]
        Task<ApiResponse<string>> RequestOtpAsync([Body] OtpRequest otpRequest);

        [Post("/api/users/me/reset-password")]
        Task<ApiResponse<string>> ResetPassowrdAsync([Body] ResetPassword resetPassword);
    }
}
