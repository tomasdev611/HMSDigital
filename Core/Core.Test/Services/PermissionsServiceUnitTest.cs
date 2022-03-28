using Core.Test.MockProvider;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class PermissionsServiceUnitTest
    {
        private readonly IPermissionsService _permissionsService;

        public PermissionsServiceUnitTest()
        {
            var mockService = new MockServices();
            _permissionsService = mockService.GetService<IPermissionsService>();
        }

        [Fact]
        public async Task GetPermissionsShouldReturnValidList()
        {
            var permissionsResult = await _permissionsService.GetAllPermissions();
            Assert.NotEmpty(permissionsResult);
        }
    }
}
