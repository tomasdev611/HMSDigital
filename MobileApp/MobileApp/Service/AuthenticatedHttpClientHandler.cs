using MobileApp.Exceptions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HttpStatusCodes = System.Net.HttpStatusCode;

namespace MobileApp.Service
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly AuthService _authService;

        private AuthenticatedHttpClientHandler()
        {
            _authService = new AuthService();
        }

        private static AuthenticatedHttpClientHandler authenticatedHttpClientHandler;

        public static AuthenticatedHttpClientHandler GetAuthenticatedClientHandler()
        {
            if(authenticatedHttpClientHandler == null)
            {
                authenticatedHttpClientHandler = new AuthenticatedHttpClientHandler();
            }
            return authenticatedHttpClientHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _authService.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return response;
                }
                switch(response.StatusCode)
                {
                    case HttpStatusCodes.BadRequest:
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        throw new BadRequestException(errorMessage);
                    case HttpStatusCodes.Forbidden:
                        throw new ForbiddenException($"You do not have access to this resource");
                    case HttpStatusCodes.Unauthorized:
                        throw new UnauthorizedAccessException($"Please login to access this resource");
                    default:
                        throw new Exception($"Unable to complete the request at the moment");
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
