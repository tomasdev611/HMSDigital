using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using CognitoIdentity = Amazon.CognitoIdentity.Model;
using AutoMapper;
using HMSDigital.Common.BusinessLayer;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HMSDigital.Common.BusinessLayer.Constants;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class AWSCognitoService : IIdentityService
    {
        private readonly AWSConfig _awsConfig;

        private readonly IMapper _mapper;

        private readonly ILogger<AWSCognitoService> _logger;

        public AWSCognitoService(IMapper mapper,
            IOptions<AWSConfig> awsConfigOptions,
             ILogger<AWSCognitoService> logger)
        {
            _mapper = mapper;
            _awsConfig = awsConfigOptions.Value;
            _logger = logger;
        }

        public async Task<User> CreateUser(UserMinimal userCreateRequest)
        {
            try
            {
                var getUserResponse = await GetUser(userCreateRequest.Email);
                if (getUserResponse != null)
                {
                    return getUserResponse;
                }

                var client = GetCognitoClient();

                var userAttributes = GetAttributesForUser(userCreateRequest).ToList();
                if (!string.IsNullOrEmpty(userCreateRequest.Email))
                {
                    userAttributes.Add(new AttributeType() { Name = "email_verified", Value = "false" });
                }
                if (userCreateRequest.PhoneNumber != 0)
                {
                    userAttributes.Add(new AttributeType() { Name = "phone_number_verified", Value = "false" });
                }

                var cognitoUserCreateRequest = new AdminCreateUserRequest()
                {
                    Username = userCreateRequest.Email,
                    UserPoolId = _awsConfig.UserPoolId,
                    UserAttributes = userAttributes,
                    DesiredDeliveryMediums = new List<string>() { DeliveryMediumType.EMAIL }
                };

                var cognitoUserResponse = await client.AdminCreateUserAsync(cognitoUserCreateRequest);

                var userResponse = _mapper.Map<User>(cognitoUserResponse.User);
                return userResponse;
            }
            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to add AWS Cognito user. {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to add AWS Cognito user. {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUser(string userId)
        {
            try
            {
                var client = GetCognitoClient();
                var result = await GetCognitoUser(client, userId);
                return _mapper.Map<ViewModels.User>(result);
            }
            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to get AWS Cognito user with userId ({userId}). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get AWS Cognito user with userId ({userId}). {ex.Message}");
                throw;
            }
        }

        public async Task<User> EnableUser(string userId)
        {
            var client = GetCognitoClient();
            var request = new AdminEnableUserRequest()
            {
                Username = userId,
                UserPoolId = _awsConfig.UserPoolId
            };
            try
            {
                await client.AdminEnableUserAsync(request);
                return await GetUser(userId);
            }
            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to enable AWS Cognito user with userId ({userId}). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to enable AWS Cognito user with userId ({userId}). {ex.Message}");
                throw;
            }

        }

        public async Task<User> DisableUser(string userId)
        {
            var client = GetCognitoClient();
            var request = new AdminDisableUserRequest()
            {
                Username = userId,
                UserPoolId = _awsConfig.UserPoolId
            };
            var signOutRequest = new AdminUserGlobalSignOutRequest()
            {
                Username = userId,
                UserPoolId = _awsConfig.UserPoolId
            };
            try
            {
                await client.AdminDisableUserAsync(request);
                await client.AdminUserGlobalSignOutAsync(signOutRequest);
                return await GetUser(userId);
            }
            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to disable AWS Cognito user with userId ({userId}). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to disable AWS Cognito user with userId ({userId}). {ex.Message}");
                throw;
            }
        }

        public async Task<User> UpdateUser(string userId, UserMinimal usersUpdateRequest)
        {
            var client = GetCognitoClient();

            var userAttributes = GetAttributesForUser(usersUpdateRequest).ToList();

            var user = await GetUser(userId);
            if (!usersUpdateRequest.Email.Equals(user.Email) && !string.IsNullOrEmpty(usersUpdateRequest.Email))
            {
                var existingUser = await GetUser(usersUpdateRequest.Email);
                if (existingUser != null)
                {
                    throw new ValidationException($"Another user with email - {usersUpdateRequest.Email} already exists");
                }
                userAttributes.Add(new AttributeType() { Name = "email_verified", Value = "false" });
            }
            if (usersUpdateRequest.PhoneNumber != user.PhoneNumber && usersUpdateRequest.PhoneNumber != 0)
            {
                userAttributes.Add(new AttributeType() { Name = "phone_number_verified", Value = "false" });
            }

            var updateRequest = new AdminUpdateUserAttributesRequest()
            {
                Username = userId,
                UserPoolId = _awsConfig.UserPoolId,
                UserAttributes = userAttributes
            };
            try
            {
                await client.AdminUpdateUserAttributesAsync(updateRequest);
                return await GetUser(userId);
            }

            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to update AWS Cognito user with userId ({userId}). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to update AWS Cognito user with userId ({userId}). {ex.Message}");
                throw;
            }
        }

        public async Task SetUserPassword(string userId, ViewModels.UserPasswordRequest userPasswordRequest)
        {
            var client = GetCognitoClient();
            var request = new AdminSetUserPasswordRequest()
            {
                Username = userId,
                UserPoolId = _awsConfig.UserPoolId,
                Password = userPasswordRequest.Password,
                Permanent = userPasswordRequest.Permanent
            };
            try
            {
                await client.AdminSetUserPasswordAsync(request);
            }
            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to set AWS Cognito user password with userId ({userId}). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to set AWS Cognito user password with userId ({userId}). {ex.Message}");
                throw;
            }
        }

        public async Task ChangeUserPassword(DTOs.ChangePasswordRequest changePasswordRequest)
        {
            var client = GetCognitoClient();
            var request = _mapper.Map<Amazon.CognitoIdentityProvider.Model.ChangePasswordRequest>(changePasswordRequest);
            try
            {
                await client.ChangePasswordAsync(request);
            }
            catch(InvalidPasswordException ipx)
            {
                _logger.LogInformation($"Failed to set AWS Cognito user password. {ipx.Message}");
                throw new ValidationException($"Password did not conform with policy");
            }
            catch(InvalidParameterException ipax)
            {
                _logger.LogInformation($"Failed to set AWS Cognito user password. {ipax.Message}");
                throw new ValidationException($"Provided password(s) failed to satisfy required constraints");
            }
            catch (NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to set AWS Cognito user password. {nx.Message}");
                throw new ValidationException($"Invalid username or password");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to set AWS Cognito user password. {ex.Message}");
                throw;
            }
        }

        private async Task<AdminGetUserResponse> GetCognitoUser(AmazonCognitoIdentityProviderClient client, string userId)
        {
            var request = new AdminGetUserRequest()
            {
                Username = userId,
                UserPoolId = _awsConfig.UserPoolId
            };
            AdminGetUserResponse result;
            try
            {
                result = await client.AdminGetUserAsync(request);
            }
            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to get AWS Cognito user with userId ({userId}). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get AWS Cognito user with userId ({userId}). {ex.Message}");
                return null;
            }
            return result;
        }

        private AmazonCognitoIdentityProviderClient GetCognitoClient()
        {
            try
            {
                return new AmazonCognitoIdentityProviderClient(_awsConfig.AccessKey, _awsConfig.SecretKey, RegionEndpoint.GetBySystemName(_awsConfig.Region));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while getting cognito client: {ex.Message}");
                throw ex;
            }
        }

        private string GetUserListFilterString(string sieveFilterString)
        {
            if (sieveFilterString != null)
            {
                if (sieveFilterString.Contains("_="))
                {
                    sieveFilterString = sieveFilterString.Replace("_=", "^=");
                }
                else
                {
                    sieveFilterString.Replace("==", "=");
                }
                var count = sieveFilterString.IndexOf('=');
                var value = sieveFilterString.Substring(count + 1);
                sieveFilterString = sieveFilterString.Replace(value, $"\"{value}\"");
                return sieveFilterString;
            }
            return null;
        }

        private IEnumerable<AttributeType> GetAttributesForUser(UserMinimal user)
        {
            var attributeList = new List<AttributeType>() {
                            new AttributeType() { Name = "email", Value = user.Email },
                            new AttributeType() { Name = "given_name", Value = user.FirstName },
                            new AttributeType() { Name = "family_name", Value = user.LastName },
                            new AttributeType() { Name = "name", Value = $"{user.FirstName} {user.LastName}" }
                        };
            if (user.PhoneNumber != 0)
            {
                if (user.CountryCode == 0)
                {
                    user.CountryCode = 1;
                }
                attributeList.Add(new AttributeType() { Name = "phone_number", Value = $"+{user.CountryCode}{user.PhoneNumber}" });
            }
            return attributeList;
        }

        public async Task<string> GetPasswordResetLink()
        {
            var client = GetCognitoClient();
            var request = new DescribeUserPoolRequest()
            {
                UserPoolId = _awsConfig.UserPoolId
            };
            try
            {
                var response = await client.DescribeUserPoolAsync(request);
                var redirectUrl = HttpUtility.UrlEncode($"{_awsConfig.RedirectUri}/login");

                var link = $"https://{response.UserPool.Domain}.auth.{_awsConfig.Region}.amazoncognito.com/forgotPassword?response_type={_awsConfig.ResponseType}" +
                           $"&client_id={_awsConfig.UserPoolClientId}&redirect_uri={redirectUrl}";
                return link;
            }
            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to get AWS Cognito user pool). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to get AWS Cognito user pool). {ex.Message}");
                throw;
            }

        }

        public async Task<User> VerifyUserAttribute(string userId, string attribute)
        {
            try
            {
                var client = GetCognitoClient();

                var userAttributes = new List<AttributeType>();

                if (attribute.Equals(VerifyAttributes.EMAIL_VERIFY))
                {
                    userAttributes.Add(new AttributeType() { Name = "email_verified", Value = "true" });
                }
                if (attribute.Equals(VerifyAttributes.PHONE_NUMBER_VERIFY))
                {
                    userAttributes.Add(new AttributeType() { Name = "phone_number_verified", Value = "true" });
                }
                var updateRequest = new AdminUpdateUserAttributesRequest()
                {
                    Username = userId,
                    UserPoolId = _awsConfig.UserPoolId,
                    UserAttributes = userAttributes
                };
                await client.AdminUpdateUserAttributesAsync(updateRequest);
                return await GetUser(userId);
            }

            catch (CognitoIdentity.NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to verify AWS Cognito user attribute). {nx.Message}");
                throw new ValidationException($"Id token is either invalid or expired.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to verify AWS Cognito user attribute). {ex.Message}");
                throw;
            }
        }

        public async Task<SignInResponse> SignIn(string username, string password)
        {
            var client = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(),
                                                RegionEndpoint.GetBySystemName(_awsConfig.Region));
            var userPool = new CognitoUserPool(_awsConfig.UserPoolId, _awsConfig.UserPoolClientId, client);
            var cognitoUser = new CognitoUser(username, _awsConfig.UserPoolClientId, userPool, client);
            var authRequest = new InitiateSrpAuthRequest()
            {
                Password = password
            };
            try
            {
                var authFlowResponse = await cognitoUser.StartWithSrpAuthAsync(authRequest);
                return _mapper.Map<SignInResponse>(authFlowResponse);
            }
            catch (UserNotFoundException ux)
            {
                _logger.LogInformation($"Failed to SignUp AWS Cognito user). {ux.Message}");
                throw new ValidationException("Incorrect username or password.");
            }
            catch (NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to SignUp AWS Cognito user). {nx.Message}");
                throw new ValidationException(nx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to SignUp AWS Cognito user). {ex.Message}");
                throw;
            }
        }

        public async Task<SignInResponse> RefreshToken(string refreshToken)
        {
            var client = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(),
                                                RegionEndpoint.GetBySystemName(_awsConfig.Region));

            var initiateAuthRequest = new InitiateAuthRequest()
            {
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                ClientId = _awsConfig.UserPoolClientId
            };
            initiateAuthRequest.AuthParameters.Add("REFRESH_TOKEN", refreshToken);
            try
            {
                var initiateAuthResponse = await client.InitiateAuthAsync(initiateAuthRequest);
                var signInResponse = _mapper.Map<SignInResponse>(initiateAuthResponse);
                signInResponse.RefreshToken = refreshToken;
                return signInResponse;
            }
            catch (NotAuthorizedException nx)
            {
                _logger.LogInformation($"Failed to Refresh AWS Cognito user token). {nx.Message}");
                throw new ValidationException(nx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to Refresh AWS Cognito user token). {ex.Message}");
                throw;
            }
        }
    }
}
