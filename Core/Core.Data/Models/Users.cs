using System;
using System.Collections.Generic;

#nullable disable

namespace HMSDigital.Core.Data.Models
{
    public partial class Users
    {
        public Users()
        {
            ContractRecordsCreatedByUser = new HashSet<ContractRecords>();
            ContractRecordsUpdatedByUser = new HashSet<ContractRecords>();
            CreditHoldHistory = new HashSet<CreditHoldHistory>();
            CsvMappingsCreatedByUser = new HashSet<CsvMappings>();
            CsvMappingsUpdatedByUser = new HashSet<CsvMappings>();
            DispatchAuditLog = new HashSet<DispatchAuditLog>();
            DispatchInstructionsCreatedByUser = new HashSet<DispatchInstructions>();
            DispatchInstructionsUpdatedByUser = new HashSet<DispatchInstructions>();
            DriversCreatedByUser = new HashSet<Drivers>();
            DriversUpdatedByUser = new HashSet<Drivers>();
            DriversUser = new HashSet<Drivers>();
            FacilitiesCreatedByUser = new HashSet<Facilities>();
            FacilitiesUpdatedByUser = new HashSet<Facilities>();
            FacilityPatientHistoryCreatedByUser = new HashSet<FacilityPatientHistory>();
            FacilityPatientHistoryUpdatedByUser = new HashSet<FacilityPatientHistory>();
            HospiceLocationMembersCreatedByUser = new HashSet<HospiceLocationMembers>();
            HospiceLocationMembersUpdatedByUser = new HashSet<HospiceLocationMembers>();
            HospiceLocationsCreatedByUser = new HashSet<HospiceLocations>();
            HospiceLocationsDeletedByUser = new HashSet<HospiceLocations>();
            HospiceLocationsUpdatedByUser = new HashSet<HospiceLocations>();
            HospiceMemberCreatedByUser = new HashSet<HospiceMember>();
            HospiceMemberUpdatedByUser = new HashSet<HospiceMember>();
            HospiceMemberUser = new HashSet<HospiceMember>();
            HospicesCreatedByUser = new HashSet<Hospices>();
            HospicesCreditHoldByUser = new HashSet<Hospices>();
            HospicesDeletedByUser = new HashSet<Hospices>();
            HospicesUpdatedByUser = new HashSet<Hospices>();
            InventoryCreatedByUser = new HashSet<Inventory>();
            InventoryDeletedByUser = new HashSet<Inventory>();
            InventoryUpdatedByUser = new HashSet<Inventory>();
            InverseCreatedByUser = new HashSet<Users>();
            InverseDisabledByUser = new HashSet<Users>();
            ItemCategoriesCreatedByUser = new HashSet<ItemCategories>();
            ItemCategoriesDeletedByUser = new HashSet<ItemCategories>();
            ItemCategoriesUpdatedByUser = new HashSet<ItemCategories>();
            ItemImageFilesCreatedByUser = new HashSet<ItemImageFiles>();
            ItemImageFilesUpdatedByUser = new HashSet<ItemImageFiles>();
            ItemImagesCreatedByUser = new HashSet<ItemImages>();
            ItemImagesUpdatedByUser = new HashSet<ItemImages>();
            ItemTransferRequestsCreatedByUser = new HashSet<ItemTransferRequests>();
            ItemTransferRequestsUpdatedByUser = new HashSet<ItemTransferRequests>();
            ItemsCreatedByUser = new HashSet<Items>();
            ItemsDeletedByUser = new HashSet<Items>();
            ItemsUpdatedByUser = new HashSet<Items>();
            OrderFulfillmentLineItemsCreatedByUser = new HashSet<OrderFulfillmentLineItems>();
            OrderFulfillmentLineItemsUpdatedByUser = new HashSet<OrderFulfillmentLineItems>();
            OrderHeadersCreatedByUser = new HashSet<OrderHeaders>();
            OrderHeadersUpdatedByUser = new HashSet<OrderHeaders>();
            OrderLineItemsCreatedByUser = new HashSet<OrderLineItems>();
            OrderLineItemsUpdatedByUser = new HashSet<OrderLineItems>();
            OrderNotesCreatedByUser = new HashSet<OrderNotes>();
            OrderNotesUpdatedByUser = new HashSet<OrderNotes>();
            SiteMembersCreatedByUser = new HashSet<SiteMembers>();
            SiteMembersUpdatedByUser = new HashSet<SiteMembers>();
            SiteMembersUser = new HashSet<SiteMembers>();
            SiteServiceAreasCreatedByUser = new HashSet<SiteServiceAreas>();
            SiteServiceAreasUpdatedByUser = new HashSet<SiteServiceAreas>();
            SitesCreatedByUser = new HashSet<Sites>();
            SitesDeletedByUser = new HashSet<Sites>();
            SitesUpdatedByUser = new HashSet<Sites>();
            UserProfilePictureCreatedByUser = new HashSet<UserProfilePicture>();
            UserProfilePictureUpdatedByUser = new HashSet<UserProfilePicture>();
            UserProfilePictureUser = new HashSet<UserProfilePicture>();
            UserRoles = new HashSet<UserRoles>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public long PhoneNumber { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public int CountryCode { get; set; }
        public string CognitoUserId { get; set; }
        public int? SklocationId { get; set; }
        public int? SkroleId { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public int OtpVerifyFailCount { get; set; }
        public int? DisabledByUserId { get; set; }
        public bool? IsDisabled { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual Users DisabledByUser { get; set; }
        public virtual ICollection<ContractRecords> ContractRecordsCreatedByUser { get; set; }
        public virtual ICollection<ContractRecords> ContractRecordsUpdatedByUser { get; set; }
        public virtual ICollection<CreditHoldHistory> CreditHoldHistory { get; set; }
        public virtual ICollection<CsvMappings> CsvMappingsCreatedByUser { get; set; }
        public virtual ICollection<CsvMappings> CsvMappingsUpdatedByUser { get; set; }
        public virtual ICollection<DispatchAuditLog> DispatchAuditLog { get; set; }
        public virtual ICollection<DispatchInstructions> DispatchInstructionsCreatedByUser { get; set; }
        public virtual ICollection<DispatchInstructions> DispatchInstructionsUpdatedByUser { get; set; }
        public virtual ICollection<Drivers> DriversCreatedByUser { get; set; }
        public virtual ICollection<Drivers> DriversUpdatedByUser { get; set; }
        public virtual ICollection<Drivers> DriversUser { get; set; }
        public virtual ICollection<Facilities> FacilitiesCreatedByUser { get; set; }
        public virtual ICollection<Facilities> FacilitiesUpdatedByUser { get; set; }
        public virtual ICollection<FacilityPatientHistory> FacilityPatientHistoryCreatedByUser { get; set; }
        public virtual ICollection<FacilityPatientHistory> FacilityPatientHistoryUpdatedByUser { get; set; }
        public virtual ICollection<HospiceLocationMembers> HospiceLocationMembersCreatedByUser { get; set; }
        public virtual ICollection<HospiceLocationMembers> HospiceLocationMembersUpdatedByUser { get; set; }
        public virtual ICollection<HospiceLocations> HospiceLocationsCreatedByUser { get; set; }
        public virtual ICollection<HospiceLocations> HospiceLocationsDeletedByUser { get; set; }
        public virtual ICollection<HospiceLocations> HospiceLocationsUpdatedByUser { get; set; }
        public virtual ICollection<HospiceMember> HospiceMemberCreatedByUser { get; set; }
        public virtual ICollection<HospiceMember> HospiceMemberUpdatedByUser { get; set; }
        public virtual ICollection<HospiceMember> HospiceMemberUser { get; set; }
        public virtual ICollection<Hospices> HospicesCreatedByUser { get; set; }
        public virtual ICollection<Hospices> HospicesCreditHoldByUser { get; set; }
        public virtual ICollection<Hospices> HospicesDeletedByUser { get; set; }
        public virtual ICollection<Hospices> HospicesUpdatedByUser { get; set; }
        public virtual ICollection<Inventory> InventoryCreatedByUser { get; set; }
        public virtual ICollection<Inventory> InventoryDeletedByUser { get; set; }
        public virtual ICollection<Inventory> InventoryUpdatedByUser { get; set; }
        public virtual ICollection<Users> InverseCreatedByUser { get; set; }
        public virtual ICollection<Users> InverseDisabledByUser { get; set; }
        public virtual ICollection<ItemCategories> ItemCategoriesCreatedByUser { get; set; }
        public virtual ICollection<ItemCategories> ItemCategoriesDeletedByUser { get; set; }
        public virtual ICollection<ItemCategories> ItemCategoriesUpdatedByUser { get; set; }
        public virtual ICollection<ItemImageFiles> ItemImageFilesCreatedByUser { get; set; }
        public virtual ICollection<ItemImageFiles> ItemImageFilesUpdatedByUser { get; set; }
        public virtual ICollection<ItemImages> ItemImagesCreatedByUser { get; set; }
        public virtual ICollection<ItemImages> ItemImagesUpdatedByUser { get; set; }
        public virtual ICollection<ItemTransferRequests> ItemTransferRequestsCreatedByUser { get; set; }
        public virtual ICollection<ItemTransferRequests> ItemTransferRequestsUpdatedByUser { get; set; }
        public virtual ICollection<Items> ItemsCreatedByUser { get; set; }
        public virtual ICollection<Items> ItemsDeletedByUser { get; set; }
        public virtual ICollection<Items> ItemsUpdatedByUser { get; set; }
        public virtual ICollection<OrderFulfillmentLineItems> OrderFulfillmentLineItemsCreatedByUser { get; set; }
        public virtual ICollection<OrderFulfillmentLineItems> OrderFulfillmentLineItemsUpdatedByUser { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeadersCreatedByUser { get; set; }
        public virtual ICollection<OrderHeaders> OrderHeadersUpdatedByUser { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItemsCreatedByUser { get; set; }
        public virtual ICollection<OrderLineItems> OrderLineItemsUpdatedByUser { get; set; }
        public virtual ICollection<OrderNotes> OrderNotesCreatedByUser { get; set; }
        public virtual ICollection<OrderNotes> OrderNotesUpdatedByUser { get; set; }
        public virtual ICollection<SiteMembers> SiteMembersCreatedByUser { get; set; }
        public virtual ICollection<SiteMembers> SiteMembersUpdatedByUser { get; set; }
        public virtual ICollection<SiteMembers> SiteMembersUser { get; set; }
        public virtual ICollection<SiteServiceAreas> SiteServiceAreasCreatedByUser { get; set; }
        public virtual ICollection<SiteServiceAreas> SiteServiceAreasUpdatedByUser { get; set; }
        public virtual ICollection<Sites> SitesCreatedByUser { get; set; }
        public virtual ICollection<Sites> SitesDeletedByUser { get; set; }
        public virtual ICollection<Sites> SitesUpdatedByUser { get; set; }
        public virtual ICollection<UserProfilePicture> UserProfilePictureCreatedByUser { get; set; }
        public virtual ICollection<UserProfilePicture> UserProfilePictureUpdatedByUser { get; set; }
        public virtual ICollection<UserProfilePicture> UserProfilePictureUser { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
