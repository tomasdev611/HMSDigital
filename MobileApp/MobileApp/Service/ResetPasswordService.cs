using System;
using System.Threading.Tasks;
using MobileApp.Assets;
using MobileApp.Exceptions;
using MobileApp.Interface;
using MobileApp.Models;
using Refit;

namespace MobileApp.Service
{
    public class ResetPasswordService
    {
        private readonly IUserApi _userApi;

        public ResetPasswordService()
        {
            _userApi = RestService.For<IUserApi>(AppConfiguration.BaseUrl);
        }

        public async Task<bool> SendOtpRequestAsync(string email)
        {
            try
            {
                var response = await _userApi.RequestOtpAsync(
                    new OtpRequest
                    {
                        Email = email
                    });
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPassword resetPassword)
        {
            try
            {
                var response = await _userApi.ResetPassowrdAsync(resetPassword);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }

    }
}
