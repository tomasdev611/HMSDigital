using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.BusinessLayer.Services;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.API;
using HMSDigital.Patient.BusinessLayer;
using HMSDigital.Patient.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HMSDigital.Patient.FHIR.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace HMSDigital.Patient.Test.MockProvider
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

            services.AddScoped(s => GetHttpContextAccessor());

            services.AddScoped<MockModels>();
            services.AddScoped<MockViewModels>();

            services.AddScoped(s => GetAddressStandardizerService());

            services.AddScoped(s => GetFhirService());

            services.AddScoped(s => GetFhirQueueService());

            services.AddScoped(s => new MockRepository(s.GetService<MockModels>()));

            services.AddScoped(s => s.GetService<MockRepository>().GetPatientsRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetUsersRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetAddressesRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetHospiceRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetPatientAddressRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetPatientNotesRepository());
            services.AddScoped(s => s.GetService<MockRepository>().GetPatientMergeHistoryRepository());            

            services.AddScoped<IPatientV2Service, PatientV2Service>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPaginationService, PaginationService>();

            services.AddLogging();

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

        public IHttpContextAccessor GetHttpContextAccessor()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            return httpContextAccessor.Object;
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
                User = claimsPrincipal
            };
            return new ControllerContext()
            {
                HttpContext = defaultHttpContext
            };
        }

        public IAddressStandardizerService GetAddressStandardizerService()
        {
            var addressStandardizerService = new Mock<IAddressStandardizerService>();
            addressStandardizerService.Setup(x => x.GetStandardizedAddress(It.IsAny<Address>())).Returns<Address>((address) => Task.FromResult(address));
            return addressStandardizerService.Object;
        }

        public IFHIRService GetFhirService()
        {
            var fhirService = new Mock<IFHIRService>();
            return fhirService.Object;
        }

        public IFHIRQueueService<FHIRPatientDetail> GetFhirQueueService()
        {
            var fhirQueueService = new Mock<IFHIRQueueService<FHIRPatientDetail>>();
            return fhirQueueService.Object;
        }
    }
}
