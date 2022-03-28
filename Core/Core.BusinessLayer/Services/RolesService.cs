using AutoMapper;
using HMSDigital.Core.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using Microsoft.Extensions.Logging;
using HMSDigital.Core.ViewModels;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.BusinessLayer.Enums;
using CoreDataModel = HMSDigital.Core.Data.Models;
using Sieve.Models;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        private readonly IMapper _mapper;

        private readonly IUsersRepository _usersRepository;

        private readonly IPaginationService _paginationService;

        private readonly IUserService _userService;

        private readonly ILogger<RolesService> _logger;

        public RolesService(
            IRolesRepository rolesRepository,
            IMapper mapper,
            IUsersRepository usersRepository,
            IPaginationService paginationService,
            IUserService userService,
            ILogger<RolesService> logger)
        {
            _rolesRepository = rolesRepository;
            _mapper = mapper;
            _usersRepository = usersRepository;
            _paginationService = paginationService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IEnumerable<Role>> GetAllRoles(SieveModel sieveModel)
        {
            _rolesRepository.SieveModel = sieveModel;
            var roleModels = await _rolesRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Role>>(roleModels);
        }

        public async Task<Role> GetRoleById(int id)
        {
            var roleModel = await _rolesRepository.GetByIdAsync(id);
            return _mapper.Map<Role>(roleModel);
        }

        public async Task<IEnumerable<UserRole>> AddUserRole(int userId, UserRoleBase userRoleRequest)
        {
            try
            {
                var roleModel = await _rolesRepository.GetByIdAsync(userRoleRequest.RoleId);
                if (roleModel == null)
                {
                    throw new ValidationException($"RoleId ({userRoleRequest.RoleId}) is not valid");
                }
                var loggedInUserRoles = await _userService.GetLoggedInUserRoles();
                if (loggedInUserRoles != null && loggedInUserRoles.Count() > 0)
                {
                    var loggedInUserRoleLevel = loggedInUserRoles.Select(ur => ur.RoleLevel).Min();
                    if (roleModel.Level < loggedInUserRoleLevel)
                    {
                        throw new ValidationException($"Can not assign higher level roles:({roleModel.Id})");
                    }
                }
                if (!Enum.TryParse(userRoleRequest.ResourceType, true, out ResourceTypes validResourceType))
                {
                    throw new ValidationException($"Resource Type ({userRoleRequest.ResourceType}) is invalid");
                }

                if (roleModel.RoleType == RoleTypes.Hospice.ToString() && userRoleRequest.ResourceId.Equals("*"))
                {
                    throw new ValidationException($"Not allowed to assign all hospices/sites permission to hospice type role.");
                }

                var user = await _usersRepository.GetByIdAsync(userId);
                if (!user.UserRoles.Any(ur => ur.ResourceType.Equals(userRoleRequest.ResourceType, StringComparison.OrdinalIgnoreCase)
                                                                    && ur.RoleId == userRoleRequest.RoleId
                                                                    && ur.ResourceId == userRoleRequest.ResourceId))
                {
                    var userRoleModel = _mapper.Map<CoreDataModel.UserRoles>(userRoleRequest);
                    user.UserRoles.Add(userRoleModel);
                    await _usersRepository.UpdateAsync(user);
                }
                var userRoles = _mapper.Map<IEnumerable<UserRole>>(user.UserRoles);

                foreach(var userRole in userRoles)
                {
                    if(userRole.RoleId == roleModel.Id)
                    {
                        userRole.RoleName = roleModel.Name;
                        userRole.RoleLevel = roleModel.Level.Value;
                    }
                }
                return userRoles;
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Adding user role to user with Id ({userId}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Adding user role to user with Id ({userId}): {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<UserRole>> RemoveUserRole(int userId, int userRoleId)
        {
            try
            {
                var user = await _usersRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ValidationException($"User with id ({userId}) not found.");
                }
                var userRole = user.UserRoles.FirstOrDefault(ur => ur.Id == userRoleId);
                if (userRole == null)
                {
                    return _mapper.Map<IEnumerable<UserRole>>(user.UserRoles);
                }

                user.UserRoles.Remove(userRole);

                await _usersRepository.UpdateAsync(user);
                return _mapper.Map<IEnumerable<UserRole>>(user.UserRoles);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Removing user role from user with Id ({userId}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Removing user role from user with Id ({userId}): {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<UserRole>> RemoveUserRoles(int userId, string resourceType, int resourceId)
        {
            try
            {
                var user = await _usersRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ValidationException($"User with id ({userId}) not found.");
                }
                var userRoles = user.UserRoles.Where(ur => ur.ResourceType == resourceType && ur.ResourceId == resourceId.ToString()).ToList();
                if (userRoles.Count() == 0)
                {
                    return _mapper.Map<IEnumerable<UserRole>>(user.UserRoles);
                }
                foreach (var userRole in userRoles)
                {
                    user.UserRoles.Remove(userRole);
                }

                await _usersRepository.UpdateAsync(user);
                return _mapper.Map<IEnumerable<UserRole>>(user.UserRoles);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Removing user role from user with Id ({userId}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Removing user role from user with Id ({userId}): {ex.Message}");
                throw ex;
            }
        }
    }
}

