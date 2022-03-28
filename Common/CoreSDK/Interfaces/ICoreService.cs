using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System;
using System.Threading.Tasks;

namespace CoreSDK.Interfaces
{
    public interface ICoreService
    {

        #region Dispatch

        Task UnAssignOrder(int orderId);

        Task<PaginatedList<DispatchInstruction>> GetAllDispatchInstructions();

        Task<SiteLoadList> GetLoadList(int siteId, SieveModel sieveModel);

        Task<DispatchResponse> GetLoggedInDriverDispatch(SieveModel sieveModel);

        Task PickupDispatchRequest(DispatchMovementRequest pickupRequest);

        #endregion

        #region Auth

        Task<SignInResponse> SignIn(LoginRequest loginRequest);

        #endregion

        #region Enum

        Task<Enumeration> GetAllEnumerations();

        #endregion

        #region User

        Task<User> GetMyUser();

        #endregion

        #region Inventory

        Task<PaginatedList<Inventory>> GetAllInventory(SieveModel sieveModel);

        #endregion

        #region Site

        Task<Site> GetSiteById(int siteId);

        Task<PaginatedList<Site>> GetAllSites(SieveModel sieveModel);

        #endregion

        #region OrderLineItemFulfillments

        Task<PaginatedList<OrderFulfillmentLineItem>> GetOrderFulfillments(int orderHeaderId, SieveModel sieveModel);

        #endregion

        #region Fulfillment

        Task FulfillOrder(OrderFulfillmentRequest fulfillmentRequest);

        #endregion

        #region SiteMember

        Task<SiteMember> GetMySiteMember();

        #endregion

        #region Driver

        Task<Driver> GetMyDriver();

        #endregion

        #region Hospices

        Task<PaginatedList<Hospice>> SearchHospices(string searchQuery);

        Task<PaginatedList<Hospice>> GetAllHospices(SieveModel sieveModel);

        Task UpsertHospiceSubscriptions(int hospiceId);

        Task UpdateHospiceFhirOrganizationId(int hospiceId, Guid fhirOrganizationId);

        #endregion
    }
}
