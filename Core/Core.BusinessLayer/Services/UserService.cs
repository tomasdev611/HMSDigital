using AutoMapper;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Core.BusinessLayer.Validations;
using System.ComponentModel.DataAnnotations;
using HMSDigital.Core.Data.Repositories.Interfaces;
using CoreDataModel = HMSDigital.Core.Data.Models;
using CoreViewModels = HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Constants;
using NotificationSDK.Interfaces;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Enums;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityService _identityService;

        private readonly IUsersRepository _usersRepository;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;

        private readonly ILogger<UserService> _logger;

        private readonly IVerifyService _userVerifyService;

        private readonly IAuditService _auditService;

        private readonly INotificationService _notificationService;

        private readonly IPaginationService _paginationService;

        private readonly IFileStorageService _fileStorageService;

        private readonly IUserProfilePictureRepository _userProfilePictureRepository;

        public UserService(IMapper mapper,
             IIdentityService identityService,
             IUsersRepository usersRepository,
             IHttpContextAccessor httpContextAccessor,
             IVerifyService userVerifyService,
             IAuditService auditService,
             INotificationService notificationService,
             IPaginationService paginationService,
             IFileStorageService fileStorageService,
             IUserProfilePictureRepository userProfilePictureRepository,
             ILogger<UserService> logger)
        {
            _mapper = mapper;
            _identityService = identityService;
            _usersRepository = usersRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
            _userVerifyService = userVerifyService;
            _auditService = auditService;
            _notificationService = notificationService;
            _fileStorageService = fileStorageService;
            _userProfilePictureRepository = userProfilePictureRepository;
            _paginationService = paginationService;
        }


        public async Task<User> GetMyUser()
        {
            var userModel = await GetLoggedInUser();
            var user = _mapper.Map<ViewModels.User>(userModel);
            var profilePicture = await GetProfilePicture(user.Id);
            if(profilePicture != null)
            {
                user.ProfilePictureUrl = profilePicture.DownloadUrl;
            }
            return user;
        }
        
        public async Task<User> UpdateMyUser(UserMinimal userMinimalRequest)
        {
            var userModel = await GetLoggedInUser();
            return await UpdateUser(userModel.Id, userMinimalRequest);
        }

        public async Task<UserProfilePicture> UpdateProfilePicture(int userId, UserPictureFileRequest userProfilePictureRequest)
        {
            var profilePicValidator = new UserProfilePictureValidator();
            var validationResults = profilePicValidator.Validate(userProfilePictureRequest);
            if(!validationResults.IsValid)
            {
                throw new ValidationException(validationResults.Errors[0].ErrorMessage);
            }

            // Get User profile picture from database
            var userProfilePictureModel = await GetProfilePictureForUserId(userId, false);

            if(userProfilePictureModel == null)
            {
                userProfilePictureModel = await CreateUserProfilePictureModel(userProfilePictureRequest, userId);
            }
            else
            {
                if(userProfilePictureModel.IsUploaded)
                {
                    await _fileStorageService.DeleteFile(userProfilePictureModel.FileMetadata.StorageRoot, userProfilePictureModel.FileMetadata.StorageFilePath);
                }
                userProfilePictureModel.FileMetadata.StorageRoot = _fileStorageService.GetStorageRoot(userProfilePictureRequest);
                userProfilePictureModel.FileMetadata.StorageFilePath = _fileStorageService.GetStorageFilePath(userProfilePictureRequest);
                userProfilePictureModel.IsUploaded = false;
                userProfilePictureModel.DownloadUrl = null;
                userProfilePictureModel.CacheExpiryDateTime = null;
                await _userProfilePictureRepository.UpdateAsync(userProfilePictureModel);
            }

            var userProfilePictureFile = _mapper.Map<UserProfilePicture>(userProfilePictureModel);
            userProfilePictureFile.UploadUrl = await _fileStorageService.GetUploadUrl(userProfilePictureRequest, userProfilePictureModel.FileMetadata.StorageFilePath);
            return userProfilePictureFile;
        }

        public async Task<UserProfilePicture> GetProfilePicture(int userId)
        {
            var userProfilePictureModel = await GetProfilePictureForUserId(userId);
            if(userProfilePictureModel == null)
            {
                return null;
            }
            var userProfilePicture = _mapper.Map<UserProfilePicture>(userProfilePictureModel);
            if (userProfilePictureModel.CacheExpiryDateTime == null || userProfilePictureModel.CacheExpiryDateTime < DateTime.UtcNow)
            {
                var minsInaDay = 60 * 24;
                userProfilePictureModel.DownloadUrl = (await _fileStorageService.GetDownloadUrl(userProfilePicture.StorageRoot, userProfilePicture.StorageFilePath, 7 * minsInaDay))?.ToString();
                userProfilePictureModel.CacheExpiryDateTime = DateTime.UtcNow.AddDays(7);
                await _userProfilePictureRepository.UpdateAsync(userProfilePictureModel);
            }
            return _mapper.Map<UserProfilePicture>(userProfilePictureModel);
        }

        public async Task RemoveProfilePicture(int userId)
        {
            var userProfilePictureModel = await GetProfilePictureForUserId(userId);
            if (userProfilePictureModel != null)
            {
                if(userProfilePictureModel.IsUploaded)
                {
                    await _fileStorageService.DeleteFile(userProfilePictureModel.FileMetadata.StorageRoot, userProfilePictureModel.FileMetadata.StorageFilePath);
                }
                await _userProfilePictureRepository.DeleteAsync(userProfilePictureModel);
            }
        }

        public async Task<UserProfilePicture> ConfirmProfilePictureUpload(int userId)
        {
            var userProfilePictureModel = await GetProfilePictureForUserId(userId, false);
            userProfilePictureModel.IsUploaded = true;
            await _userProfilePictureRepository.UpdateAsync(userProfilePictureModel);
            return _mapper.Map<UserProfilePicture>(userProfilePictureModel);
        }

        private async Task<Data.Models.UserProfilePicture> GetProfilePictureForUserId(int userId, bool onlyUploaded = true)
        {
            if(onlyUploaded)
            {
                return await _userProfilePictureRepository.GetAsync(u => u.UserId == userId && u.IsUploaded);
            }
            return await _userProfilePictureRepository.GetAsync(u => u.UserId == userId);
        }

        private async Task<Data.Models.UserProfilePicture> CreateUserProfilePictureModel(UserPictureFileRequest userProfilePictureRequest, int userId)
        {
            var userProfilePictureModel = _mapper.Map<Data.Models.UserProfilePicture>(userProfilePictureRequest);
            userProfilePictureModel.UserId = userId;
            userProfilePictureModel.FileMetadata.StorageTypeId = _fileStorageService.GetStorageTypeId();
            userProfilePictureModel.FileMetadata.StorageRoot = _fileStorageService.GetStorageRoot(userProfilePictureRequest);
            userProfilePictureModel.FileMetadata.StorageFilePath = _fileStorageService.GetStorageFilePath(userProfilePictureRequest);
            userProfilePictureModel.IsUploaded = false;
            await _userProfilePictureRepository.AddAsync(userProfilePictureModel);
            return userProfilePictureModel;
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                var user = await _usersRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ValidationException($"user with userId({userId}) does not exist");
                }
                return _mapper.Map<User>(user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while getting user with UserId ({userId}): {ex.Message}");
                throw;
            }
        }

        public async Task<PaginatedList<User>> GetAllUsers(SieveModel sieveModel)
        {
            _usersRepository.SieveModel = sieveModel;
            var totalRecords = await _usersRepository.GetCountAsync(u => true);
            var userModels = await _usersRepository.GetAllAsync();
            var users = _mapper.Map<IEnumerable<User>>(userModels);
            return _paginationService.GetPaginatedList(users, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PaginatedList<User>> SearchUsers(string searchQuery, SieveModel sieveModel)
        {
            _usersRepository.SieveModel = sieveModel;
            var totalRecords = await _usersRepository.GetCountAsync(u => u.FirstName.Contains(searchQuery)
                                                                    || u.LastName.Contains(searchQuery) || u.Email.Contains(searchQuery));
            var userModels = await _usersRepository.GetManyAsync(u => u.FirstName.Contains(searchQuery)
                                                                    || u.LastName.Contains(searchQuery) || u.Email.Contains(searchQuery));
            var users = _mapper.Map<IEnumerable<User>>(userModels);
            return _paginationService.GetPaginatedList(users, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<User> CreateUser(UserMinimal userCreateRequest)
        {
            var userValidator = new UserValidator();
            var validationResult = userValidator.Validate(userCreateRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            User userResponse;
            try
            {
                userResponse = await _identityService.CreateUser(userCreateRequest);
                var userModel = _mapper.Map<CoreDataModel.Users>(userCreateRequest);

                userModel.CognitoUserId = userResponse.UserId;
                userModel.IsEmailVerified = userResponse.IsEmailVerified ?? false;
                userModel.IsPhoneNumberVerified = userResponse.IsPhoneNumberVerified ?? false;
                userModel.CreatedByUserId = GetLoggedInUserId();

                var existingUser = await _usersRepository.GetAsync(u => u.Email == userResponse.Email);
                if (existingUser != null)
                {
                    await _usersRepository.UpdateAsync(existingUser);
                    userResponse.Id = existingUser.Id;
                }
                else
                {
                    await _usersRepository.AddAsync(userModel);
                    userResponse.Id = userModel.Id;
                    _notificationService.SendUserCreatedNotification(userResponse.Email, userResponse.FirstName, userResponse.LastName);
                }
                userResponse.UserRoles = _mapper.Map<IEnumerable<CoreViewModels.UserRole>>(userModel.UserRoles);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Creating user with Email ({userCreateRequest.Email}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Creating user with Email ({userCreateRequest.Email}): {ex.Message}");
                throw ex;
            }
            return userResponse;
        }

        public async Task<IEnumerable<User>> CreateBulkUser(IEnumerable<UserCreateRequest> userCreateRequests)
        {
            var userModels = new List<CoreDataModel.Users>();
            foreach (var user in userCreateRequests)
            {
                var cognitoUserResponse = await _identityService.CreateUser(user);

                var userModel = _mapper.Map<CoreDataModel.Users>(user);
                userModel.CognitoUserId = cognitoUserResponse.UserId;
                userModels.Add(userModel);
            }
            var userEmails = userModels.Select(i => i.Email);
            var existingUsers = await _usersRepository.GetManyAsync(u => userEmails.Contains(u.Email));
            if (existingUsers != null && existingUsers.Count() > 0)
            {
                foreach (var existingUser in existingUsers)
                {
                    var userRoles = userModels.FirstOrDefault(u => u.Email == existingUser.Email).UserRoles;
                    if (userRoles != null && userRoles.Count() > 0)
                    {
                        foreach (var userRole in userRoles)
                        {
                            if (!existingUser.UserRoles.Any(ur => ur.ResourceType.Equals(userRole.ResourceType, StringComparison.OrdinalIgnoreCase)
                                                                    && ur.RoleId == userRole.RoleId
                                                                    && ur.ResourceId == userRole.ResourceId))
                            {
                                existingUser.UserRoles.Add(userRole);
                            }
                        }
                    }
                }
                await _usersRepository.UpdateManyAsync(existingUsers);
            }

            var newUserEmails = userEmails.Except(existingUsers.Select(i => i.Email));
            var UserRequests = userModels.Where(u => newUserEmails.Contains(u.Email));
            IEnumerable<CoreDataModel.Users> createdUsers = new List<CoreDataModel.Users>();
            if (UserRequests != null && UserRequests.Count() > 0)
            {
                createdUsers = await _usersRepository.AddManyAsync(UserRequests);
                foreach (var users in createdUsers)
                {
                    _notificationService.SendUserCreatedNotification(users.Email, users.FirstName, users.LastName);
                }
            }
            return _mapper.Map<IEnumerable<User>>(createdUsers.Concat(existingUsers));
        }

        public async Task<User> EnableUser(int userId)
        {
            try
            {
                var userModel = await _usersRepository.GetByIdAsync(userId);
                if (userModel == null)
                {
                    throw new ValidationException($"User with id ({userId}) not found.");
                }
                var loggedInUser = await GetLoggedInUser();
                if (userModel.DisabledByUserId != null && loggedInUser.Id != userModel.DisabledByUserId)
                {
                    var disabledByUser = await _usersRepository.GetByIdAsync(userModel.DisabledByUserId ?? 0);
                    var disabledByUserTopRoleLevel = disabledByUser.UserRoles.Select(r => r.Role.Level).Min();
                    var loggedInUserTopRoleLevel = loggedInUser.UserRoles.Select(r => r.Role.Level).Min();
                    if (!loggedInUser.UserRoles.Any(r => r.RoleId == (int)Roles.MasterAdmin) && loggedInUserTopRoleLevel >= disabledByUserTopRoleLevel)
                    {
                        throw new ValidationException("Higher Level Access is required");
                    }
                }
                var user = await _identityService.EnableUser(userModel.CognitoUserId);

                userModel.IsDisabled = false;
                await _usersRepository.UpdateAsync(userModel);

                return user;

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while enabling user with UserId ({userId}): {ex.Message}");
                throw ex;
            }
        }

        public async Task<User> DisableUser(int userId)
        {
            try
            {
                var userModel = await _usersRepository.GetByIdAsync(userId);
                var user = await _identityService.DisableUser(userModel.CognitoUserId);

                var isUserDisabled = userModel.IsDisabled ?? false;
                if (!isUserDisabled)
                {
                    userModel.IsDisabled = true;
                    userModel.DisabledByUserId = GetLoggedInUserId();
                    await _usersRepository.UpdateAsync(userModel);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while disabling user with UserId ({userId}): {ex.Message}");
                throw ex;
            }
        }

        public async Task<User> UpdateUser(int userId, UserMinimal usersUpdateRequest)
        {
            var userValidator = new UserValidator();
            var validationResult = userValidator.Validate(usersUpdateRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            try
            {
                var userModel = await _usersRepository.GetByIdAsync(userId);
                if (userModel == null)
                {
                    throw new ValidationException($"User with id ({userId}) not found.");
                }
                var user = await _identityService.UpdateUser(userModel.CognitoUserId, usersUpdateRequest);

                userModel.FirstName = user.FirstName;
                userModel.LastName = user.LastName;
                userModel.Email = user.Email;
                userModel.CountryCode = user.CountryCode;
                userModel.PhoneNumber = user.PhoneNumber;
                userModel.IsEmailVerified = user.IsEmailVerified ?? false;
                userModel.IsPhoneNumberVerified = user.IsPhoneNumberVerified ?? false;
                await _usersRepository.UpdateAsync(userModel);

                user.Id = userModel.Id;
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while updating driver for user with UserId ({userId}): {ex.Message}");
                throw ex;
            }

        }

        public async Task<IEnumerable<UserRole>> GetUserRoles(int userId)
        {
            try
            {
                var user = await _usersRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new List<UserRole>();
                }
                return _mapper.Map<IEnumerable<UserRole>>(user.UserRoles);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Getting roles for user with Email ({userId}): {ex.Message}");
                throw ex;
            }

        }

        public async Task SetUserPassword(int userId, ViewModels.UserPasswordRequest userPasswordRequest)
        {
            try
            {
                var user = await _usersRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ValidationException($"User with id ({userId}) not found.");
                }
                await _identityService.SetUserPassword(user.CognitoUserId, userPasswordRequest);
                await AddUsersAudit(AuditActions.SET_USER_PASSWORD, userId);
                if (userPasswordRequest.Channels != null && userPasswordRequest.Channels.Count() != 0)
                {
                    if (userPasswordRequest.CountryCode == 0)
                    {
                        userPasswordRequest.CountryCode = 1;
                    }
                    var phoneNumber = $"+{userPasswordRequest.CountryCode}{userPasswordRequest.PhoneNumber}";
                    await _notificationService.SendSetPasswordNotification(
                                                                           userPasswordRequest.Email,
                                                                           phoneNumber,
                                                                           userPasswordRequest.Password,
                                                                           userPasswordRequest.Channels
                                                                           );
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while set user password with UserId ({userId}): {ex.Message}");
                throw ex;
            }
        }

        public async Task ChangeUserPassword(DTOs.ChangePasswordRequest userPasswordRequest)
        {
            try
            {
                await _identityService.ChangeUserPassword(userPasswordRequest);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while set user password: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<Site>> GetUserSites(int userId)
        {
            try
            {
                var user = await _usersRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ValidationException($"user with userId({userId}) does not exist");
                }
                var userSites = user.SiteMembersUser.Select(us => us.Site);
                return _mapper.Map<IEnumerable<Site>>(userSites);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while getting user site for user with UserId ({userId}): {ex.Message}");
                throw;
            }

        }

        public async Task SendPasswordResetLink(int userId, NotificationBase resetPasswordNotification)
        {
            try
            {
                var resetLink = await _identityService.GetPasswordResetLink();
                await AddUsersAudit(AuditActions.PASSWORD_RESET_LINK, userId);
                if (resetPasswordNotification.Channels != null && resetPasswordNotification.Channels.Count() != 0)
                {
                    if (resetPasswordNotification.CountryCode == 0)
                    {
                        resetPasswordNotification.CountryCode = 1;
                    }
                    var phoneNumber = $"+{resetPasswordNotification.CountryCode}{resetPasswordNotification.PhoneNumber}";
                    await _notificationService.SendPasswordResetLinkNotification(
                                                                           resetPasswordNotification.Email,
                                                                           phoneNumber,
                                                                           resetLink,
                                                                           resetPasswordNotification.Channels
                                                                           );
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while getting user with UserId : {ex.Message}");
                throw;
            }
        }

        public async Task SendConfirmationCode(int userId, string email, long phoneNumber, int countryCode, string verifyAttribute)
        {
            string nonce;
            var channels = new List<string>();
            try
            {
                var userModel = await _usersRepository.GetByIdAsync(userId);
                if (userModel == null)
                {
                    throw new ValidationException($"User with id ({userId}) not found.");
                }

                await ValidateOtpVerifyCount(userModel);

                if (verifyAttribute.Equals(VerifyAttributes.PHONE_NUMBER_VERIFY))
                {
                    nonce = await _userVerifyService.CreatePhoneNumberOtpAsync(phoneNumber);
                    channels.Add(Channels.SMS);
                }
                else
                {
                    nonce = await _userVerifyService.CreateEmailOtpAsync(email);
                    channels.Add(Channels.EMAIL);
                }

                if (countryCode == 0)
                {
                    countryCode = 1;
                }
                await _notificationService.SendConfirmationCode(
                                                               email,
                                                               $"+{countryCode}{phoneNumber}",
                                                               nonce,
                                                               channels
                                                               );
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while getting confirmation code UserId : {ex.Message}");
                throw;
            }
        }

        public async Task<User> VerifyCode(int userId, string email, long phoneNumber, VerifyCodeRequest verifyCodeRequest)
        {
            User user = null;
            try
            {
                bool result;
                string auditAction;
                var userModel = await _usersRepository.GetByIdAsync(userId);
                if (userModel == null)
                {
                    throw new ValidationException($"User with id ({userId}) not found.");
                }
                if (userModel.LockoutEnd > DateTime.UtcNow)
                {
                    throw new ValidationException($"Maximum attempts reached. Please try again tomorrow");
                }
                if (verifyCodeRequest.VerifyAttribute.Equals(VerifyAttributes.PHONE_NUMBER_VERIFY))
                {
                    result = await _userVerifyService.ValidatePhoneNumberOtpAsync(phoneNumber, verifyCodeRequest.Code);
                    auditAction = AuditActions.PHONE_NUMBER_VERIFIED;
                }
                else
                {
                    result = await _userVerifyService.ValidateEmailOtpAsync(email, verifyCodeRequest.Code);
                    auditAction = AuditActions.EMAIL_VERIFIED;
                }
                if (result)
                {
                    user = await _identityService.VerifyUserAttribute(userModel.CognitoUserId, verifyCodeRequest.VerifyAttribute);
                    userModel.IsEmailVerified = user.IsEmailVerified ?? false;
                    userModel.IsPhoneNumberVerified = user.IsPhoneNumberVerified ?? false;
                    await _usersRepository.UpdateAsync(userModel);
                }
                await AddUsersAudit(auditAction, userId);
                await LockOutUser(result, userModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while getting confirmation code UserId : {ex.Message}");
                throw;
            }

            return user;
        }

        public async Task<SignInResponse> SignIn(LoginRequest loginRequest)
        {
            var loginValidator = new LoginValidator();
            var validationResult = loginValidator.Validate(loginRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }
            try
            {
                if (loginRequest.GrantType == GrantTypes.PASSWORD)
                {
                    return await _identityService.SignIn(loginRequest.Username, loginRequest.Password);
                }
                if (loginRequest.GrantType == GrantTypes.REFRESH_TOKEN)
                {
                    return await _identityService.RefreshToken(loginRequest.RefreshToken);
                }
                throw new ValidationException("Invalid Grant type.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while sign in user : {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UserRole>> GetLoggedInUserRoles()
        {
            return await GetUserRoles(GetLoggedInUserId());
        }

        public async Task SendOtp(string email)
        {
            try
            {
                var user = await _usersRepository.GetAsync(u => u.Email.ToLower() == email.ToLower());
                if(user == null)
                {
                    throw new ValidationException($"Email ({email}) not found in system.");
                }
                await ValidateOtpVerifyCount(user);
                string nonce = await _userVerifyService.CreateEmailOtpAsync(email);

                await _notificationService.SendPasswordResetOtp(email, nonce);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while sending opt on request email : {ex.Message}");
                throw;
            }
        }

        public async Task ResetUserPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                var user = await _usersRepository.GetAsync(u => u.Email.ToLower() == resetPasswordRequest.Email.ToLower());
                if (user == null)
                {
                    throw new ValidationException($"User with email ({resetPasswordRequest.Email}) not found.");
                }
                if (user.LockoutEnd > DateTime.UtcNow)
                {
                    throw new ValidationException($"Maximum attempts reached. Please try again tomorrow");
                }
                var isValidOtp = await _userVerifyService.ValidateEmailOtpAsync(resetPasswordRequest.Email, resetPasswordRequest.Otp);
                if (isValidOtp)
                {
                    var userPasswordRequest = new UserPasswordRequest()
                    {
                        Password = resetPasswordRequest.Password,
                        Permanent = true
                    };
                    await _identityService.SetUserPassword(user.CognitoUserId, userPasswordRequest);
                }
                await LockOutUser(isValidOtp, user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while reset user password : {ex.Message}");
                throw ex;
            }
        }

        private async Task<CoreDataModel.Users> GetLoggedInUser()
        {
            var userId = GetLoggedInUserId();
            if (userId == -1)
            {
                throw new UnauthorizedAccessException();
            }
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }
            return user;
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

        private async Task AddUsersAudit(string auditAction, int targetUserId)
        {
            var userId = GetLoggedInUserId();

            if (userId == -1)
            {
                throw new UnauthorizedAccessException();
            }

            var targetUser = await _usersRepository.GetByIdAsync(targetUserId);

            var userAudit = new CoreViewModels.AuditLog()
            {
                UserId = userId,
                AuditAction = auditAction,
                EntityId = targetUser.Id,
                AuditDate = DateTime.UtcNow,
                ClientIpaddress = _httpContext.Connection?.RemoteIpAddress.ToString()
            };
            await _auditService.AddUsersAudit(userAudit);
        }

        private async Task ValidateOtpVerifyCount(CoreDataModel.Users user)
        {
            if (user.LockoutEnd > DateTime.UtcNow)
            {
                throw new ValidationException($"Maximum attempts reached. Please try again tomorrow");
            }
            else if (user.OtpVerifyFailCount >= UserConstants.USER_MAX_OTP_VERIFY_FAIL_COUNT)
            {
                user.OtpVerifyFailCount = 0;
                await _usersRepository.UpdateAsync(user);
            }
        }

        private async Task LockOutUser(bool isOtpValid, CoreDataModel.Users user)
        {
            if (!isOtpValid)
            {
                user.OtpVerifyFailCount++;
                if (user.OtpVerifyFailCount >= UserConstants.USER_MAX_OTP_VERIFY_FAIL_COUNT)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddDays(1);
                }
                await _usersRepository.UpdateAsync(user);

                throw new ValidationException($"Invalid OTP.");
            }
        }
    }
}
