using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreModels = HMSDigital.Core.Data.Models;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class DriversService : IDriversService
    {
        private readonly IDriverRepository _driverRepository;

        private readonly IMapper _mapper;

        private readonly ILogger<DriversService> _logger;

        private readonly ISitesRepository _sitesRepository;

        private readonly HttpContext _httpContext;

        private readonly IPaginationService _paginationService;

        private readonly IUserService _userService;

        private readonly IRolesRepository _rolesRepository;

        private readonly IRolesService _rolesService;

        public DriversService(IMapper mapper,
            IDriverRepository driverRepository,
            ISitesRepository sitesRepository,
            IHttpContextAccessor httpContextAccessor,
            IPaginationService paginationService,
            IUserService userService,
            IRolesRepository rolesRepository,
            IRolesService rolesService,
            ILogger<DriversService> logger)
        {
            _mapper = mapper;
            _driverRepository = driverRepository;
            _sitesRepository = sitesRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
            _paginationService = paginationService;
            _userService = userService;
            _rolesRepository = rolesRepository;
            _rolesService = rolesService;
        }

        public async Task<PaginatedList<ViewModels.Driver>> GetAllDrivers(SieveModel sieveModel)
        {
            _driverRepository.SieveModel = sieveModel;
            var totalRecords = await _driverRepository.GetCountAsync(d => true);
            var driverModels = await _driverRepository.GetAllAsync();
            var drivers = _mapper.Map<IEnumerable<ViewModels.Driver>>(driverModels);
            return _paginationService.GetPaginatedList(drivers, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<Driver> GetMyDriver()
        {
            var loggedInUserId = GetLoggedInUserId();
            var driverModel = await _driverRepository.GetAsync(d => d.UserId == loggedInUserId);
            if (driverModel == null)
            {
                throw new ValidationException($"Logged In user is not a valid driver");
            }
            return _mapper.Map<Driver>(driverModel);
        }

        public async Task<Driver> UpdateMyVehicle(int vehicleId)
        {
            var loggedInUserId = GetLoggedInUserId();
            var driverModel = await _driverRepository.GetAsync(d => d.UserId == loggedInUserId);
            if (driverModel == null)
            {
                throw new ValidationException($"Logged In user is not a valid driver");
            }
            var vehicle = await _sitesRepository.GetAsync(v => v.Id == vehicleId && v.LocationType.ToLower() == "vehicle");
            if (vehicle == null)
            {
                throw new ValidationException($"Vehicle id({vehicleId}) is not valid");
            }
            var assignedDriver = await _driverRepository.GetAsync(v => v.CurrentVehicleId == vehicleId);

            if (assignedDriver != null)
            {
                assignedDriver.CurrentVehicleId = null;
                await _driverRepository.UpdateAsync(assignedDriver);
            }
            driverModel.CurrentVehicleId = vehicleId;
            await _driverRepository.UpdateAsync(driverModel);

            await _sitesRepository.RefreshSitesCache();

            return _mapper.Map<Driver>(driverModel);
        }
        public async Task<PaginatedList<ViewModels.Driver>> SearchDriversBySearchQuery(SieveModel sieveModel, string searchQuery)
        {
            _driverRepository.SieveModel = sieveModel;
            var totalRecords = await _driverRepository.GetCountAsync(d => d.User.FirstName.Contains(searchQuery) || d.User.LastName.Contains(searchQuery));
            var driverModels = await _driverRepository.GetManyAsync(d => d.User.FirstName.Contains(searchQuery) || d.User.LastName.Contains(searchQuery));
            var drivers = _mapper.Map<IEnumerable<ViewModels.Driver>>(driverModels);
            return _paginationService.GetPaginatedList(drivers, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<ViewModels.Driver> GetDriverById(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            return _mapper.Map<ViewModels.Driver>(driver);
        }

        public async Task<ViewModels.Driver> CreateDriver(DriverBase driverCreateRequest)
        {
            try
            {
                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(driverCreateRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }

                await ValidateSite(driverCreateRequest.CurrentSiteId, "Site");
                await ValidateSite(driverCreateRequest.CurrentVehicleId, "Vehicle");

                var userRequest = _mapper.Map<UserCreateRequest>(driverCreateRequest);

                var userModel = await _userService.CreateUser(userRequest);
                userRequest.UserRoles = await GenerateUserRoleList();
                if (userRequest.UserRoles != null)
                {
                    foreach (var userRoleRequest in userRequest.UserRoles)
                    {
                        await _rolesService.AddUserRole(userModel.Id, userRoleRequest);
                    }
                }
                var driverModel = new CoreModels.Drivers()
                {
                    UserId = userModel.Id,
                    CurrentSiteId = driverCreateRequest.CurrentSiteId,
                    CurrentVehicleId = driverCreateRequest.CurrentVehicleId
                };

                await RemoveAssignedDriver(driverCreateRequest.CurrentVehicleId);

                await _driverRepository.AddAsync(driverModel);

                await _sitesRepository.RefreshSitesCache();

                return _mapper.Map<ViewModels.Driver>(driverModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Creating driver: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Creating driver: {ex.Message}");
                throw ex;
            }
        }

        public async Task<ViewModels.Driver> UpdateDriver(int driverId, DriverBase driverUpdateRequest)
        {
            try
            {
                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(driverUpdateRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }
                var driverModel = await _driverRepository.GetByIdAsync(driverId);
                if (driverModel == null)
                {
                    throw new ValidationException($"Driver with Id ({driverId}) not found");
                }

                await ValidateSite(driverUpdateRequest.CurrentSiteId, "Site");
                await ValidateSite(driverUpdateRequest.CurrentVehicleId, "Vehicle");

                var updatedUser = _mapper.Map<UserMinimal>(driverUpdateRequest);
                var userModel = await _userService.UpdateUser(driverModel.User.Id, updatedUser);

                driverModel.CurrentSiteId = driverUpdateRequest.CurrentSiteId;
                driverModel.CurrentVehicleId = driverUpdateRequest.CurrentVehicleId;

                await RemoveAssignedDriver(driverUpdateRequest.CurrentVehicleId);
                await _driverRepository.UpdateAsync(driverModel);

                await _sitesRepository.RefreshSitesCache();

                return _mapper.Map<ViewModels.Driver>(driverModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Updating driver: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Updating driver: {ex.Message}");
                throw ex;
            }
        }

        public async Task DeleteDriver(int driverId)
        {
            var driverModel = await _driverRepository.GetByIdAsync(driverId);
            if (driverModel == null)
            {
                return;
            }
            var userRoles = await _userService.GetUserRoles(driverModel.UserId);
            var driverUserRoleIds = userRoles.Where(ur => string.Equals(ur.RoleName, RoleNames.DRIVER_ROLE_NAME)).Select(ur => ur.Id);
            foreach (var userRoleId in driverUserRoleIds)
            {
                await _rolesService.RemoveUserRole(driverModel.UserId, userRoleId);
            }
            await _sitesRepository.RefreshSitesCache();

            await _driverRepository.DeleteAsync(driverModel);
        }

        public async Task<IEnumerable<ViewModels.Driver>> SearchDrivers(ViewModels.DriversRequest driversRequest)
        {
            var predicate = PredicateBuilder.New<CoreModels.Drivers>(false);
            if (!string.IsNullOrEmpty(driversRequest.FirstName))
            {
                predicate.Or(d => d.User.FirstName == driversRequest.FirstName);
            }
            if (!string.IsNullOrEmpty(driversRequest.LastName))
            {
                predicate.Or(d => d.User.LastName == driversRequest.LastName);
            }
            if (driversRequest.Mobile != 0)
            {
                predicate.Or(d => d.User.PhoneNumber == driversRequest.Mobile);
            }
            if (!string.IsNullOrEmpty(driversRequest.Email))
            {
                predicate.Or(d => d.User.Email.Contains(driversRequest.Email));
            }

            var drivers = await _driverRepository.GetManyAsync(predicate);
            return _mapper.Map<IEnumerable<ViewModels.Driver>>(drivers);
        }

        public async Task<ViewModels.Driver> UpdateMyLocation(GeoLocation location)
        {
            var loggedInUserId = GetLoggedInUserId();
            var driverModel = await _driverRepository.GetAsync(d => d.UserId == loggedInUserId);
            if (driverModel == null)
            {
                throw new ValidationException($"Logged In user is not a valid driver");
            }

            driverModel.LastKnownLatitude = location.Latitude;
            driverModel.LastKnownLongitude = location.Longitude;
            driverModel.LocationUpdatedDateTime = DateTime.UtcNow;

            await _driverRepository.UpdateAsync(driverModel);
            return _mapper.Map<Driver>(driverModel);
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return -1;
            }
            return int.Parse(userIdClaim.Value);
        }

        private async Task<IEnumerable<UserRoleBase>> GenerateUserRoleList()
        {
            var role = await _rolesRepository.GetAsync(r => r.Name.ToLower() == RoleNames.DRIVER_ROLE_NAME.ToLower());
            if (role == null)
            {
                throw new ValidationException($"Driver type role not found");
            }
            return new List<UserRoleBase>()
                        {
                            new UserRoleBase()
                            {
                                ResourceType = ResourceTypes.Site.ToString(),
                                ResourceId = "*",
                                RoleId = role.Id
                            },
                            new UserRoleBase()
                            {
                                ResourceType = ResourceTypes.Hospice.ToString(),
                                ResourceId = "*",
                                RoleId = role.Id
                            }
                        };
        }

        private async Task ValidateSite(int? siteId, string type)
        {
            if (siteId == null)
            {
                return;
            }

            var site = await _sitesRepository.GetAsync(s => s.Id == siteId && s.LocationType.ToLower() == type.ToLower());
            if (site == null)
            {
                throw new ValidationException($"{type} with Id ({siteId}) is not valid");
            }
        }

        private async Task RemoveAssignedDriver(int? vehicleId)
        {
            if (vehicleId == null)
            {
                return;
            }

            var assignedDriver = await _driverRepository.GetAsync(v => v.CurrentVehicleId.HasValue && v.CurrentVehicleId.Value == vehicleId.Value);

            if (assignedDriver != null)
            {
                assignedDriver.CurrentVehicleId = null;
                await _driverRepository.UpdateAsync(assignedDriver);
            }
        }

    }
}
