using Core.Test.MockProvider;
using EntityFrameworkCoreMock.NSubstitute;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.Data;
using HMSDigital.Core.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Core.Test.Repositories
{
    public class AuditDbContextUnitTest
    {
        private readonly HttpContext _httpContext;

        private readonly DbContextMock<HMSDigitalAuditContext> _auditDbContext;

        public AuditDbContextUnitTest()
        {
            var dummyOptions = new DbContextOptionsBuilder<HMSDigitalAuditContext>().UseSqlServer("FakeConnectionString").Options;
            var mockService = new MockServices();
            var httpContextAccessor = mockService.GetService<IHttpContextAccessor>();
            _httpContext = httpContextAccessor.HttpContext;
            _auditDbContext = new DbContextMock<HMSDigitalAuditContext>(dummyOptions, httpContextAccessor);
        }

        [Fact]
        public void EnableGlobalFilterShouldSucceed()
        {
            Assert.Empty(_httpContext.Items);
            _httpContext.Items.Add(Claims.IGNORE_GLOBAL_FILTER, GlobalFilters.Site);
            Assert.Equal(1,_httpContext.Items.Count);
            _auditDbContext.Object.EnableGlobalFilter(GlobalFilters.Site);
            Assert.Empty(_httpContext.Items);
        }

        [Fact]
        public void DisableGlobalFilterShouldSucceed()
        {
            Assert.Empty(_httpContext.Items);
            _auditDbContext.Object.DisableGlobalFilter(GlobalFilters.Site);
            Assert.Equal(1, _httpContext.Items.Count);
            Assert.True(_httpContext.Items.TryGetValue("IgnoreGlobalFilters", out object ignoreFilterObj));
            Assert.Equal(GlobalFilters.Site, (GlobalFilters)ignoreFilterObj);
        }

        [Fact]
        public void EnableGlobalFilterShouldNotSucceedIfAllFiltersAreDisabled()
        {
            Assert.Empty(_httpContext.Items);
            _httpContext.Items.Add(Claims.IGNORE_GLOBAL_FILTER, GlobalFilters.All);
            Assert.True(_httpContext.Items.TryGetValue("IgnoreGlobalFilters", out object ignoreFilterObj));
            Assert.Equal(GlobalFilters.All, (GlobalFilters)ignoreFilterObj);

            _auditDbContext.Object.EnableGlobalFilter(GlobalFilters.Site);
            Assert.True(_httpContext.Items.TryGetValue("IgnoreGlobalFilters", out object ignoreFilterObj2));
            Assert.Equal(GlobalFilters.All, (GlobalFilters)ignoreFilterObj2);
        }

        [Fact]
        public void DisableGlobalFilterShouldNotSucceedIfAllFiltersAreEnabled()
        {
            Assert.Empty(_httpContext.Items);
            _auditDbContext.Object.DisableGlobalFilter(GlobalFilters.All);
            Assert.Equal(1, _httpContext.Items.Count);
            Assert.True(_httpContext.Items.TryGetValue("IgnoreGlobalFilters", out object ignoreFilterObj));
            Assert.Equal(GlobalFilters.All, (GlobalFilters)ignoreFilterObj);

            _auditDbContext.Object.DisableGlobalFilter(GlobalFilters.Site);
            Assert.Equal(1, _httpContext.Items.Count);
            Assert.True(_httpContext.Items.TryGetValue("IgnoreGlobalFilters", out object ignoreFilterObj2));
            Assert.Equal(GlobalFilters.All, (GlobalFilters)ignoreFilterObj2);
        }
    }
}
