using Hms2Billing = Hms2BillingSDK.Repositories.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Test.MockProvider
{
    public class MockRepository
    {
        private readonly MockModels _mockModels;

        private readonly HttpContext _httpContext;

        public MockRepository(MockModels mockModels, IHttpContextAccessor httpContextAccessor)
        {
            _mockModels = mockModels;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IPermissionNounsRepository GetPermissionNounsRepository()
        {
            var permissionNounsRepository = GetRepository<IPermissionNounsRepository, PermissionNouns>(_mockModels.PermissionNouns, u => u.Id);
            return permissionNounsRepository.Object;
        }

        public ICreditHoldHistoryRepository GetCreditHoldHistoryRepository()
        {
            var creditHoldHistoryRepository = GetRepository<ICreditHoldHistoryRepository, CreditHoldHistory>(_mockModels.CreditHoldHistories, u => u.Id);
            return creditHoldHistoryRepository.Object;
        }

        public IPermissionVerbsRepository GetPermissionVerbsRepository()
        {
            var permissionVerbsRepository = GetRepository<IPermissionVerbsRepository, PermissionVerbs>(_mockModels.PermissionVerbs, u => u.Id);
            return permissionVerbsRepository.Object;
        }

        public IUsersRepository GetUsersRepository()
        {
            var user = new Users();
            var usersRespository = GetRepository<IUsersRepository, Users>(_mockModels.Users, u => u.Id);
            usersRespository.Setup(x => x.GetHospiceAccessByUserId(It.IsAny<int>())).Returns<int>(id => Task.FromResult(
                                                                                id > 0 ? _mockModels.Users.Where(u => u.Id == id)
                                                                                                    .FirstOrDefault().UserRoles.Select(ur => ur.ResourceId)
                                                                                : new List<string>()));

            usersRespository.Setup(x => x.GetSiteAccessByUserId(It.IsAny<int>())).Returns<int>(id => Task.FromResult(
                                                                               id > 0 ? _mockModels.Users.FirstOrDefault(u => u.Id == id)
                                                                                                   .UserRoles
                                                                                                   .Where(ur => ur.ResourceType.ToLower() == "site")
                                                                                                   .Select(ur => ur.ResourceId)
                                                                               : new List<string>()));

            usersRespository.Setup(x => x.GetByCognitoUserId(It.IsAny<string>()))
                .Callback<string>(
                cognitoUsrId =>
                {
                    user = _mockModels.Users.FirstOrDefault(u => u.CognitoUserId == cognitoUsrId);
                }).Returns(() => Task.FromResult(user));

            return usersRespository.Object;
        }

        public IRolesRepository GetRolesRepository()
        {
            var rolesRepository = GetRepository<IRolesRepository, Roles>(_mockModels.Roles, u => u.Id);
            var rolesList = _mockModels.Roles;

            rolesRepository.Setup(x => x.AddAsync(It.IsAny<Roles>()))
                .Callback<Roles>(
                roleModel =>
                {
                    roleModel.Id = new Random().Next(100000, 1000000);
                    if (roleModel.RolePermissions != null)
                    {
                        foreach (var rolePermission in roleModel.RolePermissions)
                        {
                            rolePermission.PermissionVerb = _mockModels.PermissionVerbs.FirstOrDefault(pv => pv.Id == rolePermission.PermissionVerbId);
                            rolePermission.PermissionNoun = _mockModels.PermissionNouns.FirstOrDefault(pn => pn.Id == rolePermission.PermissionNounId);
                        }
                    }
                    _mockModels.Roles.Add(roleModel);
                })
                .Returns<Roles>(role => Task.FromResult(role));
            return rolesRepository.Object;
        }

        public IUserProfilePictureRepository GetUserProfilePictureRepository()
        {
            var userProfilePictureRepository = GetRepository<IUserProfilePictureRepository, UserProfilePicture>(_mockModels.UserProfilePictures, l => l.Id);
            userProfilePictureRepository.Setup(x => x.DeleteAsync(It.IsAny<UserProfilePicture>()))
               .Callback<UserProfilePicture>(
               modelObj =>
               {
                   var existingModel = _mockModels.UserProfilePictures.Remove(modelObj);
               })
               .Returns(Task.FromResult(true));
            return userProfilePictureRepository.Object;
        }

        public IHospiceLocationRepository GethospiceLocationRepository()
        {
            var hospiceLocationRepository = GetRepository<IHospiceLocationRepository, HospiceLocations>(_mockModels.HospiceLocations, u => u.Id);

            hospiceLocationRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(_mockModels.HospiceLocations.FirstOrDefault(h => h.Id == id
                                                           && !h.IsDeleted)));
            return hospiceLocationRepository.Object;
        }

        public IFacilityRepository GetfacilityRepository()
        {
            var facilityRepository = GetRepository<IFacilityRepository, Facilities>(_mockModels.Facilities, f => f.Id);

            return facilityRepository.Object;
        }

        public IHospiceRepository GethospiceRepository()
        {
            var hospiceRepository = GetRepository<IHospiceRepository, Hospices>(_mockModels.Hospices, h => h.Id);

            Hospices model = null;
            hospiceRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Callback<int>(
                id =>
                {
                    model = _mockModels.Hospices.FirstOrDefault(h => h.Id == id && !h.IsDeleted);
                    if (model != null && model.CreditHoldByUserId != null)
                    {
                        model.CreditHoldByUser = _mockModels.Users.FirstOrDefault(u => u.Id == model.CreditHoldByUserId);
                    }
                }).Returns(() => Task.FromResult(model));

            return hospiceRepository.Object;
        }

        public ICsvMappingRepository GetCsvMappingRepository()
        {
            var csvMappingRepository = GetRepository<ICsvMappingRepository, CsvMappings>(_mockModels.CsvMappings, c => c.Id);
            return csvMappingRepository.Object;
        }

        public ISitesRepository GetSitesRepository()
        {
            var sitesRepository = GetRepository<ISitesRepository, Sites>(_mockModels.Sites, s => s.Id);
            sitesRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(_mockModels.Sites.FirstOrDefault(i => i.Id == id
                                                           && (GetUserSiteIds().Contains("*") || GetUserSiteIds().Contains(i.Id.ToString())))));

            return sitesRepository.Object;
        }

        public IOrderHeadersRepository GetOrderHeadersRepository()
        {
            var orderHeadersRepository = GetRepository<IOrderHeadersRepository, OrderHeaders>(_mockModels.OrderHeaders, o => o.Id);
            orderHeadersRepository.Setup(x => x.AddAsync(It.IsAny<OrderHeaders>()))
            .Callback<OrderHeaders>(
            orderModel =>
            {
                orderModel.Id = new Random().Next(100000, 1000000);
                if (string.IsNullOrEmpty(orderModel.OrderNumber))
                {
                    orderModel.OrderNumber = new Random().Next(100000, 1000000).ToString();
                }
                foreach (var lineItem in orderModel.OrderLineItems)
                {
                    lineItem.Id = new Random().Next(100000, 1000000);
                }
                orderModel.Site = _mockModels.Sites.FirstOrDefault(s => s.Id == orderModel.SiteId);
                _mockModels.OrderHeaders.Add(orderModel);
                _mockModels.OrderLineItems.AddRange(orderModel.OrderLineItems);
            })
            .Returns<OrderHeaders>(orderModel => Task.FromResult(orderModel));
            OrderHeaders model = null;
            orderHeadersRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Callback<int>(
                id =>
                {
                    model = _mockModels.OrderHeaders.FirstOrDefault(m => m.Id == id);
                    if (model != null)
                    {
                        model.OrderType = _mockModels.OrderTypes.FirstOrDefault(o => o.Id == model.OrderTypeId);
                        if (model.OrderNotes == null)
                        {
                            model.OrderNotes = new List<OrderNotes>();
                        }
                    }
                }).Returns(() => Task.FromResult(model));

            orderHeadersRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OrderHeaders, bool>>>()))
            .Callback<Expression<Func<OrderHeaders, bool>>>(
              expression =>
              {
                  var func = expression.Compile();
                  model = _mockModels.OrderHeaders.Where(c => func(c)).FirstOrDefault();
                  if (model != null)
                  {
                      model.OrderType = _mockModels.OrderTypes.FirstOrDefault(o => o.Id == model.OrderTypeId);
                      if (model.OrderNotes == null)
                      {
                          model.OrderNotes = new List<OrderNotes>();
                      }
                  }
              })
            .Returns(() => Task.FromResult(model));

            IEnumerable<OrderHeaders> orderModels = new List<OrderHeaders>();
            orderHeadersRepository.Setup(x => x.GetDispatchOrderAsync(It.IsAny<Expression<Func<OrderHeaders, bool>>>()))
             .Callback<Expression<Func<OrderHeaders, bool>>>(
               expression =>
               {
                   var func = expression.Compile();
                   orderModels = _mockModels.OrderHeaders.Where(c => func(c));
                   foreach (var order in orderModels)
                   {
                       order.DispatchInstructions = _mockModels.DispatchInstructions.FirstOrDefault(d => d.OrderHeaderId == order.Id);
                   }

               })
             .Returns(() => Task.FromResult(orderModels));

            return orderHeadersRepository.Object;
        }

        public IOrderLineItemsRepository GetOrderLineItemsRepository()
        {
            var orderLineItemsRepository = GetRepository<IOrderLineItemsRepository, OrderLineItems>(_mockModels.OrderLineItems, l => l.Id);
            return orderLineItemsRepository.Object;
        }

        public IItemCategoryRepository GetItemCategoryRepository()
        {
            var itemCategoryRepository = GetRepository<IItemCategoryRepository, ItemCategories>(_mockModels.ItemCategories, l => l.Id);
            return itemCategoryRepository.Object;
        }

        public IItemSubCategoryRepository GetItemSubCategoryRepository()
        {
            var itemSubCategoryRepository = GetRepository<IItemSubCategoryRepository, ItemSubCategories>(_mockModels.ItemSubCategories, l => l.Id);
            return itemSubCategoryRepository.Object;
        }

        public IItemImageFilesRepository GetItemImageFilesRepository()
        {
            var itemImageFilesRepository = GetRepository<IItemImageFilesRepository, ItemImageFiles>(_mockModels.ItemImageFiles, l => l.Id);
            return itemImageFilesRepository.Object;
        }

        public IItemImageRepository GetItemImageRepository()
        {
            var itemImageRepository = GetRepository<IItemImageRepository, ItemImages>(_mockModels.ItemImages, l => l.Id);
            return itemImageRepository.Object;
        }

        public IHospiceMemberRepository GetHospiceMemberRepository()
        {
            var hospiceMemberRepository = GetRepository<IHospiceMemberRepository, HospiceMember>(_mockModels.HospiceMembers, h => h.Id);

            HospiceMember hospiceMember = null;
            IEnumerable<HospiceMember> hospiceMembers = null;

            hospiceMemberRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<HospiceMember, bool>>>()))
            .Callback<Expression<Func<HospiceMember, bool>>>(
              expression =>
              {
                  var func = expression.Compile();
                  hospiceMember = _mockModels.HospiceMembers.Where(c => func(c)).FirstOrDefault();
                  if (hospiceMember != null)
                  {
                      hospiceMember.User = _mockModels.Users.FirstOrDefault(u => u.Id == hospiceMember.UserId);
                  }
              })
            .Returns(() => Task.FromResult(hospiceMember));

            hospiceMemberRepository.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<HospiceMember, bool>>>()))
            .Callback<Expression<Func<HospiceMember, bool>>>(
              expression =>
              {
                  var func = expression.Compile();
                  hospiceMembers = _mockModels.HospiceMembers.Where(c => func(c));
                  foreach (var member in hospiceMembers)
                  {
                      if (member != null)
                      {
                          member.User = _mockModels.Users.FirstOrDefault(u => u.Id == member.UserId);
                      }
                  }
              })
            .Returns(() => Task.FromResult(hospiceMembers));

            hospiceMemberRepository.Setup(x => x.AddAsync(It.IsAny<HospiceMember>()))
           .Callback<HospiceMember>(
           hospiceMemberModel =>
           {
               hospiceMemberModel.Id = new Random().Next(100000, 1000000);
               if (hospiceMemberModel.HospiceLocationMembers == null)
               {
                   hospiceMemberModel.HospiceLocationMembers = new List<HospiceLocationMembers>();
               }
               _mockModels.HospiceMembers.Add(hospiceMemberModel);
           });

            return hospiceMemberRepository.Object;
        }

        public IFacilityPatientRepository GetFacilityPatientRepository()
        {
            var facilityPatientRepository = GetRepository<IFacilityPatientRepository, FacilityPatient>(_mockModels.FacilityPatients, f => f.Id);
            return facilityPatientRepository.Object;
        }

        public IFacilityPatientHistoryRepository GetFacilityPatientHistoryRepository()
        {
            var facilityPatientHistoryRepository = GetRepository<IFacilityPatientHistoryRepository, FacilityPatientHistory>(_mockModels.FacilityPatientHistories, f => f.Id);
            return facilityPatientHistoryRepository.Object;
        }

        public IDriverRepository GetDriverRepository()
        {
            var driverRepository = GetRepository<IDriverRepository, Drivers>(_mockModels.Drivers, d => d.Id);
            return driverRepository.Object;
        }

        public IItemRepository GetItemRepository()
        {
            var itemRepository = GetRepository<IItemRepository, Items>(_mockModels.Items, i => i.Id);

            Items model = null;
            itemRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Callback<int>(
                id =>
                {
                    model = _mockModels.Items.FirstOrDefault(m => m.Id == id && !m.IsDeleted);
                }).Returns(() => Task.FromResult(model));

            itemRepository.Setup(x => x.AddAsync(It.IsAny<Items>()))
            .Callback<Items>(
            itemModel =>
            {
                itemModel.Id = new Random().Next(100000, 1000000);

                foreach (var inv in itemModel.Inventory)
                {
                    inv.Id = new Random().Next(100000, 1000000);
                    inv.ItemId = itemModel.Id;
                    inv.Item = itemModel;
                }
                _mockModels.Items.Add(itemModel);
                _mockModels.Inventories.AddRange(itemModel.Inventory);
            });

            itemRepository.Setup(x => x.UpdateAsync(It.IsAny<Items>()))
                .Callback<Items>(
                itemModel =>
                {
                    model = _mockModels.Items.FirstOrDefault(m => m.Id == itemModel.Id);
                    model = itemModel;
                    foreach (var inventory in itemModel.Inventory)
                    {
                        inventory.ItemId = itemModel.Id;
                        inventory.Item = itemModel;
                        var oldInventory = _mockModels.Inventories.FirstOrDefault(m => m.Id == inventory.Id);
                        if (oldInventory != null)
                        {
                            oldInventory = inventory;
                        }
                        else
                        {
                            inventory.Id = new Random().Next(100000, 1000000);
                            _mockModels.Inventories.Add(inventory);
                        }
                    }
                    if (model.ItemTransferRequests != null)
                    {
                        foreach (var transferRequest in model.ItemTransferRequests)
                        {
                            transferRequest.Id = new Random().Next(100000, 1000000);
                            transferRequest.ItemId = itemModel.Id;
                            transferRequest.Item = itemModel;
                            _mockModels.ItemTransferRequests.Add(transferRequest);
                        }
                    }
                })
                .Returns<Items>(model => Task.FromResult(model));

            return itemRepository.Object;
        }

        public IItemTransferRequestRepository GetItemTransferRequestRepository()
        {
            var itemTransferRequestRepository = GetRepository<IItemTransferRequestRepository, ItemTransferRequests>(_mockModels.ItemTransferRequests, i => i.Id);
            return itemTransferRequestRepository.Object;
        }

        public IAddressRepository GetAddressRepository()
        {
            var addressRepository = GetRepository<IAddressRepository, Addresses>(_mockModels.Addresses, a => a.Id);
            return addressRepository.Object;
        }

        public ISiteServiceAreaRepository GetSiteServiceAreaRepository()
        {
            var siteServiceAreaRepository = GetRepository<ISiteServiceAreaRepository, SiteServiceAreas>(_mockModels.SiteServiceAreas, s => s.Id);
            return siteServiceAreaRepository.Object;
        }

        public IHospiceLocationMemberRepository GetHospiceLocationMemberRepository()
        {
            var hospiceLocationMemberRepository = GetRepository<IHospiceLocationMemberRepository, HospiceLocationMembers>(_mockModels.HospiceLocationMembers, h => h.Id);
            return hospiceLocationMemberRepository.Object;
        }

        public IInventoryRepository GetInventoryRepository()
        {
            IEnumerable<Inventory> inventories = null;
            var inventoryRepository = GetRepository<IInventoryRepository, Inventory>(_mockModels.Inventories, i => i.Id);
            inventoryRepository.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<Inventory, bool>>>()))
            .Callback<Expression<Func<Inventory, bool>>>(
              expression =>
              {
                  var func = expression.Compile();
                  inventories = _mockModels.Inventories.Where(c => func(c)).ToList();
                  if (inventories != null && inventories.Any())
                  {
                      foreach (var inventory in inventories)
                      {
                          inventory.Item = _mockModels.Items.FirstOrDefault(u => u.Id == inventory.ItemId);
                      }
                  }
              })
            .Returns(() => Task.FromResult(inventories));

            inventoryRepository.Setup(x => x.AddAsync(It.IsAny<Inventory>()))
             .Callback<Inventory>(
             inventoryModel =>
             {
                 inventoryModel.Id = new Random().Next(100000, 1000000);

                 inventoryModel.Item = _mockModels.Items.FirstOrDefault(s => s.Id == inventoryModel.ItemId);
                 _mockModels.Inventories.Add(inventoryModel);
             })
             .Returns<Inventory>(inventoryModel => Task.FromResult(inventoryModel));

            inventoryRepository.Setup(x => x.DeleteAsync(It.IsAny<Inventory>()))
               .Callback<Inventory>(
               modelObj =>
               {
                   var existingModel = _mockModels.Inventories.Remove(modelObj);
               })
               .Returns(Task.FromResult(true));
            return inventoryRepository.Object;
        }

        public IPatientInventoryRepository GetPatientInventoryRepository()
        {
            var patientInventoryRepository = GetRepository<IPatientInventoryRepository, PatientInventory>(_mockModels.PatientInventories, p => p.Id);

            var patientInventoryResponses = new List<PatientInventory>();
            patientInventoryRepository.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<PatientInventory, bool>>>()))
             .Callback<Expression<Func<PatientInventory, bool>>>(
               expression =>
               {
                   var func = expression.Compile();
                   patientInventoryResponses = _mockModels.PatientInventories.Where(c => func(c)).ToList();
                   foreach (var patientInventory in patientInventoryResponses)
                   {
                       patientInventory.Item = _mockModels.Items.FirstOrDefault(i => i.Id == patientInventory.ItemId);
                   }
               })
             .Returns(() => Task.FromResult(patientInventoryResponses.AsEnumerable()));

            patientInventoryRepository.Setup(x => x.AddAsync(It.IsAny<PatientInventory>()))
                .Callback<PatientInventory>(
                model =>
                {
                    model.Id = new Random().Next(100000, 1000000);
                    model.Inventory = _mockModels.Inventories.FirstOrDefault(s => s.Id == model.InventoryId);
                    model.Item = _mockModels.Items.FirstOrDefault(i => i.Id == model.ItemId);
                    _mockModels.PatientInventories.Add(model);
                })
                .Returns<PatientInventory>(model => Task.FromResult(model));

            patientInventoryRepository.Setup(x => x.AddManyAsync(It.IsAny<IEnumerable<PatientInventory>>()))
             .Callback<IEnumerable<PatientInventory>>(
             models =>
             {
                 foreach (var model in models)
                 {
                     model.Id = new Random().Next(100000, 1000000);
                     model.Item = _mockModels.Items.FirstOrDefault(i => i.Id == model.ItemId);
                     _mockModels.PatientInventories.Add(model);
                 }
             })
             .Returns<IEnumerable<PatientInventory>>(models => Task.FromResult(models));

            return patientInventoryRepository.Object;
        }

        public IDispatchInstructionsRepository GetDispatchInstructionsRepository()
        {
            var dispatchInstructionRepository = GetRepository<IDispatchInstructionsRepository, DispatchInstructions>(_mockModels.DispatchInstructions, d => d.Id);

            dispatchInstructionRepository.Setup(x => x.UpdateManyAsync(It.IsAny<IEnumerable<DispatchInstructions>>()))
                             .Callback<IEnumerable<DispatchInstructions>>(
                             models =>
                             {
                                 var updatedIds = models.Select(di => di.Id);
                                 _mockModels.DispatchInstructions.RemoveAll(i => !updatedIds.Contains(i.Id));
                                 foreach (var model in models)
                                 {
                                     if (model.OrderHeaderId.HasValue)
                                     {
                                         model.OrderHeader = _mockModels.OrderHeaders.FirstOrDefault(o => o.Id == model.OrderHeaderId);
                                     }
                                     if (model.TransferRequestId.HasValue)
                                     {
                                         model.TransferRequest = _mockModels.ItemTransferRequests.FirstOrDefault(o => o.Id == model.TransferRequestId);
                                     }
                                 }
                                 _mockModels.DispatchInstructions.AddRange(models);
                             })
                             .Returns<IEnumerable<DispatchInstructions>>(models => Task.FromResult(models));
            return dispatchInstructionRepository.Object;
        }

        public ISiteMemberRepository GetSiteMemberRepository()
        {
            var siteMemberRepository = GetRepository<ISiteMemberRepository, SiteMembers>(_mockModels.SiteMembers, s => s.Id);
            return siteMemberRepository.Object;
        }

        public IOrderFulfillmentLineItemsRepository GetOrderFulfillmentLineItemsRepository()
        {
            var orderFulfillmentLineItemsRepository = GetRepository<IOrderFulfillmentLineItemsRepository, OrderFulfillmentLineItems>(_mockModels.OrderFulfillmentLineItems, o => o.Id);
            return orderFulfillmentLineItemsRepository.Object;
        }

        public ISubscriptionRepository GetSubscriptionRepository()
        {
            var susbscriptionRepository = GetRepository<ISubscriptionRepository, Subscriptions>(_mockModels.Subscriptions, s => s.Id);
            return susbscriptionRepository.Object;
        }

        public ISubscriptionItemRepository GetSubscriptionItemRepository()
        {
            var subscriptionItemRepository = GetRepository<ISubscriptionItemRepository, SubscriptionItems>(_mockModels.SubscriptionItems, s => s.Id);
            return subscriptionItemRepository.Object;
        }

        public IHMS2ContractRepository GetHMS2ContractRepository()
        {
            var hms2ContractRepository = GetRepository<IHMS2ContractRepository, Hms2Contracts>(_mockModels.Hms2Contracts, s => s.Id);
            return hms2ContractRepository.Object;
        }

        public IHMS2ContractItemRepository GetHMS2ContractItemRepository()
        {
            var hms2ContractItemRepository = GetRepository<IHMS2ContractItemRepository, Hms2ContractItems>(_mockModels.Hms2ContractItems, s => s.Id);
            return hms2ContractItemRepository.Object;
        }

        public Hms2Billing.IHms2BillingContractsRepository GetHms2BillingContractsRepository()
        {
            var hms2BillingContractsRepository = new Mock<Hms2Billing.IHms2BillingContractsRepository>();
            return hms2BillingContractsRepository.Object;
        }

        public Hms2Billing.IHms2BillingContractItemsRepository GetHms2BillingContractItemsRepository()
        {
            var hms2BillingContractItemsRepository = new Mock<Hms2Billing.IHms2BillingContractItemsRepository>();
            return hms2BillingContractItemsRepository.Object;
        }

        public IOrderNotesRepository GetOrderNotesRepository()
        {
            var orderNotesRepository = new Mock<IOrderNotesRepository>();
            return orderNotesRepository.Object;
        }

        public IDispatchAuditLogRepository GetDispatchAuditLogRepository()
        {
            var dispatchAuditLogRepository = new Mock<IDispatchAuditLogRepository>();
            return dispatchAuditLogRepository.Object;
        }

        public IContractRecordsRepository GetContractRecordsRepository()
        {
            var contractRecordsRepository = GetRepository<IContractRecordsRepository, ContractRecords>(_mockModels.ContractRecords, s => s.Id);
            contractRecordsRepository.Setup(x => x.DeleteAsync(It.IsAny<ContractRecords>()))
               .Callback<ContractRecords>(
               modelObj =>
               {
                   var existingModel = _mockModels.ContractRecords.Remove(modelObj);
               })
               .Returns(Task.FromResult(true));
            return contractRecordsRepository.Object;
        }

        public IEquipmentSettingTypeRepository GetEquipmentSettingTypeRepository()
        {
            var equipmentSettingTypeRepository = new Mock<IEquipmentSettingTypeRepository>();
            return equipmentSettingTypeRepository.Object;
        }

        private Mock<T> GetRepository<T, K>(List<K> modelList, Func<K, int> idFunc) where T : class, IRepository<K> where K : class
        {
            var repository = new Mock<T>();
            K model = default;
            List<K> modelResponses = default;
            repository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Callback<int>(
                id =>
                {
                    model = modelList.FirstOrDefault(m => idFunc(m) == id);
                }).Returns(() => Task.FromResult(model));

            repository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(modelList.AsEnumerable()));


            repository.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<K, bool>>>()))
             .Callback<Expression<Func<K, bool>>>(
               expression =>
               {
                   var func = expression.Compile();
                   modelResponses = modelList.Where(c => func(c)).ToList();
               })
             .Returns(() => Task.FromResult(modelResponses.AsEnumerable()));

            repository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<K, bool>>>()))
             .Callback<Expression<Func<K, bool>>>(
               expression =>
               {
                   var func = expression.Compile();
                   model = modelList.FirstOrDefault(c => func(c));
               })
             .Returns(() => Task.FromResult(model));

            repository.Setup(x => x.AddAsync(It.IsAny<K>()))
                .Callback<K>(
                modelObj =>
                {
                    var model = modelObj as dynamic;
                    model.Id = new Random().Next(100000, 1000000);
                    modelList.Add(modelObj);
                })
                .Returns<K>(model => Task.FromResult(model));

            repository.Setup(x => x.AddManyAsync(It.IsAny<IEnumerable<K>>()))
             .Callback<IEnumerable<K>>(
             models =>
             {
                 foreach (var modelObj in models)
                 {
                     var model = modelObj as dynamic;
                     model.Id = new Random().Next(100000, 1000000);
                     modelList.Add(modelObj);
                 }
             })
             .Returns<IEnumerable<K>>(models => Task.FromResult(models));

            repository.Setup(x => x.UpdateAsync(It.IsAny<K>()))
                .Callback<K>(
                model =>
                {
                    var oldModel = modelList.FirstOrDefault(m => idFunc(m) == idFunc(model));
                    oldModel = model;
                })
                .Returns<K>(model => Task.FromResult(model));

            repository.Setup(x => x.UpdateManyAsync(It.IsAny<IEnumerable<K>>()))
                             .Callback<IEnumerable<K>>(
                             models =>
                             {
                                 var modelsToUpdate = models.ToList();
                                 var updatedIds = models.Select(idFunc);
                                 modelList.RemoveAll(i => updatedIds.Contains(idFunc(i)));
                                 modelList.AddRange(modelsToUpdate);
                             })
                             .Returns<IEnumerable<K>>(models => Task.FromResult(models));

            repository.Setup(x => x.DeleteAsync(It.IsAny<K>()))
               .Callback<K>(
               modelObj =>
               {
                   var model = modelObj as dynamic;
                   var existingModel = modelList.FirstOrDefault(c => idFunc(c) == idFunc(model));
                   if (modelObj.GetType().GetProperty("IsDeleted") != null)
                   {
                       model.IsDeleted = true;
                   }
                   else
                   {
                       modelList.Remove(existingModel);
                   }
                   existingModel = model;
               })
               .Returns<K>(model => Task.FromResult(model));


            repository.Setup(x => x.DeleteAsync(It.IsAny<Expression<Func<K, bool>>>()))
                .Callback<Expression<Func<K, bool>>>(
                expression =>
                {
                    var func = expression.Compile();
                    modelList.RemoveAll(i => func(i));
                })
                .Returns(() => Task.CompletedTask);

            var isExist = false;
            repository.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<K, bool>>>()))
            .Callback<Expression<Func<K, bool>>>(
              expression =>
              {
                  var func = expression.Compile();
                  isExist = modelList.Exists(c => func(c));
              })
            .Returns(() => Task.FromResult(isExist));

            return repository;
        }

        private List<string> GetUserSiteIds()
        {

            var userId = GetUserId();
            var user = _mockModels.Users.FirstOrDefault(u => u.Id == userId);
            var userRoles = user?.UserRoles.Where(ur => ur.ResourceType == ResourceTypes.Site.ToString());
            if (userRoles == null)
            {
                return new List<string>();
            }
            return userRoles.Select(ur => ur.ResourceId).ToList();
        }

        private int? GetUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst("userId");
            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            return null;
        }
    }
}
