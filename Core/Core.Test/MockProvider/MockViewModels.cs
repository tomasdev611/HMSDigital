using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using NSViewModel = HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Test.MockProvider
{
    public class MockViewModels
    {
        public IEnumerable<User> Users { get; set; }

        public NSViewModel.ZonePaginatedList<NSViewModel.Subscription> Subscriptions { get; set; }

        public NSViewModel.ZonePaginatedList<NSViewModel.SubscriptionItem> SubscriptionItems { get; set; }

        public MockViewModels()
        {
            Users = GetUsers();
            Subscriptions = GetSubscriptions();
            SubscriptionItems = GetSubscriptionItems();
        }
        public IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                GetUser("test"),
                GetUser("hospiceMember"),
                GetUser("LoggedInUser")
            };
        }
        public User GetUser(string userId)
        {
            return new User()
            {
                FirstName = "test",
                LastName = "test",
                UserId = userId,
                Email = "abc@test.com",
            };
        }

        public Role GetRole()
        {
            return new Role()
            {
                Id = 2,
                Level = 1,
                Name = "test",
                Permissions = GetvalidPermissions()
            };
        }

        public IEnumerable<string> GetInvalidPermissions()
        {
            return new List<string>()
            {
                "invalidPermission:invalidAction"
            };
        }

        public IEnumerable<string> GetvalidPermissions()
        {
            return new List<string>()
            {
                "testPermission:testAction"
            };
        }

        public UserCreateRequest GetUserCreateRequest()
        {
            return new UserCreateRequest()
            {
                Email = "test@abc.com",
                FirstName = "test",
                LastName = "user"
            };
        }

        public List<UserCreateRequest> GetBulkUserCreateRequest()
        {
            return new List<UserCreateRequest>()
            {
                new UserCreateRequest()
                {
                    Email = "test@abc.com",
                    FirstName = "test",
                    LastName = "user"
                },
                new UserCreateRequest()
                {
                    Email = "test.user12@test.com",
                    FirstName = "existing",
                    LastName = "user",
                    UserRoles = new List<UserRoleBase>()
                    {
                        new UserRoleBase()
                        {
                            ResourceId = "*",
                            ResourceType = "Hospice",
                            RoleId = 1
                        }
                    }
                }
            };
        }

        public UserSiteUpdateRequest GetSiteUpdateRequest()
        {
            return new UserSiteUpdateRequest()
            {
                DefaultSiteId = 1,
                SiteIds = new List<int>() { 1 }
            };
        }

        public UserMinimal GetUsersUpdateRequest()
        {
            return new UserMinimal()
            {
                Email = "test@abc.com",
                FirstName = "test",
                LastName = "user"
            };
        }
        

        public Address GetAddress()
        {
            return new Address()
            {
                AddressLine1 = "line 1",
                AddressLine2 = "line 2",
                City = "testCity",
                State = "testState",
                ZipCode = 12345
            };
        }

        public CoreAddressRequest GetCoreAddress()
        {
            return new CoreAddressRequest()
            {
                AddressLine1 = "line 1",
                AddressLine2 = "line 2",
                City = "testCity",
                State = "testState",
                ZipCode = 12345,
                AddressUuid = new Guid()
            };
        }

        public NSHospiceRequest GetHospiceRequest()
        {
            return new NSHospiceRequest()
            {
                Name = "CreatedHospice",
                NetSuiteCustomerId = 1001,
                InternalWarehouseId = 2001,
                Email = "hospice@abc.com",
                NetSuiteContractingCustomerId = 1001,
                CreatedByUserEmail = "test@abc.com",
                Address = GetHospiceAddress(),
                Locations = new List<HospiceLocationCreateRequest>()
                {
                    GetHospiceLocationRequest(1002, "test location1"),
                    GetHospiceLocationRequest(1003, "test location2")
                },
                CustomerType = "Direct"
            };
        }

        public NSHospiceRequest GetUpdateHospiceRequest()
        {
            return new NSHospiceRequest()
            {
                Name = "UpdateHospice",
                NetSuiteCustomerId = 1001,
                InternalWarehouseId = 2001,
                Email = "hospice@abc.com",
                NetSuiteContractingCustomerId = 1001,
                CreatedByUserEmail = "test@abc.com",
                Address = GetHospiceAddress(),
                Locations = new List<HospiceLocationCreateRequest>()
                {
                    GetHospiceLocationRequest(1004, "test location3"),
                    GetHospiceLocationRequest(1005, "test location4")
                },
                CustomerType = "Direct"
            };
        }

        public HospiceLocationCreateRequest GetHospiceLocationRequest(int netSuiteCustomerId, string name)
        {
            return new HospiceLocationCreateRequest()
            {
                Name = name,
                Address = GetHospiceAddress(),
                NetSuiteCustomerId = netSuiteCustomerId,
                NetSuiteContractingCustomerId = 1001,
                InternalWarehouseId = 2001
            };
        }


        public CsvMapping<InputMappedItem> GetInputCsvMappingRequest()
        {
            return new CsvMapping<InputMappedItem>()
            {
                ColumnNameMappings = new Dictionary<string, InputMappedItem>()
                {
                    {"firstName", new InputMappedItem(){ IsRequired = true, Name = "First Name", Type = "string", ColumnOrder = 1} },
                    {"lastName", new InputMappedItem(){ IsRequired = true, Name = "Last Name", Type = "string", ColumnOrder = 2} },
                }
            };
        }

        public CsvMapping<OutputMappedItem> GetOutputCsvMappingRequest()
        {
            return new CsvMapping<OutputMappedItem>()
            {
                ColumnNameMappings = new Dictionary<string, OutputMappedItem>()
                {
                    {"name", new OutputMappedItem(){ Name = "Name", Type = "string", ColumnOrder = 1, ShowInDisplay = true, IncludeInExport = true} },
                    {"hospiceLocationName", new OutputMappedItem(){ Name = "Hospice Location", Type = "string", ColumnOrder = 2, ShowInDisplay = true, IncludeInExport = true} }
                }
            };
        }

        public NSViewModel.ZonePaginatedList<NSViewModel.Subscription> GetSubscriptions()
        {
            return new NSViewModel.ZonePaginatedList<NSViewModel.Subscription>()
            {
                Records = new List<NSViewModel.Subscription>()
                {
                    new NSViewModel.Subscription()
                    {
                        Name = new NSViewModel.ValueObj<string>(){ Value = "Subscription1" },
                        NetSuiteCustomerId = new NSViewModel.ValueObj<string>(){ Value = "1234" },
                        NetSuiteSubscriptionId = new NSViewModel.ValueObj<string>(){ Value = "1234" }
                    },
                    new NSViewModel.Subscription()
                    {
                        Name = new NSViewModel.ValueObj<string>(){ Value = "Subscription2" },
                        NetSuiteCustomerId = new NSViewModel.ValueObj<string>(){ Value = "1234" },
                        NetSuiteSubscriptionId = new NSViewModel.ValueObj<string>(){ Value = "1235" }
                    }
                }
            };
        }

        public NSViewModel.ZonePaginatedList<NSViewModel.SubscriptionItem> GetSubscriptionItems()
        {
            return new NSViewModel.ZonePaginatedList<NSViewModel.SubscriptionItem>()
            {
                Records = new List<NSViewModel.SubscriptionItem>()
                {
                    new NSViewModel.SubscriptionItem()
                    {
                        Name = new NSViewModel.ValueObj<string>(){ Value = "ItemName_5" },
                        NetSuiteSubscriptionItemId = new NSViewModel.ValueObj<string>(){ Value = "5" },
                        Subscription = new NSViewModel.ValueTextObj<string>(){ Value = "1234" }
                    },
                    new NSViewModel.SubscriptionItem()
                    {
                        Name = new NSViewModel.ValueObj<string>(){ Value = "ItemName_4" },
                        NetSuiteSubscriptionItemId = new NSViewModel.ValueObj<string>(){ Value = "4" },
                        Subscription = new NSViewModel.ValueTextObj<string>(){ Value = "1234" }
                    }
                }
            };
        }

        public NSContractRecordRequest GetNSContractRecordRequest(int netSuiteCustomerId, int itemId, double rate)
        {
            return new NSContractRecordRequest()
            {
                NetSuiteCustomerId = netSuiteCustomerId,
                NetSuiteRelatedItemId = itemId,
                Rate = rate,
                NetSuiteContractRecordId = 1001,
                NetSuiteSubscriptionId = 1,
                EffectiveStartDate = DateTime.UtcNow.AddMonths(2),
                EffectiveEndDate = DateTime.UtcNow,
                CreatedByUserEmail = "test@abc.com"
            };
        }
        public AddressRequest GetHospiceAddress()
        {
            return new AddressRequest()
            {
                AddressLine1 = "line 1",
                AddressLine2 = "line 2",
                City = "testCity",
                State = "testState",
                ZipCode = 12345
            };
        }

        public HospiceMemberRequest GetHospiceMemberRequest()
        {
            return new HospiceMemberRequest()
            {
                Email = "hospice.member@abc.com",
                FirstName = "hospice",
                LastName = "member",
                PhoneNumber = 1234567890,
                CountryCode = 1,
                UserRoles = new List<HospiceMemberRoleRequest>() { new HospiceMemberRoleRequest() { RoleId = 2, ResourceType = "Hospice", ResourceId = 1 } }
            };
        }

        public HospiceMemberRequest GetUpdateHospiceMemberRequest()
        {
            return new HospiceMemberRequest()
            {
                Email = "test@abc.com",
                FirstName = "Updated",
                LastName = "user",
                PhoneNumber = 1234567890,
                CountryCode = 1,
                UserRoles = new List<HospiceMemberRoleRequest>(){new HospiceMemberRoleRequest()
                {
                    RoleId = 6,
                    ResourceId = 2,
                    ResourceType = HMSDigital.Core.Data.Enums.ResourceTypes.HospiceLocation.ToString()
                } }
            };
        }

        public FacilityRequest GetFacilityRequest()
        {
            return new FacilityRequest()
            {
                Name = "Create Facility",
                HospiceId = 1,
                Address = GetAddress(),
                HospiceLocationId = 1
            };
        }

        public JsonPatchDocument<Facility> GetFacilityJsonPatchDocument()
        {
            JsonPatchDocument<Facility> patch = new JsonPatchDocument<Facility>();
            patch.Replace(e => e.Name, "UpdatedFacility");
            return patch;
        }

        public FacilityPatientRequest GetFacilityPatientRequest()
        {
            return new FacilityPatientRequest()
            {
                FacilityPatientRooms = new List<FacilityPatientRoom>() { new FacilityPatientRoom() { FacilityId = 1, PatientRoomNumber = "A1234" } },
                PatientUuid = new Guid("16142463-865c-4db3-86d6-08809ece186b")
            };
        }

        public ItemRequest GetItemRequest()
        {
            return new ItemRequest()
            {
                Name = "Create Item",
                Description = "Test create item",
                ItemNumber = "CT1001",
                CategoryId = 1
            };
        }

        public JsonPatchDocument<Item> GetItemJsonPatchDocument()
        {
            JsonPatchDocument<Item> patch = new JsonPatchDocument<Item>();
            patch.Replace(e => e.Name, "UpdatedItem");
            return patch;
        }

        public NSItemRequest GetNSItemRequest(int id, bool isAssetTagged, bool isSerialized, bool isLotNumbered)
        {
            return new NSItemRequest()
            {
                Name = "Upsert Item",
                ItemNumber = $"Upsert_{id}",
                NetSuiteItemId = id,
                Categories = new List<NSItemCategory>()
                {
                    new NSItemCategory()
                    {
                        Name = "Item Category",
                        NetSuiteCategoryId = 2020,
                        Categories = new List<NSItemSubCategory>()
                        {
                            new NSItemSubCategory()
                            {
                                Name = "Item Sub category",
                                NetSuiteSubCategoryId = 2020
                            }
                        }
                    }
                },
                IsAssetTagged = isAssetTagged,
                IsSerialized = isSerialized,
                IsLotNumbered = isLotNumbered,
                IsConsumable = true,
                Inventory = new List<NSInventory>()
                {
                    GetNSInventory(id, isAssetTagged,isSerialized,isLotNumbered)
                }
            };
        }

        public NSInventory GetNSInventory(int id, bool isAssetTagged, bool isSerialized, bool isLotNumbered)
        {
            return new NSInventory()
            {
                AssetTagNumber = isAssetTagged ? $"Asset{id}" : string.Empty,
                SerialNumber = isSerialized ? $"Serial{id}" : string.Empty,
                LotNumber = isLotNumbered ? $"Lot{id}" : string.Empty,
                NetSuiteInventoryId = id,
                QuantityAvailable = 1,
                QuantityOnHand = 1,
                NetSuiteLocationId = 2001
            };
        }

        public DispatchAssignmentRequest GetDispatchAssignmentRequest(int? orderHeaderId, int? transferRequestId = null, int vehicleId = 100)
        {
            var dispatchInstructionRequest = new DispatchInstructionRequest
            {
                DispatchStartDateTime = DateTime.Now,
                DispatchEndDateTime = DateTime.Now.AddHours(2)
            };


            var dispatchAssignmentRequest = new DispatchAssignmentRequest
            {
                VehicleId = vehicleId,
                DispatchDetails = new List<DispatchInstructionRequest>()
            };
            if (orderHeaderId.HasValue)
            {
                dispatchInstructionRequest.OrderHeaderId = orderHeaderId;
            }
            if (transferRequestId.HasValue)
            {
                dispatchInstructionRequest.TransferRequestId = transferRequestId;
            }

            dispatchAssignmentRequest.DispatchDetails = dispatchAssignmentRequest.DispatchDetails.Append(dispatchInstructionRequest);
            return dispatchAssignmentRequest;
        }
        public ItemTransferCreateRequest GetItemTransferCreateRequest(int sourceLocationId, int destinationLocationId, int count, int siteMemberId)
        {
            return new ItemTransferCreateRequest()
            {
                DestinationLocationId = destinationLocationId,
                SourceLocationId = sourceLocationId,
                ItemCount = count,
                DestinationSiteMemberId = siteMemberId
            };
        }

        public ItemImageFileRequest GetItemImageFileRequest(int id)
        {
            return new ItemImageFileRequest()
            {
                ContentType = "jpeg",
                Description = "item image description",
                IsPublic = true,
                ItemId = id,
                Name = "item image",
                SizeInBytes = 10
            };
        }

        public UserPictureFileRequest GetUserPictureFileRequest()
        {
            return new UserPictureFileRequest()
            {
                ContentType = "jpeg",
                Description = "item image description",
                IsPublic = true,
                Name = "user profile image",
                SizeInBytes = 10
            };
        }

        public List<UpdateOrderNotesRequest> GetOrderNotesRequests()
        {
            return new List<UpdateOrderNotesRequest>()
            {
                new UpdateOrderNotesRequest()
                {
                    Note = "Test",
                    HospiceMemberId = 1
                }
            };
        }

    }
}
