using Amazon.CognitoIdentityProvider.Model;
using AutoMapper;
using System;
using System.Linq;
using HMSDigital.Core.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using Audit.EntityFramework;
using Amazon.Extensions.CognitoAuthentication;
using HMSDigital.Core.Data.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using HMSDigital.Common.ViewModels;
using NSViewModel = HMSDigital.Core.ViewModels.NetSuite;
using NSSDKViewModel = HospiceSource.Digital.NetSuite.SDK.ViewModels;
using HMSDigital.Common.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Constants;
using Newtonsoft.Json.Linq;
using HMSDigital.Core.BusinessLayer.DTOs;
using SmartyStreets.USStreetApi;
using System.Globalization;
using HMSDigital.Patient.FHIR.Models;
using HMSDigital.Patient.ViewModels;
using NotificationSDK.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Hms2BillingSDK.Models;
using Hms2BillingSDK;

namespace HMSDigital.Core.API
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserType, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => GetAttributeValue(src.Attributes, "email")))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetAttributeValue(src.Attributes, "name")))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => GetAttributeValue(src.Attributes, "given_name")))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => GetAttributeValue(src.Attributes, "family_name")))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => GetCountryCodeFromPhoneNumber(GetAttributeValue(src.Attributes, "phone_number"))))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => GetPhoneNumberWithoutCountryCode(GetAttributeValue(src.Attributes, "phone_number"))))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus.Value));

            CreateMap<AdminGetUserResponse, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => GetAttributeValue(src.UserAttributes, "email")))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetAttributeValue(src.UserAttributes, "name")))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => GetAttributeValue(src.UserAttributes, "given_name")))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => GetAttributeValue(src.UserAttributes, "family_name")))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => GetCountryCodeFromPhoneNumber(GetAttributeValue(src.UserAttributes, "phone_number"))))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => GetPhoneNumberWithoutCountryCode(GetAttributeValue(src.UserAttributes, "phone_number"))))
                .ForMember(dest => dest.UserStatus, opt => opt.MapFrom(src => src.UserStatus.Value))
                .ForMember(dest => dest.IsEmailVerified, opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrEmpty(GetAttributeValue(src.UserAttributes, "email_verified")));
                    opt.MapFrom(src => GetAttributeValue(src.UserAttributes, "email_verified"));
                })
                .ForMember(dest => dest.IsPhoneNumberVerified, opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrEmpty(GetAttributeValue(src.UserAttributes, "phone_number_verified")));
                    opt.MapFrom(src => GetAttributeValue(src.UserAttributes, "phone_number_verified"));
                });

            CreateMap<UserRoleBase, Data.Models.UserRoles>()
                .ReverseMap();

            CreateMap<Data.Models.UserRoles, UserRole>();

            CreateMap<UserCreateRequest, Data.Models.Users>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles))
                .ReverseMap();

            CreateMap<User, Data.Models.Users>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles))
                .ForMember(dest => dest.CognitoUserId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => !(src.IsDisabled ?? false)));

            CreateMap<AddressMinimal, Addresses>().ReverseMap();
            CreateMap<AddressMinimal, Address>();
            CreateMap<Address, Addresses>()
                .ReverseMap();

            CreateMap<StandardizedAddress, Address>()
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.PostalCode) ? null : src.PostalCode))
                .ForMember(dest => dest.Plus4Code, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Plus4Code) ? null : src.Plus4Code))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Latitude) ? null : src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Longitude) ? null : src.Longitude))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.VerifiedBy, opt => opt.MapFrom(src => "Melissa"));

            CreateMap<AddressSuggestionResult, SuggestionResponse>()
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.PostalCode.Split("-", StringSplitOptions.None)[0]))
                .ForMember(dest => dest.Plus4Code, opt => opt.MapFrom(src => (src.Address.PostalCode.Split("-", StringSplitOptions.None).Length == 2) ?
                                                                                 src.Address.PostalCode.Split("-", StringSplitOptions.None)[1] : null))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address.AddressLine1))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
                .ForMember(dest => dest.AddressKey, opt => opt.MapFrom(src => src.Address.AddressKey))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Address.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Address.Longitude))
                .ForMember(dest => dest.AddressKey, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => "United States of America"))
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => "Address Verified"))
                .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.VerifiedBy, opt => opt.MapFrom(src => "Melissa"));

            CreateMap<Data.Models.Sites, Site>()
                  .ForMember(dest => dest.CurrentDriverName, opt =>
                  {
                      opt.PreCondition(src => src.DriversCurrentVehicle != null);
                      opt.MapFrom(src => $"{src.DriversCurrentVehicle.User.FirstName} " +
                      $"{src.DriversCurrentVehicle.User.LastName}");
                  })
                .ForMember(dest => dest.CurrentDriverId, opt =>
                {
                    opt.PreCondition(src => src.DriversCurrentVehicle != null);
                    opt.MapFrom(src => src.DriversCurrentVehicle.Id);
                });

            CreateMap<BusinessLayer.DTOs.ChangePasswordRequest, Amazon.CognitoIdentityProvider.Model.ChangePasswordRequest>()
                .ForMember(dest => dest.PreviousPassword, opt => opt.MapFrom(src => src.OldPassword))
                .ForMember(dest => dest.ProposedPassword, opt => opt.MapFrom(src => src.NewPassword));

            CreateMap<ViewModels.ChangePasswordRequest, BusinessLayer.DTOs.ChangePasswordRequest>();

            CreateMap<Data.Models.Sites, ViewModels.SiteLocation>().ForMember(dest => dest.CurrentDriverName, opt =>
            {
                opt.PreCondition(src => src.DriversCurrentVehicle != null);
                opt.MapFrom(src => $"{src.DriversCurrentVehicle.User.FirstName} " +
                $"{src.DriversCurrentVehicle.User.LastName}");
            })
                .ForMember(dest => dest.CurrentDriverId, opt =>
                {
                    opt.PreCondition(src => src.DriversCurrentVehicle != null);
                    opt.MapFrom(src => src.DriversCurrentVehicle.Id);
                });

            CreateMap<Data.Models.SiteServiceAreas, ViewModels.ServiceArea>();

            CreateMap<Data.Models.Roles, Core.ViewModels.Role>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => $"{rp.PermissionNoun.Name}:{rp.PermissionVerb.Name}")))
                .ForMember(dest => dest.PermissionsLength, opt => opt.MapFrom(src => src.RolePermissions.Count()))
                .ReverseMap();

            CreateMap<ViewModels.UserAuditLog, UserAuditCsvReport>()
                .ForMember(dest => dest.TargetUserCognitoId, opt => opt.MapFrom(src => src.TargetUser.UserId))
                .ForMember(dest => dest.TargetUserName, opt =>
                {
                    opt.PreCondition(src => src.TargetUser != null);
                    opt.MapFrom(src => $"{src.TargetUser.FirstName} {src.TargetUser.LastName}");
                })
                .ForMember(dest => dest.UserCognitoId, opt => opt.MapFrom(src => src.User.UserId))
                .ForMember(dest => dest.UserName, opt =>
                {
                    opt.PreCondition(src => src.User != null);
                    opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}");
                })
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            CreateMap<AuthFlowResponse, SignInResponse>()
                 .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AuthenticationResult.AccessToken))
                 .ForMember(dest => dest.IdToken, opt => opt.MapFrom(src => src.AuthenticationResult.IdToken))
                 .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.AuthenticationResult.RefreshToken));

            CreateMap<InitiateAuthResponse, SignInResponse>()
                 .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AuthenticationResult.AccessToken))
                 .ForMember(dest => dest.IdToken, opt => opt.MapFrom(src => src.AuthenticationResult.IdToken));

            CreateMap<HospiceRequest, Hospices>();

            CreateMap<Hospices, Hospice>()
                .Include<Hospices, FHIRHospice>()
                .ForMember(dest => dest.CreditHoldByUserName, opt =>
                 {
                     opt.PreCondition(src => src.CreditHoldByUser != null);
                     opt.MapFrom(src => $"{src.CreditHoldByUser.FirstName} {src.CreditHoldByUser.LastName}");
                 });

            CreateMap<Hospices, FHIRHospice>();

            CreateMap<Hospices, Data.Models.CreditHoldHistory>()
                .ForMember(dest => dest.HospiceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Data.Models.CreditHoldHistory, ViewModels.CreditHoldHistory>()
                .ForMember(dest => dest.CreditHoldByUserName, opt =>
                {
                    opt.PreCondition(src => src.CreditHoldByUser != null);
                    opt.MapFrom(src => $"{src.CreditHoldByUser.FirstName} {src.CreditHoldByUser.LastName}");
                });

            CreateMap<HospiceLocations, HospiceLocation>();

            CreateMap<JsonPatchDocument<Hospice>, JsonPatchDocument<Hospices>>();
            CreateMap<Operation<Hospice>, Operation<Hospices>>();
            CreateMap<JsonPatchDocument<HospiceLocation>, JsonPatchDocument<HospiceLocations>>();
            CreateMap<Operation<HospiceLocation>, Operation<HospiceLocations>>();
            CreateMap<JsonPatchDocument<Site>, JsonPatchDocument<Sites>>();
            CreateMap<Operation<Site>, Operation<Sites>>();

            CreateMap<Data.Models.Facilities, Facility>();

            CreateMap<User, ViewModels.HospiceMember>();

            CreateMap<HospiceMemberRequest, UserCreateRequest>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<HospiceMemberRequest, UserMinimal>();

            CreateMap<HospiceMemberCsvDTO, HospiceMemberRequest>();

            CreateMap<HospiceMemberCsvRequest, HospiceMemberCsvDTO>();

            CreateMap<FacilityRequest, Facilities>()
                .ReverseMap();
            CreateMap<Data.Models.FacilityPhoneNumber, ViewModels.FacilityPhoneNumber>()
                .ReverseMap();

            CreateMap<FacilityCsvDTO, Address>();
            CreateMap<FacilityCsvRequest, FacilityCsvDTO>();
            CreateMap<FacilityCsvDTO, Facilities>();

            CreateMap<PhoneNumberRequest, Data.Models.PhoneNumbers>()
                .ReverseMap()
                .ForMember(dest => dest.NumberType, opt => opt.MapFrom(src => src.NumberType.Name));

            CreateMap<Addresses, Address>()
               .ReverseMap();
            CreateMap<JsonPatchDocument<Facility>, JsonPatchDocument<Facilities>>();
            CreateMap<Operation<Facility>, Operation<Facilities>>();

            CreateMap<OrderHeaders, OrderHeader>()
                 .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => GetOrderStatus(src.StatusId)))
                 .ForMember(dest => dest.DispatchStatus, opt => opt.MapFrom(src => GetOrderStatus(src.DispatchStatusId)))
                  .ForMember(dest => dest.OrderType, opt => opt.MapFrom(src => src.OrderType.Name));

            CreateMap<OrderLineItems, OrderLineItem>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetOrderLineItemStatus(src.StatusId)))
               .ForMember(dest => dest.DispatchStatus, opt => opt.MapFrom(src => GetOrderLineItemStatus(src.DispatchStatusId)))
               .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action.Name))
               .ForMember(dest => dest.EquipmentSettings, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<JArray>(src.EquipmentSettings)));

            CreateMap<Users, ViewModels.HospiceMember>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<ViewModels.HospiceMember, Data.Models.HospiceMember>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ReverseMap()
                .ForMember(dest => dest.HospiceLocations, opt => opt.MapFrom(src => src.HospiceLocationMembers.Where(l => l.HospiceLocation != null)
                                                                                                                .Select(lm => lm.HospiceLocation)));
            CreateMap<Data.Models.HospiceLocationMembers, ViewModels.HospiceLocationMember>();

            CreateMap<FacilityPatient, FacilityPatientResponse>();

            CreateMap<FacilityPatientHistory, FacilityPatientSearchResponse>()
                .ForMember(dest => dest.AssignedDateTime, opt => opt.MapFrom(src => src.CreatedDateTime));

            CreateMap<Sites, Vehicle>()
                .ForMember(dest => dest.CurrentDriverName, opt =>
                {
                    opt.NullSubstitute("");
                    opt.MapFrom(src => $"{src.DriversCurrentVehicle.User.FirstName} " +
                    $"{src.DriversCurrentVehicle.User.LastName}");
                })
                .ForMember(dest => dest.CurrentDriverId, opt =>
                {
                    opt.PreCondition(src => src.DriversCurrentVehicle != null);
                    opt.MapFrom(src => src.DriversCurrentVehicle.Id);
                });

            CreateMap<Data.Models.Inventory, ViewModels.Inventory>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<InventoryRequest, Data.Models.Inventory>();
            CreateMap<JsonPatchDocument<ViewModels.Inventory>, JsonPatchDocument<Data.Models.Inventory>>();
            CreateMap<Operation<ViewModels.Inventory>, Operation<Data.Models.Inventory>>();

            CreateMap<Items, Item>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ItemCategoryMapping.Select(i => i.ItemCategory)))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.ItemSubCategoryMapping.Select(i => i.ItemSubCategory)));
            CreateMap<ItemRequest, Items>();
            CreateMap<JsonPatchDocument<Item>, JsonPatchDocument<Items>>();
            CreateMap<Operation<Item>, Operation<Items>>();
            CreateMap<ItemCategories, ItemCategory>();

            CreateMap<EquipmentSettingsConfig, ViewModels.EquipmentSettingConfig>()
                .ReverseMap();

            CreateMap<EquipmentSettingTypes, ViewModels.EquipmentSettingType>();

            CreateMap<AddOnsConfigRequest, AddOnGroups>()
                .ForMember(dest => dest.AddOnGroupProducts, opt => opt.MapFrom(src => src.ProductIds.Select(i => new AddOnGroupProducts() { ItemId = i })))
                .ReverseMap()
                .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.AddOnGroupProducts.Select(i => i.ItemId)));

            CreateMap<AddOnGroups, AddOnsGroup>();

            CreateMap<AddOnGroupProducts, AddOnsGroupProduct>()
                .ForMember(dest => dest.ItemName, opt =>
                {
                    opt.PreCondition(src => src.Item != null);
                    opt.MapFrom(src => src.Item.Name);
                })
                .ForMember(dest => dest.ItemImageUrls, opt =>
                {
                    opt.PreCondition(src => src.Item != null);
                    opt.MapFrom(src => src.Item.ItemImages.Select(i => i.Url));
                });

            CreateMap<ItemImages, ItemImage>();

            CreateMap<Users, Driver>();

            CreateMap<Driver, Drivers>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ReverseMap();
            CreateMap<DriverBase, UserCreateRequest>();
            CreateMap<DriverBase, UserMinimal>();
            CreateMap<ItemCategoryRequest, ItemCategories>();
            CreateMap<JsonPatchDocument<ItemCategory>, JsonPatchDocument<ItemCategories>>();
            CreateMap<Operation<ItemCategory>, Operation<ItemCategories>>();

            CreateMap<FileMetadataRequest, FilesMetadata>();
            CreateMap<ItemImageFileRequest, FilesMetadata>();
            CreateMap<ItemImageFileRequest, ItemImageFiles>()
                .ForMember(dest => dest.FileMetadata, opt => opt.MapFrom(src => src));

            CreateMap<UserPictureFileRequest, FilesMetadata>();
            CreateMap<UserPictureFileRequest, Data.Models.UserProfilePicture>()
                .ForMember(dest => dest.FileMetadata, opt => opt.MapFrom(src => src));

            CreateMap<FilesMetadata, ViewModels.UserProfilePicture>();
            CreateMap<ViewModels.UserProfilePicture, Data.Models.UserProfilePicture>().
                ForMember(dest => dest.FileMetadata, opt => opt.MapFrom(src => src))
                .ReverseMap();

            CreateMap<FilesMetadata, ItemImageFile>();
            CreateMap<ItemImageFile, ItemImageFiles>().
                ForMember(dest => dest.FileMetadata, opt => opt.MapFrom(src => src))
                .ReverseMap();

            CreateMap<Data.Models.SitePhoneNumber, Common.ViewModels.SitePhoneNumber>()
                .ReverseMap();
            CreateMap<PhoneNumberMinimal, Data.Models.PhoneNumbers>()
                .ReverseMap()
                .ForMember(dest => dest.NumberType, opt => opt.MapFrom(src => src.NumberType.Name));

            CreateMap<SiteMemberRequest, UserCreateRequest>();
            CreateMap<SiteMemberRequest, UserMinimal>();
            CreateMap<Users, SiteMember>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<SiteMember, SiteMembers>()
                 .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                 .ReverseMap();

            CreateMap<ItemTransferCreateRequest, ItemTransferRequests>();

            CreateMap<ItemTransferRequests, ItemTransferRequest>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<DispatchInstructionRequest, DispatchInstructions>();
            CreateMap<DispatchAssignmentRequest, DispatchInstructions>();

            CreateMap<DispatchInstructions, DispatchInstruction>();

            CreateMap<OrderLineItems, LoadlistItem>()
                .ForMember(dest => dest.DispatchType, opt => opt.MapFrom(src => src.Action.Name))
                .ForMember(dest => dest.DispatchStatus, opt => opt.MapFrom(src => src.Status.Name));

            CreateMap<NSViewModel.NSHospiceRequest, Hospices>()
                .ForMember(dest => dest.HospiceLocations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Address != null ? (!string.IsNullOrEmpty(src.Address.Phone) ? src.Address : null) : null));

            CreateMap<NSViewModel.HospiceLocationCreateRequest, HospiceLocations>()
                .ForMember(dest => dest.CustomerType, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Address != null ? (!string.IsNullOrEmpty(src.Address.Phone) ? src.Address : null) : null));

            CreateMap<NSViewModel.AddressRequest, Addresses>();

            CreateMap<NSViewModel.AddressRequest, PhoneNumbers>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.NumberTypeId, opt => opt.MapFrom(src => (int)(PhoneNumberType.Work)))
                .ReverseMap();

            CreateMap<NSViewModel.AddressRequest, Data.Models.FacilityPhoneNumber>()
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src))
               .ReverseMap();

            CreateMap<Hospices, NSViewModel.HospiceResponse>();

            CreateMap<NSViewModel.HospiceDeleteRequest, NSViewModel.HospiceResponse>();

            CreateMap<NSViewModel.NSHospiceRequest, HospiceMemberRequest>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => NetSuiteCustomer.FIRST_NAME))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => NetSuiteCustomer.LAST_NAME))
               .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => NetSuiteCustomer.DESIGNATION));

            CreateMap<HospiceMemberRequest, NSSDKViewModel.CustomerContact>();
            CreateMap<HospiceMemberRequest, NSSDKViewModel.CustomerContactBase>();

            CreateMap<NSViewModel.PhoneNumberReqeust, Data.Models.PhoneNumbers>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.NumberTypeId, opt => opt.MapFrom(src => (int)PhoneNumberType.Work));

            CreateMap<NSViewModel.PhoneNumberReqeust, Data.Models.SitePhoneNumber>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src));

            CreateMap<NSViewModel.WarehouseRequest, Data.Models.Sites>()
                .ForMember(dest => dest.SitePhoneNumber, opt => opt.MapFrom(src => src.PhoneNumbers));

            CreateMap<Data.Models.Sites, NSViewModel.WarehouseResponse>();

            CreateMap<NSViewModel.NSItemRequest, Items>();
            CreateMap<Items, NSViewModel.ItemResponse>();
            CreateMap<NSViewModel.ItemDeleteRequest, NSViewModel.ItemResponse>();

            CreateMap<NSViewModel.NSInventory, Data.Models.Inventory>();
            CreateMap<Data.Models.Inventory, NSViewModel.InventoryResponse>();
            CreateMap<NSViewModel.InventoryDeleteRequest, NSViewModel.InventoryResponse>();

            CreateMap<NSViewModel.HospiceContactRequest, HospiceMemberRequest>();
            CreateMap<ViewModels.HospiceMember, NSViewModel.HospiceContactResponse>();

            CreateMap<Data.Models.Items, ViewModels.PatientInventory>()
                 .ForMember(dest => dest.DeletedDateTime, opt => opt.Ignore())
                 .ForMember(dest => dest.DeletedByUserId, opt => opt.Ignore());
            CreateMap<Data.Models.Inventory, ViewModels.PatientInventory>();
            CreateMap<Data.Models.OrderHeaders, ViewModels.PatientInventory>();
            CreateMap<Data.Models.OrderNotes, ViewModels.OrderNote>()
                .ForMember(dest => dest.CreatedByUserName, opt =>
                {
                    opt.PreCondition(src => src.CreatedByUser != null);
                    opt.MapFrom(src => $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}");
                });

            CreateMap<Data.Models.PatientInventory, ViewModels.PatientInventory>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.ItemCount))
                .ForMember(dest => dest.NetSuiteOrderLineItemId, opt => opt.MapFrom(src => src.OrderLineItem.NetSuiteOrderLineItemId))
                .IncludeMembers(s => s.OrderHeader, s => s.Item, s => s.Inventory);

            CreateMap<Users, NSSDKViewModel.CustomerContactBase>()
                .ForMember(dest => dest.Phone, opt =>
                {
                    opt.PreCondition(src => src.PhoneNumber != 0);
                    opt.MapFrom(src => src.PhoneNumber);
                });

            CreateMap<ItemSubCategories, ItemSubCategory>();
            CreateMap<NSViewModel.NSItemCategory, ItemCategories>();
            CreateMap<NSViewModel.NSItemSubCategory, ItemSubCategories>();

            CreateMap<Data.Models.OrderFulfillmentLineItems, ViewModels.OrderFulfillmentLineItem>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => GetItemNameFromOrderLine(src.OrderLineItem)))
                .ForMember(dest => dest.FulfilledByDriverName, opt => opt.MapFrom(src => GetDriverName(src.FulfilledByDriver)))
                .ForMember(dest => dest.FulfilledByVehicleCvn, opt => opt.MapFrom(src => src.FulfilledByVehicle != null ? src.FulfilledByVehicle.Cvn : null));

            CreateMap<Data.Models.OrderFulfillmentLineItems, NSSDKViewModel.FulfilmentItem>()
                .ForMember(dest => dest.NetSuiteOrderLineItemId, opt => opt.MapFrom(src => src.OrderLineItem.NetSuiteOrderLineItemId))
                .ForMember(dest => dest.DeliveryDateTime, opt =>
                {
                    opt.PreCondition(src => src.OrderType == "delivery");
                    opt.MapFrom(src => src.FulfillmentEndDateTime);
                })
                .ForMember(dest => dest.PickupDateTime, opt =>
                {
                    opt.PreCondition(src => src.OrderType == "pickup");
                    opt.MapFrom(src => src.FulfillmentEndDateTime);
                })
                .ForMember(dest => dest.PickupOrderDateTime, opt =>
                {
                    opt.PreCondition(src => src.OrderType == "pickup");
                    opt.MapFrom(src => src.OrderHeader.OrderDateTime);
                });

            CreateMap<FulfilledInventory, Data.Models.OrderFulfillmentLineItems>()
                .ForMember(dest => dest.NetSuiteItemId, opt => opt.MapFrom(src => src.Inventory.Item.NetSuiteItemId))
                .ForMember(dest => dest.AssetTag, opt => opt.MapFrom(src => src.Inventory.AssetTagNumber))
                .ForMember(dest => dest.LotNumber, opt => opt.MapFrom(src => src.Inventory.LotNumber))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.Inventory.SerialNumber))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Count));

            CreateMap<APILogs, APILog>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp.UtcDateTime));

            CreateMap<Data.Models.DispatchAuditLog, ViewModels.AuditLog>()
              .ForMember(dest => dest.AuditData, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<IEnumerable<EventEntryChange>>(src.AuditData)));

            CreateMap<ViewModels.NetSuiteLogRequest, NSSDKViewModel.NetSuiteLogRequest>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.FromDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.ToDate));

            CreateMap<ViewModels.NetSuiteDispatchRequest, NSSDKViewModel.NetSuiteDispatchRequest>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.FromDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.ToDate))
                .ForMember(dest => dest.DeliveryStartDate, opt => opt.MapFrom(src => src.DeliveryFromDate))
                .ForMember(dest => dest.DeliveryEndDate, opt => opt.MapFrom(src => src.DeliveryToDate))
                .ForMember(dest => dest.PickupStartDate, opt => opt.MapFrom(src => src.PickUpFromDate))
                .ForMember(dest => dest.PickupEndDate, opt => opt.MapFrom(src => src.PickUpToDate))
                .ForMember(dest => dest.PickupRequestStartDate, opt => opt.MapFrom(src => src.PickUpRequestFromDate))
                .ForMember(dest => dest.PickupRequestEndDate, opt => opt.MapFrom(src => src.PickUpRequestToDate))
                .ForMember(dest => dest.PatientGuid, opt =>
                {
                    opt.PreCondition(src => src.PatientUuid != Guid.Empty);
                    opt.MapFrom(src => src.PatientUuid);
                });

            CreateMap<NSSDKViewModel.NetSuiteHmsDispatch, NetSuiteHmsDispatchRecord>();

            CreateMap<Candidate, Address>()
                .ForMember(dest => dest.AddressUuid, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Metadata.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Metadata.Longitude))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Components.ZipCode))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.DeliveryLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.DeliveryLine2))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Components.State))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Components.CityName))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.Metadata.CountyName))
                .ForMember(dest => dest.Plus4Code, opt => opt.MapFrom(src => src.Components.Plus4Code))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => "United States of America"));

            CreateMap<SmartyStreetsSuggestion, SuggestionResponse>()
                .ForMember(dest => dest.AddressKey, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.AddressUuid, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Entries.ToString()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.StreetLine))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Secondary))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => int.Parse(src.Zipcode)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => "United States of America"));

            CreateMap<UserRoles, HospiceMemberRoleRequest>();

            CreateMap<ServiceArea, SiteServiceAreas>();

            CreateMap<NSSDKViewModel.ZonePaginatedList<NSSDKViewModel.Subscription>, PaginatedList<ViewModels.Subscription>>();

            CreateMap<NSSDKViewModel.Subscription, Data.Models.Subscriptions>()
                 .ForMember(dest => dest.NetSuiteSubscriptionId, opt => opt.MapFrom(src => src.NetSuiteSubscriptionId.Value))
                 .ForMember(dest => dest.NetSuiteCustomerId, opt => opt.MapFrom(src => src.NetSuiteCustomerId.Value))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                 .ForMember(dest => dest.NetSuiteBillToCustomerId, opt => opt.MapFrom(src => src.BillToCustomer.Value))
                 .ForMember(dest => dest.BillToCustomer, opt => opt.MapFrom(src => src.BillToCustomer.Text))
                 .ForMember(dest => dest.BillToEntity, opt => opt.MapFrom(src => src.BillToEntity.Text))
                 .ForMember(dest => dest.NetSuiteBillToEntityId, opt => opt.MapFrom(src => src.BillToEntity.Value))
                 .ForMember(dest => dest.ConsolidateBilling, opt => opt.MapFrom(src => src.ConsolidateBilling.Value))
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.StartDate.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndDate.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.NetSuiteCurrencyId, opt => opt.MapFrom(src => src.Currency.Value))
                 .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Text))
                 .ForMember(dest => dest.NetSuiteBillingProfileId, opt => opt.MapFrom(src => src.BillingProfile.Value))
                 .ForMember(dest => dest.BillingProfile, opt => opt.MapFrom(src => src.BillingProfile.Text))
                 .ForMember(dest => dest.NetSuiteChargeScheduleId, opt => opt.MapFrom(src => src.ChargeSchedule.Value))
                 .ForMember(dest => dest.ChargeSchedule, opt => opt.MapFrom(src => src.ChargeSchedule.Text))
                 .ForMember(dest => dest.InheritChargeScheduleFromMasterContract, opt => opt.MapFrom(src => src.InheritChargeScheduleFromMasterContract.Value))
                 .ForMember(dest => dest.NetSuiteRenewalTemplateId, opt => opt.MapFrom(src => src.RenewalTemplate.Value))
                 .ForMember(dest => dest.RenewalTemplate, opt => opt.MapFrom(src => src.RenewalTemplate.Text))
                 .ForMember(dest => dest.NetSuiteEnableLineItemShippingId, opt => opt.MapFrom(src => src.EnableLineItemShipping.Value))
                 .ForMember(dest => dest.EnableLineItemShipping, opt => opt.MapFrom(src => src.EnableLineItemShipping.Text))
                 .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => Convert.ToDateTime(src.Created.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.LastModifiedDateTime, opt => opt.MapFrom(src => Convert.ToDateTime(src.LastModified.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.IsInactive, opt => opt.MapFrom(src => src.IsInactive.Value));

            CreateMap<Data.Models.Subscriptions, ViewModels.Subscription>();

            CreateMap<NSSDKViewModel.ZonePaginatedList<NSSDKViewModel.SubscriptionItem>, PaginatedList<Data.Models.SubscriptionItems>>();

            CreateMap<NSSDKViewModel.SubscriptionItem, Data.Models.SubscriptionItems>()
                .ForMember(dest => dest.NetSuiteSubscriptionItemId, opt => opt.MapFrom(src => src.NetSuiteSubscriptionItemId.Value))
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value)).ForMember(dest => dest.NetSuiteSubscriptionId, opt => opt.MapFrom(src => src.Subscription.Value))
                 .ForMember(dest => dest.NetSuiteSubscriptionId, opt => opt.MapFrom(src => src.Subscription.Value))
                 .ForMember(dest => dest.Subscription, opt => opt.Ignore())
                 .ForMember(dest => dest.NetSuiteRateTypeId, opt => opt.MapFrom(src => src.RateType.Value))
                 .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => src.RateType.Text))
                 .ForMember(dest => dest.InvertNegativeQuantity, opt => opt.MapFrom(src => src.InvertNegativeQuantity.Value))
                 .ForMember(dest => dest.UseAlternateTermMultiplier, opt => opt.MapFrom(src => src.UseAlternateTermMultiplier.Value))
                 .ForMember(dest => dest.OverageRate, opt => opt.MapFrom(src => src.OverageRate.Value))
                 .ForMember(dest => dest.NetSuiteRatePlanId, opt => opt.MapFrom(src => src.RatePlan.Value))
                 .ForMember(dest => dest.RatePlan, opt => opt.MapFrom(src => src.RatePlan.Text))
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.StartDate.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => Convert.ToDateTime(src.EndDate.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.BillInArrears, opt => opt.MapFrom(src => src.BillInArrears.Value))
                 .ForMember(dest => dest.RatingScheduleBillInArrears, opt => opt.MapFrom(src => src.RatingScheduleBillInArrears.Value))
                 .ForMember(dest => dest.NetSuiteChargeScheduleUsedId, opt => opt.MapFrom(src => src.ChargeScheduleUsed.Value))
                 .ForMember(dest => dest.ChargeScheduleUsed, opt => opt.MapFrom(src => src.ChargeScheduleUsed.Text))
                 .ForMember(dest => dest.NetSuiteCurrencyId, opt => opt.MapFrom(src => src.Currency.Value))
                 .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Text))
                 .ForMember(dest => dest.NetSuiteCustomerId, opt => opt.MapFrom(src => src.Customer.Value))
                 .ForMember(dest => dest.NetSuiteExcludeChargesFromBillingWhenId, opt => opt.MapFrom(src => src.ExcludeChargesFromBillingWhen.Value))
                 .ForMember(dest => dest.ExcludeChargesFromBillingWhen, opt => opt.MapFrom(src => src.ExcludeChargesFromBillingWhen.Text))
                 .ForMember(dest => dest.ExcludeFromOrderLevelMinimumCommitment, opt => opt.MapFrom(src => src.ExcludeFromOrderLevelMinimumCommitment.Value))
                 .ForMember(dest => dest.NetSuiteInheritChargeScheduleFromId, opt => opt.MapFrom(src => src.InheritChargeScheduleFrom.Value))
                 .ForMember(dest => dest.InheritChargeScheduleFrom, opt => opt.MapFrom(src => src.InheritChargeScheduleFrom.Text))
                 .ForMember(dest => dest.NetSuiteItemId, opt => opt.MapFrom(src => src.Item.Value))
                 .ForMember(dest => dest.Item, opt => opt.Ignore())
                 .ForMember(dest => dest.ItemDescription, opt => opt.MapFrom(src => src.ItemDescription.Text))
                 .ForMember(dest => dest.NetSuiteProRationTypeId, opt => opt.MapFrom(src => src.ProRationType.Value))
                 .ForMember(dest => dest.ProRationType, opt => opt.MapFrom(src => src.ProRationType.Text))
                 .ForMember(dest => dest.RatingPriority, opt => opt.MapFrom(src => src.RatingPriority.Value))
                 .ForMember(dest => dest.ChargeIncludedUnits, opt => opt.MapFrom(src => src.ChargeIncludedUnits.Value))
                 .ForMember(dest => dest.CreditRebillPriorNewCountCharges, opt => opt.MapFrom(src => src.CreditRebillPriorNewCountCharges.Value))
                 .ForMember(dest => dest.PushFutureCountCharges, opt => opt.MapFrom(src => src.PushFutureCountCharges.Value))
                 .ForMember(dest => dest.AdjustmentExcludePriorToEffectiveDate, opt => opt.MapFrom(src => src.AdjustmentExcludePriorToEffectiveDate.Value))
                 .ForMember(dest => dest.ApplyAdjustmentsRetroactively, opt => opt.MapFrom(src => src.ApplyAdjustmentsRetroactively.Value))
                 .ForMember(dest => dest.NetSuiteRenewalItemId, opt => opt.MapFrom(src => src.RenewalItem.Value))
                 .ForMember(dest => dest.RenewalItem, opt => opt.MapFrom(src => src.RenewalItem.Text))
                 .ForMember(dest => dest.ApplyPrepaidToAllSubscriptionItems, opt => opt.MapFrom(src => src.ApplyPrepaidToAllSubscriptionItems.Value))
                 .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => Convert.ToDateTime(src.Created.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.LastModifiedDatetime, opt => opt.MapFrom(src => Convert.ToDateTime(src.LastModified.Value, new CultureInfo("en-US"))))
                 .ForMember(dest => dest.IsInactive, opt => opt.MapFrom(src => src.IsInactive.Value));

            CreateMap<Data.Models.Hms2Contracts, ViewModels.Hms2Contract>();

            CreateMap<Data.Models.Hms2ContractItems, ViewModels.Hms2ContractItem>();

            CreateMap<OrderHeaderRequest, OrderHeaders>()
                 .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.OrderStatusId));

            CreateMap<CoreOrderLineItemRequest, OrderLineItems>()
                .ForMember(dest => dest.Action, opt => opt.Ignore())
                .ForMember(dest => dest.EquipmentSettings, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.EquipmentSettings)));

            CreateMap<CoreAddressRequest, Addresses>();
            CreateMap<OrderNotesRequest, Data.Models.OrderNotes>();
            CreateMap<UpdateOrderNotesRequest, OrderNotesRequest>().ReverseMap();


            CreateMap<Data.Models.SubscriptionItems, ViewModels.SubscriptionItem>();

            CreateMap<Data.Models.Features, ViewModels.Feature>().ReverseMap();

            CreateMap<ViewModels.DispatchRecordUpdateRequest, NSSDKViewModel.DispatchRecordUpdateValues>()
                .ReverseMap();

            CreateMap<NSSDKViewModel.DispatchRecordUpdateRequest, ViewModels.DispatchRecordUpdateRequest>()
                .IncludeMembers(dest => dest.Values)
                .ReverseMap();

            CreateMap<ViewModels.PatientInventoryRequest, ViewModels.PatientInventory>();

            CreateMap<NSViewModel.NSInventoryRequest, Data.Models.Inventory>();
            CreateMap<Data.Models.Inventory, NSViewModel.NSInventoryResponse>();
            CreateMap<NSViewModel.NSInventoryRequest, NSViewModel.NSInventory>();

            CreateMap<MergePatientBaseRequest, MergePatientRequest>().ReverseMap();

            CreateMap<NSViewModel.NSContractRecordRequest, Data.Models.ContractRecords>();

            CreateMap<Data.Models.ContractRecords, ViewModels.ContractRecord>();

            CreateMap<Data.Models.ContractRecords, ViewModels.CatalogItem>()
                .ForMember(dest => dest.ItemImageUrls, opt =>
                {
                    opt.PreCondition(src => src.Item != null);
                    opt.MapFrom(src => src.Item.ItemImages.Select(i => i.Url));
                })
                .ForMember(dest => dest.EquipmentSettingFields, opt =>
                {
                    opt.PreCondition(src => src.Item != null);
                    opt.MapFrom(src => src.Item.EquipmentSettingsConfig.Select(e => e.EquipmentSettingType.Name));
                });

            CreateMap<Data.Models.Hms2ContractItems, ViewModels.CatalogItem>()
                .ForMember(dest => dest.ItemImageUrls, opt =>
                {
                    opt.PreCondition(src => src.Item != null);
                    opt.MapFrom(src => src.Item.ItemImages.Select(i => i.Url));
                })
                .ForMember(dest => dest.EquipmentSettingFields, opt =>
                {
                    opt.PreCondition(src => src.Item != null);
                    opt.MapFrom(src => src.Item.EquipmentSettingsConfig.Select(e => e.EquipmentSettingType.Name));
                })
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.SalePrice > 0 ? src.SalePrice : src.RentalPrice));

            CreateMap<Common.ViewModels.AuditLogAzureTable, ViewModels.AuditLog>()
            .ForMember(dest => dest.AuditData, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<IEnumerable<EventEntryChange>>(src.AuditEvent)));

            CreateMap<Data.Models.OrderHeaders, OrderNotification>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => GetOrderStatus(src.StatusId)))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderLineItems))
                .ForMember(dest => dest.HospiceLocationName, opt =>
                {
                    opt.PreCondition(src => src.HospiceLocation != null);
                    opt.MapFrom(src => src.HospiceLocation.Name);
                });
            CreateMap<Data.Models.OrderLineItems, OrderItem>()
                 .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.ItemCount))
                 .ForMember(dest => dest.ItemName, opt =>
                 {
                     opt.PreCondition(src => src.Item != null);
                     opt.MapFrom(src => src.Item.Name);
                 });
            CreateMap<Data.Models.OrderNotes, NotificationSDK.ViewModels.OrderNote>();

            CreateMap<NetSuiteTransferOrder, TransferOrder>();

            CreateMap<NetSuiteInventoryLineItem, InventoryLineItem>()
                .ReverseMap();

            CreateMap<TransferOrderCreateRequest, NetSuiteTransferOrder>();

            CreateMap<TOrderFulfillReceiveRequest, NetSuiteTOFulfillReceiveRequest>();

            CreateMap<InventoryRequest, NetSuiteInventory>();

            CreateMap<NetSuiteInventoryResponse, NSUpdateInventoryResponse>();

            CreateMap<NSInventoryLineRequest, NetSuiteInventoryLineRequest>();

            CreateMap<NSInventoryItem, NetSuiteInventory>();

            CreateMap<InventoryItemResponse, NSAddInventoryResponse>();

            CreateMap<TblContracts, Data.Models.Hms2Contracts>()
                .ForMember(dest => dest.HospiceId, opt => opt.Ignore())
                .ForMember(dest => dest.Hms2ContractId, opt => opt.MapFrom(src => src.ContractId))
                .ForMember(dest => dest.Hms2HospiceId, opt => opt.MapFrom(src => src.HospiceId))
                .ForMember(dest => dest.Hms2CustomerId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<ContractInventory, Data.Models.Hms2ContractItems>()
                .ForMember(dest => dest.ContractId, opt => opt.Ignore())
                .ForMember(dest => dest.Hms2ContractId, opt => opt.MapFrom(src => src.ContractId))
                .ForMember(dest => dest.Hms2ContractItemId, opt => opt.MapFrom(src => src.InvctrId))
                .ForMember(dest => dest.Hms2ItemId, opt => opt.MapFrom(src => src.InventoryId))
                .ForMember(dest => dest.IsPerDiem, opt => opt.MapFrom(src => src.Perdiem))
                .ForMember(dest => dest.ShowOnOrderScreen, opt => opt.MapFrom(src => src.OrderScreen));
        }

        private string GetPhoneNumberWithoutCountryCode(string phoneNumberString)
        {
            if (!string.IsNullOrEmpty(phoneNumberString))
            {
                return phoneNumberString.Substring(phoneNumberString.Length - 10);
            }
            return "0";
        }

        private string GetCountryCodeFromPhoneNumber(string phoneNumberString)
        {
            if (!string.IsNullOrEmpty(phoneNumberString))
            {
                var countryCode = phoneNumberString.Substring(1, phoneNumberString.Length - 11);
                if (string.IsNullOrEmpty(countryCode))
                {
                    return "0";
                }
                return countryCode;
            }
            return "0";
        }

        private int GetOrderTypeId(string orderTypeString)
        {
            if (Enum.TryParse(orderTypeString, true, out Data.Enums.OrderTypes orderType))
            {
                return (int)orderType;
            }
            return -1;
        }

        private int GetOrderStatusId(string orderStatusString)
        {
            if (Enum.TryParse(orderStatusString, true, out Data.Enums.OrderHeaderStatusTypes orderStatus))
            {
                return (int)orderStatus;
            }
            return -1;
        }

        private string GetAttributeValue(List<AttributeType> attributes, string key)
        {
            var userAttribute = attributes.FirstOrDefault(a => string.Equals(a.Name, key, StringComparison.OrdinalIgnoreCase));
            if (userAttribute != null)
            {
                return userAttribute.Value;
            }
            return "";
        }

        private string GetItemNameFromOrderLine(OrderLineItems orderLine)
        {
            if (orderLine == null || orderLine.Item == null)
            {
                return null;
            }

            return orderLine.Item.Name;
        }

        private string GetDriverName(Drivers driver)
        {
            if (driver == null || driver.User == null)
            {
                return null;
            }

            return $"{driver.User.FirstName} {driver.User.LastName}";
        }

        private string GetOrderStatus(int? statusId)
        {
            if (statusId != null && Enum.IsDefined(typeof(Data.Enums.OrderHeaderStatusTypes), statusId))
            {
                var status = ((Data.Enums.OrderHeaderStatusTypes)statusId).ToString();
                return status.Replace('_', ' ');
            }
            return null;
        }

        private string GetOrderLineItemStatus(int? statusId)
        {
            if (statusId != null && Enum.IsDefined(typeof(Data.Enums.OrderLineItemStatusTypes), statusId))
            {
                var status = ((Data.Enums.OrderLineItemStatusTypes)statusId).ToString();
                return status.Replace('_', ' ');
            }
            return null;
        }
    }
}