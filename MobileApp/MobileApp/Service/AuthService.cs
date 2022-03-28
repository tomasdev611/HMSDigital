using System.Threading.Tasks;
using MobileApp.Assets.Constants;
using MobileApp.Assets;
using MobileApp.Interface;
using MobileApp.Models;
using Refit;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace MobileApp.Service
{
    public class AuthService
    {
        private IAuthApi _authApi;

        private StorageService _storageService;

        public AuthService()
        {
            _storageService = new StorageService();
            _authApi = RestService.For<IAuthApi>(AppConfiguration.BaseUrl);
        }

        public async Task<string> LoginAsync(UserLogin User)
        {
            try
            {
                var response = await _authApi.RequestLoginAsync(User);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                else
                {
                    _storageService.AddToStorage(StorageConstants.AccessToken, response.Content.AccessToken, true);
                    _storageService.AddToStorage(StorageConstants.RefreshToken, response.Content.RefreshToken, true);
                    _storageService.AddToStorage(StorageConstants.IdToken, response.Content.IdToken, true);
                    return AppConstants.SUCCESS;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var accessToken = _storageService.GetFromStorage(StorageConstants.AccessToken, true);
                if (accessToken == null)
                {
                    return null;
                }
                else if (IsTokenExpired(accessToken))
                {
                    accessToken = await GetAccessTokenUsingRefreshTokenAsync();
                }
                return accessToken;
            }
            catch
            {
                throw;
            }
        }

        private async Task<string> GetAccessTokenUsingRefreshTokenAsync()
        {
            try
            {
                var refreshToken = _storageService.GetFromStorage(StorageConstants.RefreshToken, true);
                if (refreshToken == null)
                {
                    _storageService.RemoveFromStorage(StorageConstants.RefreshToken, true);
                    return null;
                }
                var userData = new UserLogin
                {
                    GrantType = "refresh_token",
                    RefreshToken = refreshToken,
                };
                var response = await _authApi.RequestLoginAsync(userData);
                if (!response.IsSuccessStatusCode || response.Content == null)
                {
                    return null;
                }
                _storageService.AddToStorage(StorageConstants.AccessToken, response.Content.AccessToken, true);
                _storageService.AddToStorage(StorageConstants.RefreshToken, response.Content.RefreshToken, true);
                return response.Content.AccessToken;
            }
            catch
            {
                throw;
            }
        }

        private bool IsTokenExpired(string accessToken)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var token = jwtHandler.ReadJwtToken(accessToken);
                return DateTime.UtcNow.CompareTo(token.ValidTo) > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            var token = await GetAccessTokenAsync();
            return token != null;
        }
    }
}
