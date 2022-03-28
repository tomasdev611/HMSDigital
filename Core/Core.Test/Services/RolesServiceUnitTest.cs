using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using Sieve.Models;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class RolesServiceUnitTest
    {
        private readonly IRolesService _rolesService;

        private readonly MockViewModels _mockViewModels;

        private readonly SieveModel _sieveModel;

        public RolesServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _rolesService = mockService.GetService<IRolesService>();
            _sieveModel = new SieveModel();
        }

        [Fact]
        public async Task GetRolesShouldReturnValidList()
        {
            var rolesResult = await _rolesService.GetAllRoles(_sieveModel);
            Assert.NotEmpty(rolesResult);
        }
    }
}
