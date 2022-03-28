using AutoMapper;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Enums;
using CoreModel = HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Common.BusinessLayer.Constants;
using Microsoft.AspNetCore.Http;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class SiteMemberService : ISiteMemberService
    {
        private readonly ISiteMemberRepository _siteMemberRepository;

        private readonly IMapper _mapper;

        private readonly ILogger<SiteMemberService> _logger;

        private readonly IRolesRepository _rolesRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IPaginationService _paginationService;

        private readonly IUserService _userService;

        private readonly HttpContext _httpContext;

        private readonly IRolesService _rolesService;

        public SiteMemberService(ISiteMemberRepository siteMemberRepository,
            IMapper mapper,
            IRolesRepository rolesRepository,
            ISitesRepository sitesRepository,
            IPaginationService paginationService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            IRolesService rolesService,
            ILogger<SiteMemberService> logger)
        {
            _siteMemberRepository = siteMemberRepository;
            _mapper = mapper;
            _logger = logger;
            _rolesRepository = rolesRepository;
            _sitesRepository = sitesRepository;
            _paginationService = paginationService;
            _userService = userService;
            _httpContext = httpContextAccessor.HttpContext;
            _rolesService = rolesService;
        }

        public async Task<PaginatedList<SiteMember>> GetAllSiteMembers(int siteId, SieveModel sieveModel)
        {
            _siteMemberRepository.SieveModel = sieveModel;
            var totalRecords = await _siteMemberRepository.GetCountAsync(m => m.SiteId == siteId);
            var sitesMemberModels = await _siteMemberRepository.GetManyAsync(m => m.SiteId == siteId);
            var sitesMembers = _mapper.Map<IEnumerable<SiteMember>>(sitesMemberModels);
            return _paginationService.GetPaginatedList(sitesMembers, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<SiteMember> GetMySiteMember()
        {
            var loggedInUserId = GetLoggedInUserId();
            var siteMember = await _siteMemberRepository.GetAsync(sm => sm.UserId == loggedInUserId);
            if (siteMember == null)
            {
                throw new ValidationException($"Logged In user is not a valid site member");
            }
            siteMember.User.UserRoles = siteMember.User.UserRoles.Where(ur => ur.ResourceType == ResourceTypes.Site.ToString()
                                                                                             && ur.ResourceId == siteMember.SiteId.ToString()).ToList();
            return _mapper.Map<SiteMember>(siteMember);
        }

        public async Task<SiteMember> GetSiteMemberById(int siteId, int memberId)
        {
            var siteMemberModel = await _siteMemberRepository.GetAsync(m => m.Id == memberId &&
                                                                    m.SiteId == siteId);
            if (siteMemberModel != null)
            {
                siteMemberModel.User.UserRoles = siteMemberModel.User.UserRoles.Where(ur => ur.ResourceType == ResourceTypes.Site.ToString()
                                                                                             && ur.ResourceId == siteId.ToString()).ToList();
            }
            return _mapper.Map<SiteMember>(siteMemberModel);
        }

        public async Task<SiteMember> CreateSiteMember(int siteId, SiteMemberRequest siteMemberRequest)
        {
            try
            {
                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(siteMemberRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }
                var site = await _sitesRepository.GetByIdAsync(siteId);
                if (site == null)
                {
                    throw new ValidationException($"Site with Id ({siteId}) is invalid");
                }
                var memberCreateRequest = _mapper.Map<UserCreateRequest>(siteMemberRequest);

                var userModel = await _userService.CreateUser(memberCreateRequest);
                if (siteMemberRequest.RoleIds != null && siteMemberRequest.RoleIds.Count() > 0)
                {
                    memberCreateRequest.UserRoles = await GenerateSiteMemberRoleList(siteId, siteMemberRequest.RoleIds);
                    foreach (var userRoleRequest in memberCreateRequest.UserRoles)
                    {
                        await _rolesService.AddUserRole(userModel.Id, userRoleRequest);
                    }
                }

                var siteMemberModel = new CoreModel.SiteMembers()
                {
                    UserId = userModel.Id,
                    SiteId = siteId,
                    Designation = siteMemberRequest.Designation
                };
                await _siteMemberRepository.AddAsync(siteMemberModel);
                return _mapper.Map<SiteMember>(siteMemberModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Creating site member: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Creating site member: {ex.Message}");
                throw ex;
            }
        }

        public async Task<SiteMember> UpdateSiteMember(int siteId, int memberId, SiteMemberRequest siteMemberUpdateRequest)
        {
            try
            {
                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(siteMemberUpdateRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }
                var siteMemberModel = await _siteMemberRepository.GetAsync(m => m.Id == memberId &&
                                                                     m.SiteId == siteId);
                if (siteMemberModel == null)
                {
                    throw new ValidationException($"Site member with Id ({memberId}) not found");
                }

                var updatedUser = _mapper.Map<UserMinimal>(siteMemberUpdateRequest);

                var userModel = await _userService.UpdateUser(siteMemberModel.User.Id, updatedUser);

                if (siteMemberUpdateRequest.RoleIds != null && siteMemberUpdateRequest.RoleIds.Count() > 0)
                {
                    var userRoles = await GenerateSiteMemberRoleList(siteId, siteMemberUpdateRequest.RoleIds);
                    var userRoleModels = _mapper.Map<IEnumerable<CoreModel.UserRoles>>(userRoles).ToList();
                    foreach (var userRole in userRoleModels)
                    {
                        if (!siteMemberModel.User.UserRoles.Any(ur => ur.ResourceType.Equals(ResourceTypes.Site.ToString(), StringComparison.OrdinalIgnoreCase)
                                                                && ur.RoleId == userRole.RoleId
                                                                && ur.ResourceId == userRole.ResourceId))
                        {
                            siteMemberModel.User.UserRoles.Add(userRole);
                        }
                    }
                }
                siteMemberModel.Designation = siteMemberUpdateRequest.Designation;

                await _siteMemberRepository.UpdateAsync(siteMemberModel);
                return _mapper.Map<SiteMember>(siteMemberModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Updating site member: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Updating site member: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<Site>> GetMySites()
        {
            var loggedInUserId = GetLoggedInUserId();
            var siteMembers = await _siteMemberRepository.GetManyAsync(sm => sm.UserId == loggedInUserId);
            if (siteMembers == null || !siteMembers.Any())
            {
                throw new ValidationException($"Logged In user is not a valid site member");
            }

            return _mapper.Map<IEnumerable<Site>>(siteMembers.Select(x => x.Site));
        }

        private async Task<IEnumerable<UserRoleBase>> GenerateSiteMemberRoleList(int siteId, IEnumerable<int> roleIds)
        {
            var roles = await _rolesRepository.GetManyAsync(i => roleIds.Contains(i.Id));

            if (roles.Count() != roleIds.Count())
            {
                var invalidIds = roleIds.Except(roles.Select(i => i.Id));
                throw new ValidationException($"RoleIds ({string.Join(",", invalidIds)}) not valid");
            }
            var nonInternalRoles = roles.Where(r => r.RoleType != RoleTypes.Internal.ToString());
            if (nonInternalRoles != null && nonInternalRoles.Count() > 0)
            {
                throw new ValidationException($"RoleIds ({string.Join(",", nonInternalRoles.Select(r => r.Id))}) not related to site");
            }
            var userRoles = new List<UserRoleBase>();

            foreach (var roleId in roleIds)
            {
                userRoles.Add(new UserRoleBase()
                {
                    ResourceType = ResourceTypes.Site.ToString(),
                    ResourceId = siteId.ToString(),
                    RoleId = roleId
                });
            }
            return userRoles;
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
    }
}
