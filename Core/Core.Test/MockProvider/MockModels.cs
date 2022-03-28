using HMSDigital.Core.Data.Models;
using NSSDKViewModels = HospiceSource.Digital.NetSuite.SDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Enums = HMSDigital.Core.Data.Enums;
namespace Core.Test.MockProvider
{

    public class MockModels
    {
        public List<Users> Users { get; set; }

        public Users LoggedInUser { get; set; }

        public List<Roles> Roles { get; set; }

        public List<PermissionNouns> PermissionNouns { get; set; }

        public List<PermissionVerbs> PermissionVerbs { get; set; }

        public List<Sites> Sites { get; set; }

        public List<HospiceLocations> HospiceLocations { get; set; }

        public List<Facilities> Facilities { get; set; }

        public List<Hospices> Hospices { get; set; }

        public List<OrderHeaders> OrderHeaders { get; set; }

        public List<OrderLineItems> OrderLineItems { get; set; }

        public List<HospiceMember> HospiceMembers { get; set; }

        public List<FacilityPatient> FacilityPatients { get; set; }

        public List<FacilityPatientHistory> FacilityPatientHistories { get; set; }

        public List<Inventory> Inventories { get; set; }

        public List<PatientInventory> PatientInventories { get; set; }

        public List<Items> Items { get; set; }

        public List<Drivers> Drivers { get; set; }

        public List<CsvMappings> CsvMappings { get; set; }

        public List<ItemTransferRequests> ItemTransferRequests { get; set; }

        public List<Addresses> Addresses { get; set; }

        public List<SiteMembers> SiteMembers { get; set; }

        public List<DispatchInstructions> DispatchInstructions { get; set; }

        public List<OrderFulfillmentLineItems> OrderFulfillmentLineItems { get; set; }

        public List<SiteServiceAreas> SiteServiceAreas { get; set; }

        public List<HospiceLocationMembers> HospiceLocationMembers { get; set; }

        public List<Subscriptions> Subscriptions { get; set; }

        public List<SubscriptionItems> SubscriptionItems { get; set; }

        public List<OrderTypes> OrderTypes { get; set; }

        public List<NSSDKViewModels.InventoryMovementRequest> InventoryMovementRequests { get; set; }

        public List<NSSDKViewModels.ConfirmFulfilmentRequest> ConfirmFulfilmentRequests { get; set; }

        public List<CreditHoldHistory> CreditHoldHistories { get; set; }

        public List<ContractRecords> ContractRecords { get; set; }

        public List<ItemCategories> ItemCategories { get; set; }

        public List<ItemSubCategories> ItemSubCategories { get; set; }

        public List<NSSDKViewModels.NetSuiteHmsDispatch> DispatchRecords { get; set; }

        public List<ItemImageFiles> ItemImageFiles { get; set; }

        public List<ItemImages> ItemImages { get; set; }

        public List<UserProfilePicture> UserProfilePictures { get; set; }

        public List<NSSDKViewModels.CustomerContactBase> CustomerContactRequests { get; set; }

        public List<Hms2Contracts> Hms2Contracts { get; set; }

        public List<Hms2ContractItems> Hms2ContractItems { get; set; }

        public MockModels()
        {
            LoggedInUser = GetLoggedInUser();
            Users = GetUsers();
            Roles = GetRolesList();
            PermissionNouns = GetPermissionNouns();
            PermissionVerbs = GetPermissionVerbs();
            Sites = GetSites();
            HospiceLocations = GetHospiceLocations();
            Facilities = GetFacilities();
            Hospices = GetHospices();
            OrderHeaders = GetOrderHeaders();
            OrderLineItems = GetOrderLineItems();
            HospiceMembers = GetHospiceMembers();
            FacilityPatients = GetFacilityPatients();
            FacilityPatientHistories = GetFacilityPatientsHistory();
            Inventories = GetInventories();
            PatientInventories = new List<PatientInventory>() { GetPatientInventory() };
            Items = GetItems();
            Drivers = GetDrivers();
            OrderTypes = GetOrderTypes();
            Subscriptions = GetSubscriptions();
            SubscriptionItems = GetSubscriptionItems();
            CsvMappings = new List<CsvMappings>();
            ItemTransferRequests = new List<ItemTransferRequests>();
            Addresses = new List<Addresses>() { GetAddresses() };
            SiteMembers = GetMembers();
            DispatchInstructions = new List<DispatchInstructions>();
            OrderFulfillmentLineItems = GetOrderFulfillments();
            SiteServiceAreas = new List<SiteServiceAreas>();
            HospiceLocationMembers = GetHospiceLocationMembers();
            InventoryMovementRequests = new List<NSSDKViewModels.InventoryMovementRequest>();
            ConfirmFulfilmentRequests = new List<NSSDKViewModels.ConfirmFulfilmentRequest>();
            CreditHoldHistories = new List<CreditHoldHistory>();
            ContractRecords = GetContractRecords();
            ItemCategories = GetItemCategories();
            ItemSubCategories = GetItemSubCategories();
            DispatchRecords = GetDispatchRecords();
            ItemImageFiles = new List<ItemImageFiles>();
            ItemImages = GetItemImages();
            UserProfilePictures = GetUserProfilePictures();
            CustomerContactRequests = new List<NSSDKViewModels.CustomerContactBase>();
            Hms2Contracts = new List<Hms2Contracts>();
            Hms2ContractItems = new List<Hms2ContractItems>();
        }

        public List<OrderHeaders> GetOrderHeaders()
        {
            return new List<OrderHeaders>()
            {
                new OrderHeaders()
                {
                    Id = 1,
                    OrderLineItems = GetOrderLineItems() as ICollection<OrderLineItems>,
                    HospiceId = 1,
                    HospiceLocationId = 1,
                    OrderTypeId = (int)Enums.OrderTypes.Delivery,
                    StatusId = (int)Enums.OrderHeaderStatusTypes.Planned,
                    SiteId = 1,
                    Site = new Sites()
                    {
                        Id = 10,
                        NetSuiteLocationId = 10010
                    },
                    PatientUuid = Guid.NewGuid(),
                    OrderRecipientUserId = 1,
                    CreatedByUserId = 1234,
                    UpdatedByUserId = 13
                },
                new OrderHeaders()
                {
                    Id = 2,
                    OrderLineItems = GetOrderLineItems() as ICollection<OrderLineItems>,
                    HospiceId = 2,
                    HospiceLocationId = 2,
                    NetSuiteCustomerId = 2,
                    OrderTypeId = (int)Enums.OrderTypes.Delivery,
                    StatusId = (int)Enums.OrderHeaderStatusTypes.Planned,
                    PatientUuid = Guid.NewGuid(),
                    DeliveryAddress = GetAddresses()
                }
            };
        }

        public List<OrderFulfillmentLineItems> GetOrderFulfillments()
        {
            return new List<OrderFulfillmentLineItems>()
            {
                new OrderFulfillmentLineItems()
                {
                    Id = 1,
                    OrderHeaderId = 1,
                    OrderLineItemId = 1,
                }
            };
        }

        public List<SiteServiceAreas> GetSiteServiceAreas()
        {
            return new List<SiteServiceAreas>()
            {
                new SiteServiceAreas()
                {
                    Id = 1,
                    SiteId = 2,
                    ZipCode = 12345,
                }
            };
        }

        public List<OrderLineItems> GetOrderLineItems()
        {
            return new List<OrderLineItems>()
            {
                new OrderLineItems()
                {
                    Id = 1,
                    ItemId = 1,
                    Item = GetItem(1),
                    ItemCount = 1,
                    ActionId = (int)Enums.OrderTypes.Delivery,
                    Action = new OrderTypes()
                    {
                        Name = "Delivery"
                    },
                    StatusId = (int)Enums.OrderHeaderStatusTypes.Planned
                },
                new OrderLineItems()
                {
                    Id = 2,
                    ItemId = 2,
                    Item = GetItem(2),
                    ItemCount = 1,
                    ActionId = (int)Enums.OrderTypes.Delivery,
                    Action = new OrderTypes()
                    {
                        Name = "Delivery"
                    },
                    StatusId = (int)Enums.OrderHeaderStatusTypes.Partial_Fulfillment
                },
                new OrderLineItems()
                {
                    Id = 3,
                    ItemId = 3,
                    Item = GetItem(3),
                    ItemCount = 1,
                    ActionId = (int)Enums.OrderTypes.Delivery,
                    Action = new OrderTypes()
                    {
                        Name = "Delivery"
                    },
                    StatusId = (int)Enums.OrderHeaderStatusTypes.Completed
                }
            };
        }

        public List<Drivers> GetDrivers()
        {
            return new List<Drivers>()
            {
                new Drivers()
                {
                    Id = 1,
                    CurrentVehicleId = 100,
                    CurrentVehicle = new Sites()
                    {
                        Id = 100
                    },
                    User = GetUser(1),
                    UserId = 1
                }
            };
        }

        public List<Sites> GetSites()
        {
            return new List<Sites>()
            {
                new Sites()
                {
                    Id = 1,
                    Name = "test site",
                    SiteCode = 123,
                    Address = GetAddresses(),
                    NetSuiteLocationId = 2001
                },
                new Sites()
                {
                    Id = 2,
                    Name = "test site 2",
                    SiteCode = 223,
                    Address = GetAddresses(),
                    NetSuiteLocationId = 2002
                },
                 new Sites()
                {
                    Id = 10,
                    Name = "patient site",
                    SiteCode = 124,
                    Address = GetAddresses(),
                    NetSuiteLocationId = 999
                },
                 new Sites()
                {
                    Id = 100,
                    Name = "Vehicle",
                    SiteCode = 1240,
                    Address = GetAddresses(),
                    NetSuiteLocationId = 100,
                    LocationType = "vehicle",
                    ParentNetSuiteLocationId = 2001
                }
            };
        }

        public Addresses GetAddresses()
        {
            return new Addresses()
            {
                AddressLine1 = "test",
                City = "TestCity",
                State = "TestState",
                ZipCode = 12345
            };
        }

        public List<Roles> GetRolesList()
        {
            return new List<Roles>()
            {
                GetRole(1, "Internal"),
                GetRole(2, "Hospice"),
                GetRole(3, "DME"),
                GetRole(6, "Hospice", name: "HospiceAdmin"),  // HospiceAdmin role
                new Roles()
                {
                    Id = 7,
                    Name = "test Role",
                    Level = 1,
                    IsStatic = true,
                    RoleType = "Hospice"
                }

            };
        }

        public Roles GetRole(int roleId, string type, string name = "test Role")
        {
            return new Roles()
            {
                Id = roleId,
                Name = name,
                Level = 3,
                IsStatic = true,
                RoleType = type,
                UserRoles = GetUserRoles()
            };
        }

        public List<UserRoles> GetUserRoles()
        {
            return new List<UserRoles>() { GetUserRole(1) };
        }

        public UserRoles GetUserRole(int id)
        {
            return new UserRoles()
            {
                Id = id,
                User = GetUser(id)
            };
        }

        public List<Users> GetUsers()
        {
            return new List<Users>()
            {
                LoggedInUser,
                GetUser(1234),
                GetUser(1),
                GetUser(13,12),
                new Users()
                {
                    Id = 12,
                    FirstName = "existing",
                    LastName = "user",
                    CognitoUserId = "test",
                    Email = $"test.user12@test.com",
                    UserRoles = new List<UserRoles>()
                    {
                        new UserRoles()
                        {
                            Id = 12,
                            RoleId = 7,
                            Role = GetRolesList().FirstOrDefault(r => r.Id == 7),
                            ResourceId = "*",
                            ResourceType = Enums.ResourceTypes.Site.ToString(),
                        }
                    }
                },
                GetUser(2)
            };

        }

        public Users GetLoggedInUser()
        {
            return new Users()
            {
                Id = 10,
                CognitoUserId = "LoggedInUser",
                FirstName = "LoggedIn",
                LastName = "User",
                UserRoles = new List<UserRoles>()
                {
                    new UserRoles()
                    {
                        Role = new Roles()
                        {
                            Level = 3
                        },
                        ResourceId = "*",
                        ResourceType = Enums.ResourceTypes.Site.ToString(),
                        RoleId = 1
                    }
                },
                Email = "loggedinuser@example.com"
            };
        }

        public Users GetUser(int id, int? disableByUserId = null)
        {
            return new Users()
            {
                Id = id,
                FirstName = "test",
                LastName = "User",
                CognitoUserId = "test",
                Email = $"test.user{id}@test.com",
                DisabledByUserId = disableByUserId,
                SiteMembersUser = GetSiteMembers().ToList(),
                UserRoles = new List<UserRoles>()
                {
                    new UserRoles()
                    {
                        Id = id,
                        RoleId = 1,
                        ResourceId = "1",
                        ResourceType = Enums.ResourceTypes.Site.ToString()
                    }
                }
            };
        }
        public List<SiteMembers> GetSiteMembers()
        {
            return new List<SiteMembers>()
            {
                new SiteMembers()
                {
                    SiteId = 1,
                    UserId = 1,
                    Site = new Sites()
                    {
                        Id = 1,
                        Name = "test"
                    }
                }
            };
        }
        public List<PermissionNouns> GetPermissionNouns()
        {
            return new List<PermissionNouns>()
            {
                new PermissionNouns()
                {
                    Id = 1,
                    Name = "testPermission"
                }
            };
        }

        public List<PermissionVerbs> GetPermissionVerbs()
        {
            return new List<PermissionVerbs>()
            {
                new PermissionVerbs()
                {
                    Id = 1,
                    Name = "testAction"
                }
            };
        }

        public List<ContractRecords> GetContractRecords()
        {
            return new List<ContractRecords>()
            {
                new ContractRecords()
                {
                    Id = 1,
                    NetSuiteContractRecordId = 1
                }
            };
        }

        public List<HospiceLocations> GetHospiceLocations()
        {
            return new List<HospiceLocations>()
            {
                new HospiceLocations()
                {
                    HospiceId = 1,
                    Id = 1,
                    Name = "location1"
                },
                new HospiceLocations()
                {
                    HospiceId = 2,
                    Id = 2,
                    Name = "location2",
                    NetSuiteCustomerId = 2,
                    SiteId = 2
                },
                new HospiceLocations()
                {
                    HospiceId = 1235,
                    Id = 2020,
                    Name = "Delete Location",
                    NetSuiteCustomerId = 2020
                }
            };
        }

        public List<Facilities> GetFacilities()
        {
            return new List<Facilities>()
            {
                new Facilities()
                {
                    HospiceId = 1,
                    Id = 1,
                    Name = "facility1",
                    Address = GetAddresses()
                },
                new Facilities()
                {
                    HospiceId = 2,
                    Id = 2,
                    Name = "facility2"
                }
            };
        }

        public List<Hospices> GetHospices()
        {
            return new List<Hospices>()
            {
                new Hospices()
                {
                    Id = 1,
                    Name = "Hospice1",
                    NetSuiteCustomerId = 1,
                    HospiceLocations = GetHospiceLocations()
                },
                new Hospices()
                {
                    Id = 2,
                    Name = "Hospice2"
                },
                new Hospices()
                {
                    Id=1234,
                    Name="Netsuite Hospice",
                    NetSuiteCustomerId = 1234,
                    AssignedSiteId = 1,
                },
                new Hospices()
                {
                    Id=1235,
                    Name="Delete Hospice",
                    NetSuiteCustomerId = 1235,
                    AssignedSiteId = 1,
                }
            };
        }

        public List<HospiceMember> GetHospiceMembers()
        {
            return new List<HospiceMember>()
            {
                new HospiceMember()
                {
                    HospiceId = 1,
                    NetSuiteContactId = 1,
                    UserId = 1,
                    Id = 1,
                    User = GetUser(1)
                },
                 new HospiceMember()
                {
                    HospiceId = 1,
                    UserId = 2,
                    Id = 2,
                    User = GetUser(2),
                    NetSuiteContactId = 2,
                    HospiceLocationMembers = GetHospiceLocationMembers()
                },
                new HospiceMember()
                {
                    HospiceId = 1234,
                    UserId = 1234,
                    Id = 1234,
                    User = GetUser(1234),
                    NetSuiteContactId=1234
                }
            };
        }

        public List<FacilityPatient> GetFacilityPatients()
        {
            return new List<FacilityPatient>()
            {
                new FacilityPatient()
                {
                    Id = 1,
                    FacilityId = 1,
                    PatientUuid = new System.Guid("16142463-865c-4db3-86d6-08809ece186a")
                }
            };
        }

        public List<OrderTypes> GetOrderTypes()
        {
            return new List<OrderTypes>()
            {
                new OrderTypes()
                {
                    Id = 10,
                    Name = "Delivery"

                },
                new OrderTypes()
                {
                    Id = 11,
                    Name = "Exchange"

                },
                new OrderTypes()
                {
                    Id = 12,
                    Name = "Move"

                },
                new OrderTypes()
                {
                    Id = 13,
                    Name = "Respite"

                },
                new OrderTypes()
                {
                    Id = 14,
                    Name = "Pickup"

                }
            };
        }

        public List<FacilityPatientHistory> GetFacilityPatientsHistory()
        {
            return new List<FacilityPatientHistory>()
            {
                new FacilityPatientHistory()
                {
                    Id = 1,
                    FacilityId = 1,
                    PatientUuid = new System.Guid("16142463-865c-4db3-86d6-08809ece186a"),
                    Active = true
                }
            };
        }

        public List<Inventory> GetInventories()
        {
            return new List<Inventory>
            {
                GetInventory(1,1,serialNumber:"123456"),
                GetInventory(2,2,assetTagNumber:"654321"),
                GetInventory(3,3,lotNumber:"212121212121"),
                GetInventory(6,6)
            };
        }

        public Inventory GetInventory(int id, int itemId, string serialNumber = "", string assetTagNumber = "", string lotNumber = "", int locationId = 1)
        {
            var item = GetItems().FirstOrDefault(i => i.Id == itemId);
            return new Inventory
            {
                Id = id,
                ItemId = itemId,
                Item = item,
                SerialNumber = serialNumber,
                AssetTagNumber = assetTagNumber,
                LotNumber = lotNumber,
                NetSuiteInventoryId = id,
                CurrentLocationId = locationId
            };
        }

        public List<Items> GetItems()
        {
            return new List<Items>
            {
                GetItem(1, isSerialized: true),
                GetItem(2, isAssetTagged: true),
                GetItem(3, isLotNumbered: true),
                GetItem(4, true, true),
                GetItem(5),
                GetItem(6),
                GetItem(7)
            };
        }

        public List<Subscriptions> GetSubscriptions()
        {
            return new List<Subscriptions>()
            {
                GetSubscription(1234,1234)
            };
        }

        public List<SubscriptionItems> GetSubscriptionItems()
        {
            return new List<SubscriptionItems>()
            {
                new SubscriptionItems()
                {
                    Id = 1,
                    SubscriptionId = 1234,
                    Name = "ItemName_5",
                    ItemId = 5,
                    HospiceId = 5,
                    NetSuiteSubscriptionId = 1234,
                    NetSuiteSubscriptionItemId = 5,
                    NetSuiteItemId = 5,
                    NetSuiteCustomerId = 1234
                }
            };
        }

        public Subscriptions GetSubscription(int id, int netSuiteCustomerId)
        {
            return new Subscriptions()
            {
                Id = id,
                HospiceId = id,
                NetSuiteCustomerId = netSuiteCustomerId,
                NetSuiteSubscriptionId = id
            };
        }

        public Items GetItem(int id, bool isSerialized = false, bool isAssetTagged = false, bool isLotNumbered = false)
        {
            return new Items()
            {
                Id = id,
                Name = $"ItemName_{id}",
                Description = $"ItemDescription_{id}",
                ItemNumber = $"ItemNumber_{id}",
                CogsAccountName = $"CogsAccountName_{id}",
                NetSuiteItemId = id,
                IsSerialized = isSerialized,
                IsAssetTagged = isAssetTagged,
                IsLotNumbered = isLotNumbered
            };
        }

        public List<ItemSubCategories> GetItemSubCategories()
        {
            return new List<ItemSubCategories>()
            {
                new ItemSubCategories()
                {
                    CategoryId = 1,
                    Id = 1,
                    Name = "Test Sub category",
                    NetSuiteSubCategoryId = 1
                }
            };
        }

        public List<ItemCategories> GetItemCategories()
        {
            return new List<ItemCategories>()
            {
                new ItemCategories()
                {
                    Id = 1,
                    Name = "Test category",
                    NetSuiteCategoryId = 1
                }
            };
        }

        public List<SiteMembers> GetMembers()
        {
            return new List<SiteMembers>()
            {
                GetSiteMember(1,1),
                GetSiteMember(2,2),
            };
        }

        public SiteMembers GetSiteMember(int id, int siteId)
        {
            return new SiteMembers()
            {
                Id = id,
                SiteId = siteId
            };
        }

        public List<ItemImages> GetItemImages()
        {
            return new List<ItemImages>()
            {
                new ItemImages()
                {
                    Id = 1,
                    ItemId = 6
                }
            };
        }

        public List<UserProfilePicture> GetUserProfilePictures()
        {
            return new List<UserProfilePicture>()
            {
                new UserProfilePicture()
                {
                    Id = 1,
                    UserId = 12,
                    IsUploaded = true,
                    FileMetadata = new FilesMetadata()
                },
                new UserProfilePicture()
                {
                    Id = 2,
                    UserId = 1234,
                    IsUploaded = true,
                    FileMetadata = new FilesMetadata()
                }
            };
        }

        public PatientInventory GetPatientInventory()
        {
            return new PatientInventory()
            {
                Id = 1,
                ItemId = 6,
                Item = GetItems().FirstOrDefault(i => i.Id == 6)
            };
        }

        private List<NSSDKViewModels.NetSuiteHmsDispatch> GetDispatchRecords()
        {
            return new List<NSSDKViewModels.NetSuiteHmsDispatch>()
            {
                new NSSDKViewModels.NetSuiteHmsDispatch()
                {
                   NSDispatchId=1,
                   AssetTagNumber="a123"
                }
            };
        }

        private List<HospiceLocationMembers> GetHospiceLocationMembers()
        {
            return new List<HospiceLocationMembers>()
            {
                new HospiceLocationMembers()
                {
                   Id = 2,
                   HospiceLocationId = 2020,
                   NetSuiteContactId = 2,
                   HospiceMemberId = 2
                }
            };
        }
    }
}
