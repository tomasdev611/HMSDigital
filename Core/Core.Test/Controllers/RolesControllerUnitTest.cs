using Core.Test.MockProvider;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.API.Controllers;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Controllers
{
    public class RolesControllerUnitTest
    {
        private readonly RolesController _rolesController;

        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly MockServices _mockController;

        public RolesControllerUnitTest()
        {
            _mockController = new MockServices();
            _rolesController = _mockController.GetController<RolesController>();
            _sieveModel = new SieveModel();
            _mockViewModels = new MockViewModels();
        }

    }
}
