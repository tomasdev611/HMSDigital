using HMSDigital.Common.SDK.Config;
using HMSDigital.Common.SDK.Services.Interfaces;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Common.SDK.Services
{
    public class TokenService : ITokenService
    {
        private static readonly IDictionary<string, TokenResponse> _clientTokenDictionary = new Dictionary<string, TokenResponse>();

        private readonly HttpClient _httpClient;

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenByClientCredentials(IdentityClientConfig identityClient)
        {
            var clientTokenKey = $"{identityClient.ClientId}_{identityClient.Scope}";
            if (_clientTokenDictionary.TryGetValue(clientTokenKey, out TokenResponse tokenResponse))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtAccessToken = tokenHandler.ReadJwtToken(tokenResponse.AccessToken);

                // subtract 10 minutes as max clock skew
                if (DateTime.Compare(jwtAccessToken.ValidTo.AddMinutes(-10), DateTime.UtcNow) > 0)
                {
                    return tokenResponse.AccessToken;
                }
            }

            tokenResponse = await GetAccessTokenUsingClientCredentials(identityClient);
            if (tokenResponse.IsError)
            {
                throw new ValidationException($"Authorization Failed.");
            }

            _clientTokenDictionary[clientTokenKey] = tokenResponse;

            return tokenResponse.AccessToken;
        }

        private async Task<TokenResponse> GetAccessTokenUsingClientCredentials(IdentityClientConfig config)
        {
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = config.Address,
                Policy =
                    {
                        ValidateEndpoints = false,
                        ValidateIssuerName = false
                    }
            });

            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = config.ClientId,
                ClientSecret = config.ClientSecret,
            };

            if (!string.IsNullOrEmpty(config.Scope))
            {
                tokenRequest.Scope = config.Scope;
            }

            if (!string.IsNullOrEmpty(config.Resource))
            {
                tokenRequest.Parameters.Add("resource", config.Resource);
            }

            return await _httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
        }
    }
}
