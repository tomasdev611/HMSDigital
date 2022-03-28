using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.API;
using HMSDigital.Core.API.Controllers;
using HMSDigital.Core.BusinessLayer.Services;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Sieve;
using HMSDigital.Core.ViewModels;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using NotificationSDK.Interfaces;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using PatientSDK = HospiceSource.Digital.Patient.SDK.Interfaces;
using NSSDKViewModels = HospiceSource.Digital.NetSuite.SDK.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using HMSDigital.Common.BusinessLayer.Config;

namespace Core.Test.MockProvider
{
    public class MockServices
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly HttpContextAccessor _httpContextAccessor;

        private readonly MockModels _mockModels;

        private readonly MockViewModels _mockViewModels;

        private readonly IMapper _mapper;

        public MockServices()
        {
            _httpContextAccessor = new HttpContextAccessor()
            {
                HttpContext = GetControllerContext().HttpContext
            };

            var services = new ServiceCollection()
               .AddAutoMapper(typeof(Startup)).AddScoped<IHttpContextAccessor>(h => _httpContextAccessor); ;

            services.AddLogging();
            services.AddScoped(s => GetUsersService());
            services.AddScoped(s => GetUserAccessService());
            services.AddScoped(s => GetIdentityService());
            services.AddScoped(s => GetUserVerifyService());
            services.AddScoped(s => GetNotificationService());
            services.AddScoped(s => GetPatientSDKService());
            services.AddScoped(s => GetAuditService());
            services.AddScoped(s => GetFileService());
            services.AddScoped(s => GetNetSuiteService());
            services.AddScoped(s => GetAddressStandardizerService());
            services.AddScoped(s => GetPatientService());
            services.AddScoped(s => GetInventoryService());
            services.AddScoped(s => GetVehiclesService());
            services.AddScoped(s => GetFileStorageService());

            services.AddScoped(s => GetHttpContextAccessor());

            services.AddScoped<MockModels>();
            services.AddScoped<MockViewModels>();

            services.AddScoped(s => new MockRepository(s.GetService<MockModels>(), s.GetService<IHttpContextAccessor>()));

            services.AddScoped(s => s.GetService<MockRepository>().GetRolesRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetPermissionNounsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetPermissionVerbsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetUsersRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetfacilityRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GethospiceLocationRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GethospiceRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetCsvMappingRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetSitesRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetOrderHeadersRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetOrderLineItemsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetHospiceMemberRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetFacilityPatientRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetFacilityPatientHistoryRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetDriverRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetItemRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetAddressRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetSiteServiceAreaRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetHospiceLocationMemberRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetInventoryRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetPatientInventoryRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetItemTransferRequestRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetDispatchInstructionsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetSiteMemberRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetOrderFulfillmentLineItemsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetSubscriptionRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetSubscriptionItemRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetHMS2ContractRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetHMS2ContractItemRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetUserProfilePictureRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetOrderNotesRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetDispatchAuditLogRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetCreditHoldHistoryRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetContractRecordsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetItemCategoryRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetItemSubCategoryRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetItemImageFilesRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetItemImageRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetEquipmentSettingTypeRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetHms2BillingContractsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetHms2BillingContractItemsRepository());

            services.AddScoped<ISitesService, SitesService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAccessService, UserAccessService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IPermissionsService, PermissionsService>();
            services.AddScoped<IHospiceLocationService, HospiceLocationService>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IHospiceService, HospiceService>();
            services.AddScoped<IOrderHeadersService, OrderHeadersService>();
            services.AddScoped<IOrderLineItemsService, OrderLineItemsService>();
            services.AddScoped<IHospiceMemberService, HospiceMemberService>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IItemsService, ItemsService>();
            services.AddScoped<IDriversService, DriversService>();
            services.AddScoped<IDispatchService, DispatchService>();
            services.AddScoped<IFulfillmentService, FulfillmentService>();
            services.AddScoped<IDispatchService, DispatchService>();
            services.AddScoped<IDbContextFactoryService, DbContextFactoryService>();
            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilters>();
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSorts>();

            services.AddScoped<HospiceMemberService>();

            services.AddLogging();
            //controllers
            services.AddTransient<UsersController>();
            services.AddTransient<RolesController>();


            var netSuiteOptions = new Mock<IOptions<NetSuiteConfig>>();
            netSuiteOptions.Setup(x => x.Value).Returns(GetNetSuiteConfig());
            services.AddScoped(s => netSuiteOptions.Object);

            services.AddScoped<IHttpContextAccessor>(s => _httpContextAccessor);

            _serviceProvider = services.BuildServiceProvider();

            _mockModels = _serviceProvider.GetService<MockModels>();
            _mockViewModels = _serviceProvider.GetService<MockViewModels>();
            _mapper = _serviceProvider.GetService<IMapper>();
        }

        public T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        public T GetController<T>() where T : ControllerBase
        {
            var controller = _serviceProvider.GetService<T>();
            controller.ControllerContext = GetControllerContext();
            return controller;
        }

        public void TestAuthorizeAttribute<T>(T controller, string methodName, string policyName, Type[] argumentTypes)
        {
            var type = controller.GetType();
            var methodInfo = type.GetMethod(methodName, argumentTypes);
            var attributes = (AuthorizeAttribute[])methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);

            //test correct Authorize attribute
            Assert.NotEmpty(attributes);
            Assert.Single(attributes);
            Assert.Equal(policyName, attributes[0].Policy);
        }

        public IUserService GetUsersService()
        {
            var userService = new Mock<IUserService>();
            return userService.Object;
        }

        public IUserAccessService GetUserAccessService()
        {
            var userAccessService = new Mock<IUserAccessService>();
            return userAccessService.Object;
        }

        public IRolesService GetRolesService()
        {
            var rolesService = new Mock<IRolesService>();
            return rolesService.Object;
        }

        public IIdentityService GetIdentityService()
        {
            var identityService = new Mock<IIdentityService>();
            User user = null;
            identityService.Setup(x => x.EnableUser(It.IsAny<string>()))
              .Callback<string>(
                 expression =>
                 {
                     user = _mockViewModels.Users.Where(i => i.UserId == expression).FirstOrDefault();
                     user.Enabled = true;
                 })
                .Returns(() => Task.FromResult(user));
            identityService.Setup(x => x.DisableUser(It.IsAny<string>()))
              .Callback<string>(
                 expression =>
                 {
                     user = _mockViewModels.Users.Where(i => i.UserId == expression).FirstOrDefault();
                     user.Enabled = false;
                 })
                .Returns(() => Task.FromResult(user));

            identityService.Setup(x => x.GetUser(It.IsAny<string>()))
                 .Callback<string>(
                 expression =>
                 {
                     user = _mockViewModels.Users.Where(i => i.UserId == expression).FirstOrDefault();
                 })
                .Returns(() => Task.FromResult(user));

            identityService.Setup(x => x.CreateUser(It.IsAny<UserMinimal>()))
                .Callback<UserMinimal>(
                (userCreateRequest) =>
                {
                    user = new User()
                    {
                        UserId = new Random().Next(100000, 1000000).ToString(),
                        FirstName = userCreateRequest.FirstName,
                        LastName = userCreateRequest.LastName,
                        Email = userCreateRequest.Email,
                        PhoneNumber = userCreateRequest.PhoneNumber
                    };
                    var list = _mockViewModels.Users.ToList();
                    list.Add(user);
                    _mockViewModels.Users = list;
                })
                .Returns(() => Task.FromResult(user));

            identityService.Setup(x => x.UpdateUser(It.IsAny<string>(), It.IsAny<UserMinimal>()))
                .Callback<string, UserMinimal>(
                (userId, usersUpdateRequest) =>
                {
                    var list = _mockViewModels.Users.ToList();
                    user = list.Where(u => u.UserId == userId).FirstOrDefault();
                    user.FirstName = usersUpdateRequest.FirstName;
                    user.LastName = usersUpdateRequest.LastName;
                    user.Email = usersUpdateRequest.Email;
                    user.PhoneNumber = usersUpdateRequest.PhoneNumber;
                    _mockViewModels.Users = list;
                })
                .Returns(() => Task.FromResult(user));
            return identityService.Object;
        }

        public IVerifyService GetUserVerifyService()
        {
            var userVerifyService = new Mock<IVerifyService>();
            return userVerifyService.Object;
        }

        public IHttpContextAccessor GetHttpContextAccessor()
        {
            var httpContextAccessor = new HttpContextAccessor()
            {
                HttpContext = GetControllerContext().HttpContext
            };
            return httpContextAccessor;
        }

        public INotificationService GetNotificationService()
        {
            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.SendUserCreatedNotification(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.Delay(1));
            return notificationService.Object;
        }

        public PatientSDK.IPatientService GetPatientSDKService()
        {
            var patientService = new Mock<PatientSDK.IPatientService>();
            return patientService.Object;
        }

        public IAuditService GetAuditService()
        {
            var auditService = new Mock<IAuditService>();
            return auditService.Object;
        }

        public IFileService GetFileService()
        {
            var fileService = new Mock<IFileService>();
            return fileService.Object;
        }

        public INetSuiteService GetNetSuiteService()
        {
            var netSuiteService = new Mock<INetSuiteService>();
            netSuiteService.Setup(x => x.ConfirmInventoryMovement(It.IsAny<NSSDKViewModels.InventoryMovementRequest>()))
                .Callback<NSSDKViewModels.InventoryMovementRequest>(
                (inventoryMovementRequest) =>
                {
                    _mockModels.InventoryMovementRequests.Add(inventoryMovementRequest);
                });

            netSuiteService.Setup(x => x.ConfirmOrderFulfilment(It.IsAny<NSSDKViewModels.ConfirmFulfilmentRequest>()))
                .Callback<NSSDKViewModels.ConfirmFulfilmentRequest>(
                (confirmFulfillmentRequest) =>
                {
                    _mockModels.ConfirmFulfilmentRequests.Add(confirmFulfillmentRequest);
                });

            var dispatchResponse = new NSSDKViewModels.NetSuiteHMSDispatchResponse();
            netSuiteService.Setup(x => x.GetNetSuiteHmsDispatchRecords(It.IsAny<NSSDKViewModels.NetSuiteDispatchRequest>()))
               .Callback<NSSDKViewModels.NetSuiteDispatchRequest>(
               netSuiteDispatchRequest =>
               {
                   dispatchResponse.Results = _mockModels.DispatchRecords.Where(i => i.NSDispatchId.HasValue && netSuiteDispatchRequest.DispatchRecordIds.Contains(i.NSDispatchId.Value))
                                                                        .Select(d => new NSSDKViewModels.NetSuiteHmsDispatch()
                                                                        {
                                                                            NSDispatchId = d.NSDispatchId,
                                                                            HmsDeliveryDate = d.HmsDeliveryDate,
                                                                            HmsPickupRequestDate = d.HmsPickupRequestDate,
                                                                            PickupDate = d.PickupDate
                                                                        }).ToList();
               })
              .Returns(() => Task.FromResult(dispatchResponse));

            IEnumerable<NSSDKViewModels.DispatchRecordUpdateResponse> dispatchUpdateResponse = new List<NSSDKViewModels.DispatchRecordUpdateResponse>();
            netSuiteService.Setup(x => x.UpdateDispatchRecords(It.IsAny<IEnumerable<NSSDKViewModels.DispatchRecordUpdateRequest>>()))
               .Callback<IEnumerable<NSSDKViewModels.DispatchRecordUpdateRequest>>(
               dispatchRecordUpdateRequests =>
               {
                   foreach (var dispatchRecordUpdateRequest in dispatchRecordUpdateRequests)
                   {
                       var dispatchRecord = _mockModels.DispatchRecords.FirstOrDefault(d => d.NSDispatchId == dispatchRecordUpdateRequest.DispatchRecordId);
                       dispatchRecord.HmsDeliveryDate = dispatchRecordUpdateRequest.Values.HmsDeliveryDate;
                       dispatchRecord.PickupDate = dispatchRecordUpdateRequest.Values.PickupDate;
                       dispatchRecord.HmsPickupRequestDate = dispatchRecordUpdateRequest.Values.HmsPickupRequestDate;

                       dispatchUpdateResponse = dispatchUpdateResponse.Append(new NSSDKViewModels.DispatchRecordUpdateResponse()
                       {
                           DispatchRecordId = dispatchRecordUpdateRequest.DispatchRecordId,
                           Status = "success"
                       });
                   }
               })
              .Returns(() => Task.FromResult(dispatchUpdateResponse));

            ZonePaginatedList<NSSDKViewModels.Subscription> paginatedList = null;
            netSuiteService.Setup(x => x.GetSubscriptions(It.IsAny<IEnumerable<int>>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Callback<IEnumerable<int>, int?, int?>(
                (netSuiteIds, pageNumber, pageSize) =>
                {
                    var list = _mockViewModels.Subscriptions.Records.Where(r => netSuiteIds.Contains(int.Parse(r.NetSuiteCustomerId.Value)));
                    paginatedList = new ZonePaginatedList<NSSDKViewModels.Subscription>()
                    {
                        Records = list
                    };
                })
                .Returns(() => Task.FromResult(paginatedList));

            ZonePaginatedList<NSSDKViewModels.SubscriptionItem> paginatedItemList = null;
            netSuiteService.Setup(x => x.GetSubscriptionItemsBySubscription(It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Callback<int, int?, int?>(
                (netSuiteSubscriptionId, pageNumber, pageSize) =>
                {
                    var list = _mockViewModels.SubscriptionItems.Records.Where(r => int.Parse(r.Subscription.Value) == netSuiteSubscriptionId);
                    paginatedItemList = new ZonePaginatedList<NSSDKViewModels.SubscriptionItem>()
                    {
                        Records = list
                    };
                })
                .Returns(() => Task.FromResult(paginatedItemList));

            netSuiteService.Setup(x => x.CreateCustomerContact(It.IsAny<CustomerContactBase>()))
               .Callback<CustomerContactBase>(
               (customerContact) =>
               {
                   _mockModels.CustomerContactRequests.Add(customerContact);
               });

            netSuiteService.Setup(x => x.UpdateCustomerContact(It.IsAny<CustomerContact>()))
               .Callback<CustomerContactBase>(
               (customerContact) =>
               {
                   _mockModels.CustomerContactRequests.Add(customerContact);
               });

            return netSuiteService.Object;
        }

        public IPatientService GetPatientService()
        {
            var patientService = new Mock<IPatientService>();
            return patientService.Object;
        }

        public IInventoryService GetInventoryService()
        {
            var inventoryService = new Mock<IInventoryService>();
            return inventoryService.Object;
        }

        public IVehiclesService GetVehiclesService()
        {
            var vehiclesService = new Mock<IVehiclesService>();
            return vehiclesService.Object;
        }

        public IFileStorageService GetFileStorageService()
        {
            var fileStorageService = new Mock<IFileStorageService>();
            return fileStorageService.Object;
        }

        public IAddressStandardizerService GetAddressStandardizerService()
        {
            var addressStandardizerService = new Mock<IAddressStandardizerService>();
            addressStandardizerService.Setup(x => x.GetStandardizedAddress(It.IsAny<Address>())).Returns<Address>((address) => Task.FromResult(address));
            return addressStandardizerService.Object;
        }

        private NetSuiteConfig GetNetSuiteConfig()
        {
            return new NetSuiteConfig()
            {
                PatientWarehouseId = 999,
                InternalUsersHostCustomerId = 1234
            };
        }

        private ControllerContext GetControllerContext()
        {
            var claims = new List<Claim>() {
                new Claim(Claims.USERNAME_CLAIM, "LoggedInUser"),
                new Claim(Claims.USER_ID_CLAIM, "10")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var hostString = new HostString("testHot", 123);
            mockHttpRequest.Setup(x => x.Host).Returns(hostString);
            mockHttpRequest.Setup(x => x.Scheme).Returns("testScheme");
            mockHttpContext.Setup(x => x.User).Returns(claimsPrincipal);
            mockHttpContext.Setup(x => x.Request).Returns(mockHttpRequest.Object);
            var defaultHttpContext = new DefaultHttpContext()
            {
                User = claimsPrincipal,
            };
            defaultHttpContext.Connection.RemoteIpAddress = new System.Net.IPAddress(new List<byte>() { 1, 2, 3, 4 }.ToArray());
            return new ControllerContext()
            {
                HttpContext = defaultHttpContext
            };
        }

    }
}
