using CoreSDK.Interfaces;
using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestEase;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CoreSDK
{
    public class CoreService : ICoreService
    {
        public string AccessToken { get; set; }

        private readonly CoreConfig _coreConfig;

        private readonly ContractConfig _contractConfig;

        private readonly ICoreAPI _coreAPI;

        private readonly ITokenService _tokenService;

        private readonly ILogger<CoreService> _logger;

        public CoreService(IOptions<CoreConfig> coreOptions,
                           IOptions<ContractConfig> contractOptions,
                           ITokenService tokenService,
                           ILogger<CoreService> logger)
        {
            _coreConfig = coreOptions.Value;
            _contractConfig = contractOptions.Value;
            _coreAPI = RestClient.For<ICoreAPI>(_coreConfig.ApiUrl);
            _tokenService = tokenService;
            _logger = logger;
        }

        #region Dispatch

        public async Task UnAssignOrder(int orderId)
        {
            try
            {
                await SetAuthorizationToken();
                await _coreAPI.UnAssignOrder(orderId);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while unassign orders :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while unassign orders :{ex.Message}");
                throw;
            }
        }

        public async Task<PaginatedList<DispatchInstruction>> GetAllDispatchInstructions()
        {
            try
            {
                await SetAuthorizationToken();
                return await _coreAPI.GetAllDispatchInstruction();
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting dispatch instructions :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting dispatch instructions :{ex.Message}");
                throw;
            }
        }

        public async Task<SiteLoadList> GetLoadList(int siteId, SieveModel sieveModel)
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";

                return await _coreAPI.GetLoadList(siteId, sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting loadList :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting loadList :{ex.Message}");
                throw;
            }
        }

        public async Task<DispatchResponse> GetLoggedInDriverDispatch(SieveModel sieveModel)
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";

                return await _coreAPI.GetLoggedInDriverDispatch(sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting inventory :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting inventory :{ex.Message}");
                throw;
            }
        }

        public async Task PickupDispatchRequest(DispatchMovementRequest pickupRequest)
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                await _coreAPI.PickupDispatchRequest(pickupRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while picking up dispatch: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while picking up dispatch: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Auth

        public async Task<SignInResponse> SignIn(LoginRequest loginRequest)
        {
            try
            {
                var signInRequest = new Dictionary<string, string>()
                {
                    { "grant_type", loginRequest.GrantType },
                    { "username", loginRequest.Username },
                    { "password", loginRequest.Password },
                    { "refresh_token", loginRequest.RefreshToken }
                };
                return await _coreAPI.SignIn(signInRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while signin :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while signin :{ex.Message}");
                throw;
            }
        }

        #endregion

        #region Enum

        public async Task<Enumeration> GetAllEnumerations()
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                return await _coreAPI.GetAllEnumerations();
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting enumeration :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting enumeration :{ex.Message}");
                throw;
            }
        }

        #endregion

        #region User

        public async Task<User> GetMyUser()
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                return await _coreAPI.GetMyUser();
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting loggedin user :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting loggedin user :{ex.Message}");
                throw;
            }
        }

        #endregion

        #region Inventory

        public async Task<PaginatedList<Inventory>> GetAllInventory(SieveModel sieveModel)
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";

                return await _coreAPI.GetAllInventory(sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting inventory :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting inventory :{ex.Message}");
                throw;
            }
        }

        #endregion

        #region Site

        public async Task<Site> GetSiteById(int siteId)
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                return await _coreAPI.GetSiteById(siteId);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting site by id: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting site by id:{ex.Message}");
                throw;
            }
        }

        public async Task<PaginatedList<Site>> GetAllSites(SieveModel sieveModel)
        {
            try
            {
                await SetAuthorizationToken();

                return await _coreAPI.GetAllSites(sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting sites :{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting sites :{ex.Message}");
                throw;
            }
        }

        #endregion

        #region OrderLineItemFulfillments

        public async Task<PaginatedList<OrderFulfillmentLineItem>> GetOrderFulfillments(int orderHeaderId, SieveModel sieveModel)
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                return await _coreAPI.GetOrderFulfillments(orderHeaderId, sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting order fulfillments: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting order fulfillments: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Fulfillment

        public async Task FulfillOrder(OrderFulfillmentRequest fulfillmentRequest)
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                await _coreAPI.FulfillOrder(fulfillmentRequest);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while fulfilling order: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while fulfilling order: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region SiteMember

        public async Task<SiteMember> GetMySiteMember()
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                return await _coreAPI.GetMySiteMember();
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting logged in site member: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting logged in site member: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Driver

        public async Task<Driver> GetMyDriver()
        {
            try
            {
                _coreAPI.BearerToken = $"Bearer {AccessToken}";
                return await _coreAPI.GetMyDriver();
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting logged in driver: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting logged in driver: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Hospices


        public async Task<PaginatedList<Hospice>> SearchHospices(string searchQuery)
        {
            try
            {
                await SetAuthorizationToken();
                return await _coreAPI.SearchHospices(searchQuery);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting Hospice for search query ({searchQuery}): {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting Hospice for search query ({searchQuery}):{ex.Message}");
                throw;
            }
        }

        public async Task<PaginatedList<Hospice>> GetAllHospices(SieveModel sieveModel)
        {
            try
            {
                await SetAuthorizationToken();
                return await _coreAPI.GetAllHospices(sieveModel?.Filters, sieveModel?.Sorts, sieveModel?.Page, sieveModel?.PageSize);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while getting Hospices : {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting Hospices :{ex.Message}");
                throw;
            }
        }

        public async Task UpsertHospiceSubscriptions(int hospiceId)
        {
            try
            {
                await SetAuthorizationToken();

                if (string.Equals(_contractConfig.ContractSource, "ZAB", StringComparison.OrdinalIgnoreCase))
                {
                    await _coreAPI.UpsertHospiceSubscriptions(hospiceId);
                    return;
                }
                await _coreAPI.UpsertHms2HospiceContracts(hospiceId);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Error Occurred while upserting Hospice subscriptions : {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while upserting Hospice subscriptions :{ex.Message}");
                throw;
            }
        }

        public async Task UpdateHospiceFhirOrganizationId(int hospiceId, Guid fhirOrganizationId)
        {
            try
            {
                await SetAuthorizationToken();
                await _coreAPI.UpdateHospiceFhirOrganizationId(hospiceId, fhirOrganizationId);
            }
            catch (ApiException ex)
            {
                _logger.LogWarning($"Exception Occurred while updating patient status with uniqueId({hospiceId}) :{ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new ValidationException(JsonConvert.DeserializeObject<string>(ex.Content));
                }
            }
        }

        #endregion

        private async Task SetAuthorizationToken()
        {
            var accessToken = await _tokenService.GetAccessTokenByClientCredentials(_coreConfig.IdentityClient);
            _coreAPI.BearerToken = $"Bearer {accessToken}";
        }
    }
}
