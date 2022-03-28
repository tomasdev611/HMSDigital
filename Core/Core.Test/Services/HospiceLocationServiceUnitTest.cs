using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class HospiceLocationServiceUnitTest
    {
        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly IHospiceLocationService _hospiceLocationService;

        private readonly IHospiceService _hospiceService;

        public HospiceLocationServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _hospiceLocationService = mockService.GetService<IHospiceLocationService>();
            _hospiceService = mockService.GetService<IHospiceService>();
            _sieveModel = new SieveModel();
        }

        [Fact]
        public async Task GetAllHospiceLocationsShouldReturnValidList()
        {
            var hospiceLocations = await _hospiceLocationService.GetAllHospiceLocations(1, _sieveModel);
            Assert.NotEmpty(hospiceLocations.Records);
        }
    }
}
