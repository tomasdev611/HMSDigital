using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Formatter;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CoreModel = HMSDigital.Core.Data.Models;
using NSSDKViewModel = HospiceSource.Digital.NetSuite.SDK.ViewModels;
using NSViewModel = HMSDigital.Core.ViewModels.NetSuite;
using SystemValidation = System.ComponentModel.DataAnnotations;

[assembly: InternalsVisibleTo("Core.Test")]
namespace HMSDigital.Core.BusinessLayer.Services
{
    public class HospiceMemberService : IHospiceMemberService
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IMapper _mapper;

        private readonly ILogger<HospiceMemberService> _logger;

        private readonly IRolesRepository _rolesRepository;

        private readonly IUserService _userService;

        private readonly IFileService _fileService;

        private readonly HttpContext _httpContext;

        private readonly ICsvMappingRepository _csvMappingRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly IHospiceMemberRepository _hospiceMemberRepository;

        private readonly IPaginationService _paginationService;

        private readonly INetSuiteService _netSuiteService;

        private readonly IRolesService _rolesService;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IHospiceLocationMemberRepository _hospiceLocationMemberRepository;

        private readonly NetSuiteConfig _netSuiteConfig;

        private readonly IDbContextFactoryService _dbContextFactoryService;

        public HospiceMemberService(IUsersRepository usersRepository,
            IMapper mapper,
            IRolesRepository rolesRepository,
            IUserService userService,
            IFileService fileService,
            IHttpContextAccessor httpContextAccessor,
            ICsvMappingRepository csvMappingRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            IHospiceMemberRepository hospiceMemberRepository,
            IPaginationService paginationService,
            INetSuiteService netSuiteService,
            IRolesService rolesService,
            IHospiceRepository hospiceRepository,
            IHospiceLocationMemberRepository hospiceLocationMemberRepository,
            IOptions<NetSuiteConfig> netSuiteOptions,
            IDbContextFactoryService dbContextFactoryService,
            ILogger<HospiceMemberService> logger)
        {
            _mapper = mapper;
            _usersRepository = usersRepository;
            _logger = logger;
            _rolesRepository = rolesRepository;
            _userService = userService;
            _fileService = fileService;
            _httpContext = httpContextAccessor.HttpContext;
            _csvMappingRepository = csvMappingRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _hospiceMemberRepository = hospiceMemberRepository;
            _paginationService = paginationService;
            _netSuiteService = netSuiteService;
            _rolesService = rolesService;
            _hospiceRepository = hospiceRepository;
            _hospiceLocationMemberRepository = hospiceLocationMemberRepository;
            _netSuiteConfig = netSuiteOptions.Value;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task<PaginatedList<ViewModels.HospiceMember>> GetAllHospiceMembers(int hospiceId, SieveModel sieveModel, string roleName)
        {
            _usersRepository.SieveModel = sieveModel;
            _hospiceMemberRepository.SieveModel = sieveModel;
            var predicate = PredicateBuilder.New<CoreModel.HospiceMember>(true);
            if (hospiceId != 0)
            {
                predicate.And(m => m.HospiceId == hospiceId);
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                predicate.And(m => m.User.UserRoles.Any(ur => ur.Role.Name.ToLower() == roleName.ToLower()));
            }
            var totalRecords = await _hospiceMemberRepository.GetCountAsync(predicate);
            var hospiceMemberModels = await _hospiceMemberRepository.GetManyAsync(predicate);
            var hospiceMembersMapping = await _csvMappingRepository.GetAsync(cm => cm.MappedTable == MappedItemTypes.HospiceMember.ToString() &&
                                                                             cm.HospiceId == hospiceId &&
                                                                             cm.MappingType == CsvMappingTypes.Output.ToString());
            if (hospiceMembersMapping != null && !string.IsNullOrEmpty(hospiceMembersMapping.MappingInJson))
            {
                _httpContext.Items.Add(CsvMapping.MAPPING_DICTIONARY, hospiceMembersMapping.MappingInJson);
            }
            var hospiceMembers = _mapper.Map<IEnumerable<ViewModels.HospiceMember>>(hospiceMemberModels);
            return _paginationService.GetPaginatedList(hospiceMembers, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<HospiceApprover> GetApproverContacts()
        {
            var userId = GetLoggedInUserId();
            var hospiceMemberModels = await _hospiceMemberRepository.GetManyAsync(m => m.UserId == userId);
            return new HospiceApprover()
            {
                HospiceIds = hospiceMemberModels.Where(hm => hm.CanApproveOrder.HasValue && hm.CanApproveOrder.Value).Select(hm => hm.HospiceId),
                HospiceLocationIds = hospiceMemberModels.SelectMany(hm => hm.HospiceLocationMembers).Where(hlm => hlm.HospiceLocationId.HasValue && hlm.CanApproveOrder.HasValue && hlm.CanApproveOrder.Value).Select(hm => hm.HospiceLocationId.Value)
            };
        }

        public async Task<ViewModels.HospiceMember> GetHospiceMemberById(int hospiceId, int memberId)
        {
            var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(m => m.Id == memberId &&
                                                                    m.HospiceId == hospiceId);

            if (hospiceMemberModel != null)
            {
                var allowedHospiceLocations = await _hospiceLocationRepository.GetManyAsync(l => l.HospiceId == hospiceMemberModel.HospiceId);
                var allowedHospiceLocationIds = allowedHospiceLocations.Select(l => l.Id);
                hospiceMemberModel.User.UserRoles = hospiceMemberModel.User.UserRoles?.Where(ur => (ur.ResourceType == ResourceTypes.Hospice.ToString() && ur.ResourceId == hospiceId.ToString())
                                                                                                    || (int.TryParse(ur.ResourceId, out int resourceIdValue)
                                                                                                        && allowedHospiceLocationIds.Contains(resourceIdValue)
                                                                                                        && ur.ResourceType == ResourceTypes.HospiceLocation.ToString())
                                                                                                    ).ToList();
            }
            return _mapper.Map<ViewModels.HospiceMember>(hospiceMemberModel);
        }

        public async Task<ViewModels.HospiceMember> CreateHospiceMember(int hospiceId, HospiceMemberRequest hospiceMemberRequest)
        {
            try
            {
                var existingMember = await _hospiceMemberRepository.GetAsync(m => m.HospiceId == hospiceId
                                                                            && m.User.Email == hospiceMemberRequest.Email);
                if (existingMember != null)
                {
                    return await UpdateHospiceMember(hospiceId, existingMember.Id, hospiceMemberRequest);
                }
                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(hospiceMemberRequest);
                if (!validationResult.IsValid)
                {
                    throw new SystemValidation.ValidationException(validationResult.Errors[0].ErrorMessage);
                }

                var hospice = await _hospiceRepository.GetByIdAsync(hospiceId);
                if (hospice == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice Id ({hospiceId}) not valid");
                }

                var memberCreateRequest = _mapper.Map<UserCreateRequest>(hospiceMemberRequest);

                var validHospiceLocationIds = hospice.HospiceLocations.Select(l => l.Id);
                if (hospiceMemberRequest.UserRoles != null)
                {
                    memberCreateRequest.UserRoles = await GenerateHospiceMemberRoleList(hospiceId, validHospiceLocationIds, hospiceMemberRequest.UserRoles);
                }

                var userModel = await _userService.CreateUser(memberCreateRequest);
                if (hospiceMemberRequest.UserRoles != null)
                {
                    foreach (var userRoleRequest in memberCreateRequest.UserRoles)
                    {
                        await _rolesService.AddUserRole(userModel.Id, userRoleRequest);
                    }
                }
                bool isAdmin = false;
                if (memberCreateRequest.UserRoles != null && memberCreateRequest.UserRoles.Any(ur => ur.RoleId == (int)Enums.Roles.HospiceAdmin && ur.ResourceId == hospiceId.ToString()))
                {
                    isAdmin = true;
                }

                var locationMembers = await GenerateHospiceLocationMemberList(null, hospiceMemberRequest);
                var hospiceMemberModel = new CoreModel.HospiceMember()
                {
                    UserId = userModel.Id,
                    HospiceId = hospiceId,
                    Designation = hospiceMemberRequest.Designation,
                    CanAccessWebStore = hospiceMemberRequest.CanAccessWebStore,
                    HospiceLocationMembers = locationMembers?.ToList()
                };

                await _hospiceMemberRepository.AddAsync(hospiceMemberModel);
                CreateNetSuiteContact(hospice.NetSuiteCustomerId, hospiceMemberRequest, isAdmin, hospiceMemberModel.Id);
                return _mapper.Map<ViewModels.HospiceMember>(hospiceMemberModel);
            }
            catch (SystemValidation.ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Creating hospice member: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Creating hospice member: {ex.Message}");
                throw ex;
            }
        }

        public async Task<ViewModels.HospiceMember> UpdateHospiceMember(int hospiceId, int memberId, HospiceMemberRequest memberUpdateRequest)
        {
            try
            {
                IEnumerable<UserRoleBase> generatedUserRoles = null;
                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(memberUpdateRequest);
                if (!validationResult.IsValid)
                {
                    throw new SystemValidation.ValidationException(validationResult.Errors[0].ErrorMessage);
                }
                var hospice = await _hospiceRepository.GetByIdAsync(hospiceId);
                if (hospice == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice Id ({hospiceId}) not valid");
                }
                var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(m => m.Id == memberId &&
                                                                     m.HospiceId == hospiceId);
                if (hospiceMemberModel == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice member with Id ({memberId}) not found");
                }

                var validHospiceLocationIds = hospice.HospiceLocations.Select(l => l.Id);
                if (memberUpdateRequest.UserRoles != null && memberUpdateRequest.UserRoles.Count() > 0)
                {
                    generatedUserRoles = await GenerateHospiceMemberRoleList(hospiceId, validHospiceLocationIds, memberUpdateRequest.UserRoles);
                }

                var updatedUser = _mapper.Map<UserMinimal>(memberUpdateRequest);

                var userModel = await _userService.UpdateUser(hospiceMemberModel.User.Id, updatedUser);

                var userRoles = hospiceMemberModel.User.UserRoles.Where(ur => !(ur.ResourceId == hospiceId.ToString()
                                                                                    && ur.ResourceType == ResourceTypes.Hospice.ToString())
                                                                                    && !(int.TryParse(ur.ResourceId, out int resourceIdValue)
                                                                                    && validHospiceLocationIds.Contains(resourceIdValue)
                                                                                    && ur.ResourceType == ResourceTypes.HospiceLocation.ToString()));
                hospiceMemberModel.User.UserRoles = userRoles.ToList();

                if (memberUpdateRequest.UserRoles != null && memberUpdateRequest.UserRoles.Count() > 0)
                {
                    var userRoleModels = _mapper.Map<IEnumerable<UserRoles>>(generatedUserRoles);
                    hospiceMemberModel.User.UserRoles = hospiceMemberModel.User.UserRoles.Concat(userRoleModels).ToList();
                }

                var locationMembers = await GenerateHospiceLocationMemberList(hospiceMemberModel, memberUpdateRequest);
                hospiceMemberModel.Designation = memberUpdateRequest.Designation;
                hospiceMemberModel.CanAccessWebStore = memberUpdateRequest.CanAccessWebStore;
                hospiceMemberModel.HospiceLocationMembers = locationMembers?.ToList();

                if (hospice.NetSuiteCustomerId.HasValue)
                {
                    var hospiceAdminRole = await _rolesRepository.GetByIdAsync((int)Enums.Roles.HospiceAdmin);
                    var customerContactRequest = _mapper.Map<NSSDKViewModel.CustomerContact>(memberUpdateRequest);

                    customerContactRequest.NetSuiteCustomerId = hospice.NetSuiteCustomerId.Value;
                    if (hospiceMemberModel.User.UserRoles != null && hospiceMemberModel.User.UserRoles.Any(ur => ur.RoleId == hospiceAdminRole.Id))
                    {
                        customerContactRequest.IsAdmin = true;
                    }
                    if (hospiceMemberModel.NetSuiteContactId.HasValue)
                    {
                        customerContactRequest.NetSuiteContactId = hospiceMemberModel.NetSuiteContactId.Value;
                        await _netSuiteService.UpdateCustomerContact(customerContactRequest);
                    }
                    else
                    {
                        var customerContact = await _netSuiteService.CreateCustomerContact(customerContactRequest);
                        hospiceMemberModel.NetSuiteContactId = customerContact?.NetSuiteContactId;
                    }
                }

                await _hospiceMemberRepository.UpdateAsync(hospiceMemberModel);
                return _mapper.Map<ViewModels.HospiceMember>(hospiceMemberModel);
            }
            catch (SystemValidation.ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Updating hospice member: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Updating hospice member: {ex.Message}");
                throw ex;
            }
        }

        public async Task UpsertApprovers(NSViewModel.ApproverRequest approverRequest)
        {
            try
            {
                var hospiceModel = await _hospiceRepository.GetAsync(h => h.NetSuiteCustomerId == approverRequest.NetSuiteCustomerId);
                IEnumerable<CoreModel.HospiceMember> hospiceMembers;
                if (hospiceModel != null)
                {
                    hospiceMembers = await _hospiceMemberRepository.GetManyAsync(m => m.HospiceId == hospiceModel.Id && m.NetSuiteContactId != null);

                    var invalidContactIds = approverRequest.Approvers.Except(hospiceMembers.Select(i => i.NetSuiteContactId.Value));
                    if (invalidContactIds != null && invalidContactIds.Count() > 0)
                    {
                        throw new SystemValidation.ValidationException($"Approver contact Ids ({string.Join(",", invalidContactIds)}) are not valid");
                    }

                    var approverMembers = hospiceMembers.Where(m => approverRequest.Approvers.Contains(m.NetSuiteContactId.Value));
                    foreach (var member in approverMembers)
                    {
                        member.CanApproveOrder = true;
                    }

                    var nonApproverMembers = hospiceMembers.Where(m => !approverRequest.Approvers.Contains(m.NetSuiteContactId.Value));
                    foreach (var member in nonApproverMembers)
                    {
                        member.CanApproveOrder = false;
                    }

                }
                else
                {
                    var hospiceLocationModel = await _hospiceLocationRepository.GetAsync(l => l.NetSuiteCustomerId == approverRequest.NetSuiteCustomerId);
                    if (hospiceLocationModel == null)
                    {
                        throw new SystemValidation.ValidationException($"Customer id ({approverRequest.NetSuiteCustomerId}) is invalid");
                    }

                    hospiceMembers = await _hospiceMemberRepository.GetManyAsync(m => m.HospiceLocationMembers.Any(lm => lm.HospiceLocationId == hospiceLocationModel.Id)
                                                                                    && m.NetSuiteContactId != null);
                    var locationMembers = hospiceMembers.SelectMany(m => m.HospiceLocationMembers);

                    var invalidContactIds = approverRequest.Approvers.Except(locationMembers.Select(i => i.NetSuiteContactId.Value));
                    if (invalidContactIds != null && invalidContactIds.Count() > 0)
                    {
                        throw new SystemValidation.ValidationException($"Approver contact Ids ({string.Join(",", invalidContactIds)}) are not valid");
                    }
                    var approverLocationMembers = locationMembers.Where(m => approverRequest.Approvers.Contains(m.NetSuiteContactId.Value));
                    foreach (var member in approverLocationMembers)
                    {
                        member.CanApproveOrder = true;
                    }

                    var nonApproverLocationMembers = locationMembers.Where(m => !approverRequest.Approvers.Contains(m.NetSuiteContactId.Value));
                    foreach (var member in nonApproverLocationMembers)
                    {
                        member.CanApproveOrder = false;
                    }
                }

                await _hospiceMemberRepository.UpdateManyAsync(hospiceMembers);
            }
            catch (SystemValidation.ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Creating hospice with location and facility: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Creating hospice with location and facility: {ex.Message}");
                throw ex;
            }
        }

        public async Task<NSViewModel.HospiceContactResponse> UpdateHospiceContact(NSViewModel.HospiceContactRequest hospiceContactRequest)
        {
            var hospiceContact = await _hospiceMemberRepository.GetAsync(m => m.NetSuiteContactId == hospiceContactRequest.NetSuiteCustomerContactId);
            if (hospiceContact == null)
            {
                throw new SystemValidation.ValidationException($"Hospice contact with Net Suite Customer Contact Id ({hospiceContactRequest.NetSuiteCustomerContactId}) not found");
            }
            var hospiceMemberRequest = _mapper.Map<HospiceMemberRequest>(hospiceContactRequest);
            hospiceMemberRequest.CanAccessWebStore = hospiceContact.CanAccessWebStore ?? false;
            var hospiceMember = await UpdateHospiceMember(hospiceContact.HospiceId, hospiceContact.Id, hospiceMemberRequest);
            return _mapper.Map<NSViewModel.HospiceContactResponse>(hospiceMember);
        }

        public async Task<(IEnumerable<ViewModels.HospiceMember>, IEnumerable<HospiceMemberCsvRequest>, ValidatedList<HospiceMemberCsvRequest>)> CreateHospiceMembersFromCsvFile(
            int hospiceId,
            IFormFile members,
            bool validateOnly,
            bool parseOnly)
        {
            try
            {
                var hospiceMembersMapping = await _csvMappingRepository.GetAsync(cm =>
                                                        cm.MappedTable == MappedItemTypes.HospiceMember.ToString() &&
                                                        cm.HospiceId == hospiceId &&
                                                        cm.MappingType == CsvMappingTypes.Input.ToString());
                CsvHeaderMap<HospiceMemberCsvRequest> csvHeaderMap = null;
                if (hospiceMembersMapping != null)
                {
                    var mapping = JsonConvert.DeserializeObject<CsvMapping<InputMappedItem>>(hospiceMembersMapping.MappingInJson);
                    csvHeaderMap = new CsvHeaderMap<HospiceMemberCsvRequest>(mapping);
                }
                else
                {
                    csvHeaderMap = new CsvHeaderMap<HospiceMemberCsvRequest>(CsvMapping.GetInputCsvMapping(MappedItemTypes.HospiceMember));
                }
                var hospiceMembers = _fileService.GetMappedRecords(members, csvHeaderMap);
                if (parseOnly)
                {
                    return (null, hospiceMembers, null);
                }
                var (createdHospiceMembers, validatedList) = await CreateHospiceMembers(hospiceId, hospiceMembers, validateOnly);
                return (createdHospiceMembers, null, validatedList);
            }
            catch (SystemValidation.ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Uploading hospice members: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Uploading hospice members: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewModels.HospiceMember>> BulkUploadMembers(int hospiceId, IEnumerable<HospiceMemberRequest> hospiceMemberRequests)
        {
            var hospiceMembers = new List<ViewModels.HospiceMember>();
            foreach (var memberRequest in hospiceMemberRequests)
            {
                hospiceMembers.Add(await CreateHospiceMember(hospiceId, memberRequest));
            }
            return hospiceMembers;
        }

        public async Task DeleteHospiceMember(int hospiceId, int memberId)
        {
            try
            {
                var hospice = await _hospiceRepository.GetByIdAsync(hospiceId);
                if (hospice == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice Id ({hospiceId}) not valid");
                }

                var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(m => m.Id == memberId &&
                                                                     m.HospiceId == hospiceId);
                if (hospiceMemberModel != null)
                {
                    var memberLocations = hospiceMemberModel.HospiceLocationMembers.ToList();
                    foreach (var memberLocation in memberLocations)
                    {
                        await _rolesService.RemoveUserRoles(hospiceMemberModel.UserId, Data.Enums.ResourceTypes.HospiceLocation.ToString(), memberLocation.HospiceLocationId.Value);
                        if (memberLocation.NetSuiteContactId != null)
                        {
                            await _netSuiteService.DeleteCustomerContact(memberLocation.NetSuiteContactId.Value);
                            await _hospiceLocationMemberRepository.DeleteAsync(memberLocation);
                        }
                    }
                    if (hospiceMemberModel.NetSuiteContactId != null)
                    {
                        await _netSuiteService.DeleteCustomerContact(hospiceMemberModel.NetSuiteContactId.Value);
                    }
                    await _rolesService.RemoveUserRoles(hospiceMemberModel.UserId, Data.Enums.ResourceTypes.Hospice.ToString(), hospice.Id);

                    await _hospiceMemberRepository.DeleteAsync(hospiceMemberModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while deleting hospice member: {ex.Message}");
                throw ex;
            }
        }

        public async Task<ViewModels.HospiceMember> CreateInternalHospiceMember(User user)
        {
            var hospiceModel = await _hospiceRepository.GetAsync(h => h.NetSuiteCustomerId == _netSuiteConfig.InternalUsersHostCustomerId);
            if (hospiceModel == null)
            {
                _logger.LogWarning($"InternalUsersHostCustomerId does not have any corresponding hospice");
                return null;
            }
            var hospiceMemberRequest = new HospiceMemberRequest()
            {
                CanAccessWebStore = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CountryCode = user.CountryCode,
                UserRoles = new List<HospiceMemberRoleRequest>()
                                    {
                                        new HospiceMemberRoleRequest()
                                        {
                                            RoleId = (int) Enums.Roles.HospiceAdmin,
                                            ResourceType = ResourceTypes.Hospice.ToString(),
                                            ResourceId = hospiceModel.Id
                                        }
                                    }
            };
            return await CreateHospiceMember(hospiceModel.Id, hospiceMemberRequest);
        }

        public async Task DeleteInternalHospiceMember(int userId)
        {
            var hospiceModel = await _hospiceRepository.GetAsync(h => h.NetSuiteCustomerId == _netSuiteConfig.InternalUsersHostCustomerId);
            if (hospiceModel == null)
            {
                _logger.LogWarning($"InternalUsersHostCustomerId does not have any corresponding hospice");
                return;
            }
            var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(hm => hm.HospiceId == hospiceModel.Id
                                                                                && hm.UserId == userId);
            if (hospiceMemberModel == null)
            {
                return;
            }
            await DeleteHospiceMember(hospiceModel.Id, hospiceMemberModel.Id);
        }

        public async Task<ViewModels.HospiceMember> UpdateHospiceMemberInNetSuite(int userId)
        {
            var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(m => m.UserId == userId);
            if (hospiceMemberModel == null)
            {
                return null;
            }
            var hospice = await _hospiceRepository.GetByIdAsync(hospiceMemberModel.HospiceId);
            var memberUpdateRequest = new HospiceMemberRequest()
            {
                FirstName = hospiceMemberModel.User.FirstName,
                LastName = hospiceMemberModel.User.LastName,
                Email = hospiceMemberModel.User.Email,
                PhoneNumber = hospiceMemberModel.User.PhoneNumber,
                CanAccessWebStore = hospiceMemberModel.CanAccessWebStore ?? false,
                UserRoles = _mapper.Map<IEnumerable<HospiceMemberRoleRequest>>(hospiceMemberModel.User.UserRoles)
            };

            var locationMembers = await GenerateHospiceLocationMemberList(hospiceMemberModel, memberUpdateRequest);
            hospiceMemberModel.Designation = memberUpdateRequest.Designation;
            hospiceMemberModel.CanAccessWebStore = memberUpdateRequest.CanAccessWebStore;
            hospiceMemberModel.HospiceLocationMembers = locationMembers?.ToList();

            if (hospice.NetSuiteCustomerId.HasValue)
            {
                var hospiceAdminRole = await _rolesRepository.GetByIdAsync((int)Enums.Roles.HospiceAdmin);
                var customerContactRequest = _mapper.Map<NSSDKViewModel.CustomerContact>(memberUpdateRequest);

                customerContactRequest.NetSuiteCustomerId = hospice.NetSuiteCustomerId.Value;
                if (hospiceMemberModel.User.UserRoles != null && hospiceMemberModel.User.UserRoles.Any(ur => ur.RoleId == hospiceAdminRole.Id))
                {
                    customerContactRequest.IsAdmin = true;
                }
                if (hospiceMemberModel.NetSuiteContactId.HasValue)
                {
                    customerContactRequest.NetSuiteContactId = hospiceMemberModel.NetSuiteContactId.Value;
                    await _netSuiteService.UpdateCustomerContact(customerContactRequest);
                }
                else
                {
                    var customerContact = await _netSuiteService.CreateCustomerContact(customerContactRequest);
                    hospiceMemberModel.NetSuiteContactId = customerContact?.NetSuiteContactId;
                }
            }
            await _hospiceMemberRepository.UpdateAsync(hospiceMemberModel);

            return _mapper.Map<ViewModels.HospiceMember>(hospiceMemberModel);
        }

        public async Task SetMemberPassword(int hospiceId, int memberId, UserPasswordRequest userPasswordRequest)
        {
            try
            {
                var hospice = await _hospiceRepository.GetByIdAsync(hospiceId);
                if (hospice == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice Id ({hospiceId}) not valid");
                }
                var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(m => m.Id == memberId &&
                                                                     m.HospiceId == hospiceId);
                if (hospiceMemberModel == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice member with Id ({memberId}) not found");
                }

                await _userService.SetUserPassword(hospiceMemberModel.UserId, userPasswordRequest);
            }
            catch (SystemValidation.ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while resetting password of hospice member: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while resetting password of hospice member: {ex.Message}");
                throw ex;
            }
        }

        public async Task SendMemberPasswordResetLink(int hospiceId, int memberId, NotificationBase resetPasswordNotification)
        {
            try
            {
                var hospice = await _hospiceRepository.GetByIdAsync(hospiceId);
                if (hospice == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice Id ({hospiceId}) not valid");
                }
                var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(m => m.Id == memberId &&
                                                                     m.HospiceId == hospiceId);
                if (hospiceMemberModel == null)
                {
                    throw new SystemValidation.ValidationException($"Hospice member with Id ({memberId}) not found");
                }

                await _userService.SendPasswordResetLink(hospiceMemberModel.UserId, resetPasswordNotification);
            }
            catch (SystemValidation.ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while sending password reset link to hospice member: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while sending password reset link to hospice member: {ex.Message}");
                throw ex;
            }
        }

        internal async Task<(IEnumerable<ViewModels.HospiceMember>, ValidatedList<HospiceMemberCsvRequest>)> CreateHospiceMembers(
            int hospiceId,
            IEnumerable<HospiceMemberCsvRequest> hospiceMembers,
            bool validateOnly)
        {
            var validator = new ListValidator<HospiceMemberValidator, HospiceMemberCsvRequest>();
            var validatedMembersList = validator.Validate(hospiceMembers);

            if (!validatedMembersList.IsValid)
            {
                return (null, validatedMembersList);
            }

            var emailIds = hospiceMembers.Select(hm => hm.Email).ToList();
            var existEmailIds = await _usersRepository.GetManyAsync(u => emailIds.Contains(u.Email));

            var duplicateEmailIds = emailIds.GroupBy(i => i).Where(ig => ig.Count() > 1);
            if (duplicateEmailIds.Count() > 0)
            {
                var ErrorMessage = "Duplicate member with the same email";
                validatedMembersList = validator.GetFormatedList(hospiceMembers, ErrorMessage, duplicateEmailIds.Select(i => i.Key),
                                                                m => m.Email);
                return (null, validatedMembersList);
            }

            var locationListFromCsv = hospiceMembers.Select(hm => hm.HospiceLocation)
                                                    .Where(i => !string.IsNullOrEmpty(i))
                                                    .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var locations = await _hospiceLocationRepository.GetManyAsync(l => locationListFromCsv.Contains(l.Name));
            if (locationListFromCsv.Count() != locations.Count())
            {
                var invalidlocations = locationListFromCsv.Except(locations.Select(i => i.Name));
                var ErrorMessage = "Invalid hospice location";
                var InvalidLocationEmailId = hospiceMembers.Where(u => invalidlocations.Contains(u.HospiceLocation)).Select(f => f.Email);
                validatedMembersList = validator.GetFormatedList(hospiceMembers, ErrorMessage, InvalidLocationEmailId, u => u.Email);
                return (null, validatedMembersList);
            }

            IEnumerable<CoreModel.Roles> roles = null;
            var hospiceMemberWithRoles = hospiceMembers.Where(hm => !string.IsNullOrEmpty(hm.Role));
            if (hospiceMemberWithRoles != null && hospiceMemberWithRoles.Count() > 0)
            {

                var roleNames = hospiceMemberWithRoles.Select(h => h.Role).Distinct();
                if (roleNames != null && roleNames.Count() > 0)
                {
                    roles = await _rolesRepository.GetManyAsync(i => roleNames.Contains(i.Name));
                    if (roles.Count() != roleNames.Count())
                    {
                        var invalidRoles = roleNames.Except(roles.Select(i => i.Name));
                        var InvalidRoleEmailId = hospiceMembers.Where(u => invalidRoles.Contains(u.Role)).Select(f => f.Email);
                        var ErrorMessage = $"Invalid Role";
                        validatedMembersList = validator.GetFormatedList(hospiceMembers, ErrorMessage, InvalidRoleEmailId, u => u.Email);
                        return (null, validatedMembersList);
                    }

                    var nonHospiceRoles = roles.Where(r => r.RoleType != RoleTypes.Hospice.ToString()).Select(r => r.Name).ToList();
                    if (nonHospiceRoles != null && nonHospiceRoles.Count() > 0)
                    {
                        var nonHospiceRoleEmailId = hospiceMembers.Where(u => nonHospiceRoles.Contains(u.Role)).Select(f => f.Email);
                        var ErrorMessage = $"Roles not related to hospice";
                        validatedMembersList = validator.GetFormatedList(hospiceMembers, ErrorMessage, nonHospiceRoleEmailId, u => u.Email);
                        return (null, validatedMembersList);
                    }
                }
            }

            if (validateOnly)
            {
                return (null, validatedMembersList);
            }
            var hospiceMemberCsvList = _mapper.Map<IEnumerable<HospiceMemberCsvDTO>>(hospiceMembers);
            var standardHospiceRole = await _rolesRepository.GetByIdAsync((int)Enums.Roles.StandardHospiceUser);
            var hospiceLocations = await _hospiceLocationRepository.GetManyAsync(l => l.HospiceId == hospiceId);
            foreach (var member in hospiceMemberCsvList)
            {
                int roleId;
                var userRoles = new List<HospiceMemberRoleRequest>();
                if (!string.IsNullOrEmpty(member.Role) && roles != null)
                {
                    var memberRole = roles.FirstOrDefault(r => r.Name.Equals(member.Role, StringComparison.OrdinalIgnoreCase));
                    roleId = memberRole.Id;
                }
                else
                {
                    roleId = standardHospiceRole.Id;
                }
                if (!string.IsNullOrEmpty(member.HospiceLocation) && locations != null)
                {
                    member.HospiceLocationIds = locations.Where(l => l.Name.Equals(member.HospiceLocation, StringComparison.OrdinalIgnoreCase))
                                                            .Select(i => i.Id);
                }
                member.CanAccessWebStore = true;
                member.CountryCode = 1;
                userRoles.Add(new HospiceMemberRoleRequest()
                {
                    ResourceId = hospiceId,
                    ResourceType = ResourceTypes.Hospice.ToString(),
                    RoleId = roleId
                });

                foreach (var location in hospiceLocations)
                {
                    userRoles.Add(new HospiceMemberRoleRequest()
                    {
                        ResourceId = location.Id,
                        ResourceType = ResourceTypes.HospiceLocation.ToString(),
                        RoleId = roleId
                    });
                }

                member.UserRoles = userRoles;
                member.HospiceLocationIds = hospiceLocations.Select(l => l.Id);
            }
            var hospiceMemberList = _mapper.Map<IEnumerable<HospiceMemberRequest>>(hospiceMemberCsvList);

            var hospiceMembersResponse = await BulkUploadMembers(hospiceId, hospiceMemberList);

            return (hospiceMembersResponse, null);
        }

        private async Task<IEnumerable<UserRoleBase>> GenerateHospiceMemberRoleList(int hospiceId, IEnumerable<int> hospiceLocationIds, IEnumerable<HospiceMemberRoleRequest> roles)
        {
            var standardHospiceRole = await _rolesRepository.GetByIdAsync((int)Enums.Roles.StandardHospiceUser);
            var userRoles = new List<UserRoleBase>();
            foreach (var role in roles)
            {
                if (!Enum.TryParse(role.ResourceType, true, out Data.Enums.ResourceTypes resourceType)
                    || (resourceType != ResourceTypes.Hospice && resourceType != ResourceTypes.HospiceLocation))
                {
                    throw new SystemValidation.ValidationException($"resource type ({role.ResourceType}) is not valid. Resource type should be either hospice or hospiceLocation");
                }

                if (resourceType == ResourceTypes.Hospice && role.ResourceId != hospiceId)
                {
                    throw new SystemValidation.ValidationException($"can not provide resource access to hospice with Id({role.ResourceId})");
                }

                else if (resourceType == ResourceTypes.HospiceLocation && !hospiceLocationIds.Contains(role.ResourceId))
                {
                    throw new SystemValidation.ValidationException($"can not provide resource access to hospice location with Id({role.ResourceId})");
                }

                if (role.RoleId != null)
                {
                    var roleModel = await _rolesRepository.GetByIdAsync(role.RoleId ?? 0);

                    if (roleModel == null)
                    {
                        throw new SystemValidation.ValidationException($"RoleIds ({role.RoleId}) not valid");
                    }
                    if (roleModel != null && roleModel.RoleType != RoleTypes.Hospice.ToString())
                    {
                        throw new SystemValidation.ValidationException($"RoleIds ({role.RoleId}) not related to hospice");
                    }
                }
                userRoles.Add(new UserRoleBase()
                {
                    ResourceType = resourceType.ToString(),
                    ResourceId = role.ResourceId.ToString(),
                    RoleId = role.RoleId ?? standardHospiceRole.Id
                });
            }
            return userRoles;
        }

        private async Task<IEnumerable<HospiceLocationMembers>> GenerateHospiceLocationMemberList(CoreModel.HospiceMember hospiceMember, HospiceMemberRequest hospiceMemberRequest)
        {
            var hospiceLocationMembers = new List<HospiceLocationMembers>();
            var hospiceLocationIds = hospiceMemberRequest.UserRoles?.Where(u => u.ResourceType.ToLower() == ResourceTypes.HospiceLocation.ToString().ToLower())
                                                                    .Select(u => u.ResourceId);
            if (hospiceMember != null)
            {
                IEnumerable<HospiceLocationMembers> deleteContacts = new List<HospiceLocationMembers>();
                if (hospiceLocationIds == null || hospiceLocationIds.Count() == 0)
                {
                    deleteContacts = hospiceMember.HospiceLocationMembers.Where(i => i.NetSuiteContactId != null);
                }
                else
                {
                    deleteContacts = hospiceMember.HospiceLocationMembers.Where(i => i.HospiceLocationId.HasValue
                                                                                && !hospiceLocationIds.Contains(i.HospiceLocationId.Value)
                                                                                && i.NetSuiteContactId != null);
                }

                foreach (var contact in deleteContacts)
                {
                    await _netSuiteService.DeleteCustomerContact(contact.NetSuiteContactId ?? 0);
                }
            }
            if (hospiceLocationIds == null || hospiceLocationIds.Count() == 0)
            {
                return null;
            }
            var hospiceLocations = await _hospiceLocationRepository.GetManyAsync(l => hospiceLocationIds.Contains(l.Id));
            foreach (var locationId in hospiceLocationIds)
            {
                bool canApproveOrder = false;
                int? netSuiteContactId = null;
                var location = hospiceLocations.FirstOrDefault(l => l.Id == locationId);
                if (hospiceMember != null)
                {
                    var existingContact = hospiceMember.HospiceLocationMembers.FirstOrDefault(l => l.HospiceLocationId == location.Id);
                    netSuiteContactId = existingContact?.NetSuiteContactId;
                    canApproveOrder = existingContact?.CanApproveOrder ?? false;
                }

                NSSDKViewModel.ContactResponse customerContact = null;
                if (netSuiteContactId == null && location.NetSuiteCustomerId.HasValue)
                {
                    var customerContactRequest = _mapper.Map<NSSDKViewModel.CustomerContactBase>(hospiceMemberRequest);
                    customerContactRequest.NetSuiteCustomerId = location.NetSuiteCustomerId.Value;
                    customerContact = await _netSuiteService.CreateCustomerContact(customerContactRequest);
                }
                else if (location.NetSuiteCustomerId.HasValue)
                {
                    var customerContactUpdateRequest = _mapper.Map<NSSDKViewModel.CustomerContact>(hospiceMemberRequest);
                    customerContactUpdateRequest.NetSuiteCustomerId = location.NetSuiteCustomerId.Value;
                    customerContactUpdateRequest.NetSuiteContactId = netSuiteContactId.Value;
                    customerContact = await _netSuiteService.UpdateCustomerContact(customerContactUpdateRequest);
                }

                hospiceLocationMembers.Add(new HospiceLocationMembers
                {
                    HospiceLocationId = location.Id,
                    NetSuiteContactId = customerContact?.NetSuiteContactId,
                    HospiceMemberId = hospiceMember?.Id,
                    CanApproveOrder = canApproveOrder
                });
            }

            return hospiceLocationMembers;
        }

        private async Task<int?> CreateNetSuiteContact(int? netSuiteCustomerId, HospiceMemberRequest hospiceMemberRequest, bool isAdmin, int memberId)
        {
            if (!netSuiteCustomerId.HasValue)
            {
                return null;
            }
            var customerContactRequest = _mapper.Map<NSSDKViewModel.CustomerContactBase>(hospiceMemberRequest);
            customerContactRequest.NetSuiteCustomerId = netSuiteCustomerId.Value;
            customerContactRequest.IsAdmin = isAdmin;

            var customerContact = await _netSuiteService.CreateCustomerContact(customerContactRequest);
            var netSuiteContactId = customerContact?.NetSuiteContactId;

            if (netSuiteContactId != null)
            {
                var dbContext = await _dbContextFactoryService.GetDBContext();
                var hospiceMember = await dbContext.HospiceMember.FindAsync(memberId);
                hospiceMember.NetSuiteContactId = netSuiteContactId;
                await dbContext.SaveChangesAsync();
            }

            return netSuiteContactId;
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
