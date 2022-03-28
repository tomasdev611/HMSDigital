using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using RestEase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreSDK.Interfaces
{
    public interface ICoreAPI
    {
        [Header("Authorization", "Bearer")]
        string BearerToken { get; set; }

        #region Dispatch

        [Post("api/dispatch/un-assign")]
        Task UnAssignOrder([Body] int orderId);

        [Get("api/dispatch-instructions")]
        Task<PaginatedList<DispatchInstruction>> GetAllDispatchInstruction();

        [Get("api/dispatch/loadlist")]
        Task<SiteLoadList> GetLoadList([Query] int siteId, [Query] string filters, [Query] string sorts, [Query] int? page, [Query] int? pageSize);

        [Get("api/dispatch/me")]
        Task<DispatchResponse> GetLoggedInDriverDispatch([Query] string filters, [Query] string sorts, [Query] int? page, [Query] int? pageSize);

        [Post("/api/dispatch/pickup")]
        Task PickupDispatchRequest([Body] DispatchMovementRequest pickupRequest);

        #endregion

        #region Auth

        [Post("auth/token")]
        Task<SignInResponse> SignIn([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> loginRequest);

        #endregion

        #region Enum

        [Get("api/enumerations")]
        Task<Enumeration> GetAllEnumerations();

        #endregion

        #region User

        [Get("api/users/me")]
        Task<User> GetMyUser();

        #endregion

        #region Inventory

        [Get("api/inventory")]
        Task<PaginatedList<Inventory>> GetAllInventory([Query] string filters, [Query] string sorts, [Query] int? page, [Query] int? pageSize);

        #endregion

        #region Site

        [Get("/api/sites/{siteId}")]
        Task<Site> GetSiteById([Path] int siteId);

        [Get("api/sites")]
        Task<PaginatedList<Site>> GetAllSites([Query] string filters, [Query] string sorts, [Query] int? page, [Query] int? pageSize);

        #endregion

        #region OrderLineItemFulfillments

        [Get("/api/order-headers/{orderHeaderId}/fulfillment")]
        Task<PaginatedList<OrderFulfillmentLineItem>> GetOrderFulfillments([Path] int orderHeaderId, [Query] string filters, [Query] string sorts, [Query] int? page, [Query] int? pageSize);

        #endregion

        #region Fulfillment

        [Post("api/dispatch/fulfill-order")]
        Task FulfillOrder([Body] OrderFulfillmentRequest fulfillmentRequest);

        #endregion

        #region SiteMember

        [Get("/api/sites/members/me")]
        Task<SiteMember> GetMySiteMember();

        #endregion

        #region Driver

        [Get("/api/drivers/me")]
        Task<Driver> GetMyDriver();

        #endregion

        #region Hospice

        [Get("/api/hospices/search")]
        Task<PaginatedList<Hospice>> SearchHospices([Query] string searchQuery);

        [Get("/api/hospices")]
        Task<PaginatedList<Hospice>> GetAllHospices([Query] string filters, [Query] string sorts, [Query] int? page, [Query] int? pageSize);

        [Put("/api/hospices/{hospiceId}/subscriptions")]
        Task UpsertHospiceSubscriptions([Path] int hospiceId);

        [Put("api/hospices/fhirOrganizationId")]
        Task UpdateHospiceFhirOrganizationId([Query] int hospiceId, [Body] Guid fhirOrganizationId);

        [Put("/api/hospices/{hospiceId}/hms2-contracts")]
        Task UpsertHms2HospiceContracts([Path] int hospiceId);

        #endregion
    }
}
