using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Core.BusinessLayer.Validations;
using AutoMapper;
using HMSDigital.Common.ViewModels;

namespace HMSDigital.Core.API.Controllers
{
    [Produces("application/json")]
    [Route("/api/users")]

    public class UsersController : CoreBaseController
    {
        private readonly IUserService _userService;

        private readonly IUserAccessService _userAccessService;

        private readonly IRolesService _rolesService;

        private readonly IHospiceMemberService _hospiceMemberService;

        private readonly IHospiceService _hospiceService;

        private readonly ILogger<UsersController> _logger;

        private readonly IMapper _mapper;

        public UsersController(IUserService userService,
            IMapper mapper,
            IUserAccessService userAccessService,
            IRolesService rolesService,
            IHospiceMemberService hospiceMemberService,
            IHospiceService hospiceService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _userAccessService = userAccessService;
            _rolesService = rolesService;
            _hospiceMemberService = hospiceMemberService;
            _hospiceService = hospiceService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<IActionResult> GetMyUser()
        {
            try
            {
                var user = await _userService.GetMyUser();
                return Ok(user);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting user:{ex.Message}");
                throw;
            }
        }

        [HttpPut]
        [Authorize]
        [Route("me")]
        public async Task<IActionResult> UpdateMyProfile(UserMinimal userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedUser = await _userService.UpdateMyUser(userRequest);
                return Ok(updatedUser);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating my user:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("{userId}/profile-pic")]
        public async Task<IActionResult> UpdateProfilePicture([FromRoute] int userId, UserPictureFileRequest userPictureFileRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_userAccessService.ValidateLoggedInUser(userId))
            {
                return Forbid();
            }
            try
            {
                var userProfilePicture = await _userService.UpdateProfilePicture(userId, userPictureFileRequest);
                return Ok(userProfilePicture);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while updating user profile picture:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("{userId}/profile-pic/confirm")]
        public async Task<IActionResult> ConfirmProfilePictureUpload([FromRoute] int userId)
        {
            if (!_userAccessService.ValidateLoggedInUser(userId))
            {
                return Forbid();
            }
            try
            {
                var userProfilePicture = await _userService.ConfirmProfilePictureUpload(userId);
                return Ok(userProfilePicture);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while confirming user profile picture upload:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}/profile-pic")]
        public async Task<IActionResult> GetProfilePicture([FromRoute] int userId)
        {
            if (!_userAccessService.ValidateLoggedInUser(userId))
            {
                return Forbid();
            }
            try
            {
                var userProfilePicture = await _userService.GetProfilePicture(userId);
                return Ok(userProfilePicture);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting user profile picture:{ex.Message}");
                throw;
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{userId}/profile-pic")]
        public async Task<IActionResult> RemoveProfilePicture([FromRoute] int userId)
        {
            if (!_userAccessService.ValidateLoggedInUser(userId))
            {
                return Forbid();
            }
            try
            {
                await _userService.RemoveProfilePicture(userId);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while removing user profile picture:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_USER)]
        [Route("")]
        public async Task<IActionResult> GetUsers([FromQuery] SieveModel sieveModel)
        {
            try
            {
                var users = await _userService.GetAllUsers(sieveModel);
                return Ok(users);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting users:{ex.Message}");
                throw;
            }
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_USER)]
        [Route("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchQuery, [FromQuery] SieveModel sieveModel)
        {
            try
            {
                var users = await _userService.SearchUsers(searchQuery, sieveModel);
                return Ok(users);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while searching users:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_CREATE_USER)]
        [Route("")]
        public async Task<IActionResult> CreateUser([FromBody] ViewModels.UserCreateRequest userCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ViewModels.User user;
            try
            {
                user = await _userService.CreateUser(userCreateRequest);
                if (userCreateRequest.UserRoles != null)
                {
                    foreach (var userRoleRequest in userCreateRequest.UserRoles)
                    {
                        var updatedUserRoles = await _rolesService.AddUserRole(user.Id, userRoleRequest);
                    }
                }

                await CreateHospiceMemberForInternalUser(user, userCreateRequest.UserRoles.Select(ur => ur.RoleId));
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while creating user:{ex.Message}");
                throw;
            }
            return CreatedAtAction("GetUserById", "Users", new { userId = user.Id }, user);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_USER_ACCESS)]
        [Route("{userId}/enable")]
        public async Task<IActionResult> EnableUser([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user;
            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }

                user = await _userService.EnableUser(userId);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while enabling user:{ex.Message}");
                throw;
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_USER_ACCESS)]
        [Route("{userId}/disable")]
        public async Task<IActionResult> DisableUser([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user;
            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }

                user = await _userService.DisableUser(userId);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while disabling user:{ex.Message}");
                throw;
            }

            return Ok(user);
        }

        [HttpPut]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_USER)]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int userId, [FromBody] ViewModels.UserMinimal usersRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ViewModels.User updatedUser;
            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }
                var user = await _userService.GetUserById(userId);
                updatedUser = await _userService.UpdateUser(userId, usersRequest);
                if(!string.Equals(user.Email, updatedUser.Email, StringComparison.OrdinalIgnoreCase))
                {
                    await _hospiceMemberService.UpdateHospiceMemberInNetSuite(updatedUser.Id);
                }
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while update user :{ex.Message}");
                throw;
            }
            return Ok(updatedUser);
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_USER)]
        [Route("{userId}/roles")]
        public async Task<IActionResult> GetUserRoles([FromRoute] int userId)
        {
            IEnumerable<UserRole> userRoles;
            try
            {
                userRoles = await _userService.GetUserRoles(userId);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Getting user roles :{ex.Message}");
                throw;
            }
            return Ok(userRoles);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_USER)]
        [Route("{userId}/roles")]
        public async Task<IActionResult> AddUserRoles([FromRoute] int userId, [FromBody] UserRoleBase userRoleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<UserRole> updatedUserRoles;
            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }
                var user = await _userService.GetUserById(userId);

                updatedUserRoles = await _rolesService.AddUserRole(userId, userRoleRequest);

                await CreateHospiceMemberForInternalUser(user, new List<int>() { userRoleRequest.RoleId });
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Adding user roles :{ex.Message}");
                throw;
            }
            return Ok(updatedUserRoles);
        }

        [HttpDelete]
        [Authorize(Policy = PolicyConstants.CAN_UPDATE_USER)]
        [Route("{userId}/roles/{userRoleId}")]
        public async Task<IActionResult> RemoveUserRoles([FromRoute] int userId, [FromRoute] int userRoleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<UserRole> updatedUserRoles;
            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }

                updatedUserRoles = await _rolesService.RemoveUserRole(userId, userRoleId);
                await RemoveHospiceMemberForInternalUser(userId, updatedUserRoles.Select(ur => ur.RoleId));
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Removing user roles :{ex.Message}");
                throw;
            }
            return Ok(updatedUserRoles);
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_USER_ACCESS)]
        [Route("{userId}/reset-password")]
        public async Task<IActionResult> SetUserPassword([FromRoute] int userId, [FromBody] ViewModels.UserPasswordRequest userPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }

                var invalidChannels = userPasswordRequest.Channels.Where(c => c != Channels.EMAIL && c != Channels.SMS);
                if (invalidChannels.Count() != 0)
                {
                    return BadRequest($"Invalid notification channels [{string.Join(",", invalidChannels)}].");
                }
                var userSetPasswordValidator = new UserSetPasswordValidator();
                var validationResult = userSetPasswordValidator.Validate(userPasswordRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors[0].ErrorMessage);
                }
                await _userService.SetUserPassword(userId, userPasswordRequest);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while set user password:{ex.Message}");
                throw;
            }
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("me/change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ViewModels.ChangePasswordRequest changePasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var changeuserPasswordValidator = new ChangeUserPasswordValidator();
                var validationResult = changeuserPasswordValidator.Validate(changePasswordRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors[0].ErrorMessage);
                }
                var changePassRequest = _mapper.Map<BusinessLayer.DTOs.ChangePasswordRequest>(changePasswordRequest);
                changePassRequest.AccessToken = GetAuthorizationToken();
                if(changePassRequest.AccessToken == null)
                {
                    return Forbid();
                }
                await _userService.ChangeUserPassword(changePassRequest);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while set user password:{ex.Message}");
                throw;
            }
            return Ok();
        }

        [HttpGet]
        [Authorize(Policy = PolicyConstants.CAN_READ_USER)]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);
                return Ok(user);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while getting user:{ex.Message}");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_USER_ACCESS)]
        [Route("{userId}/reset-password-link")]
        public async Task<IActionResult> SendUserPasswordResetLink([FromRoute] int userId, [FromBody] ViewModels.NotificationBase resetPasswordNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var invalidChannels = resetPasswordNotification.Channels.Where(c => c != Channels.EMAIL && c != Channels.SMS);
            if (invalidChannels.Count() != 0)
            {
                return BadRequest($"Invalid notification channels [{string.Join(",", invalidChannels)}].");
            }
            var userSetPasswordValidator = new UserSetPasswordValidator();
            var validationResult = userSetPasswordValidator.Validate(resetPasswordNotification);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors[0].ErrorMessage);
            }
            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }

                await _userService.SendPasswordResetLink(userId, resetPasswordNotification);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while send password reset link:{ex.Message}");
                throw;
            }
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_USER_ACCESS)]
        [Route("{userId}/send-confirmation-code")]
        public async Task<IActionResult> SendAttributeConfirmationCode([FromRoute] int userId, [FromBody] ConfirmationCodeRequest confirmationCodeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!confirmationCodeRequest.VerifyAttribute.Equals(VerifyAttributes.EMAIL_VERIFY) &&
                !confirmationCodeRequest.VerifyAttribute.Equals(VerifyAttributes.PHONE_NUMBER_VERIFY))
            {
                return BadRequest($"Invalid user verify attribute {confirmationCodeRequest.VerifyAttribute}.");
            }

            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }

                var user = await _userService.GetUserById(userId);

                if (confirmationCodeRequest.VerifyAttribute.Equals(VerifyAttributes.PHONE_NUMBER_VERIFY) && user.PhoneNumber == 0)
                {
                    return BadRequest($"Phone number need to present for verify.");
                }

                var channels = new List<string>();

                await _userService.SendConfirmationCode(userId, user.Email, user.PhoneNumber, user.CountryCode, confirmationCodeRequest.VerifyAttribute);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while send attribute verify code:{ex.Message}");
                throw;
            }
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = PolicyConstants.CAN_MANAGE_USER_ACCESS)]
        [Route("{userId}/verify-code")]
        public async Task<IActionResult> VerificationCode([FromRoute] int userId, [FromBody] VerifyCodeRequest verifyCodeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!verifyCodeRequest.VerifyAttribute.Equals(VerifyAttributes.EMAIL_VERIFY) &&
                !verifyCodeRequest.VerifyAttribute.Equals(VerifyAttributes.PHONE_NUMBER_VERIFY))
            {
                return BadRequest($"Invalid user verify attribute {verifyCodeRequest.VerifyAttribute}.");
            }

            try
            {
                var canAccessUser = await _userAccessService.CanAccessUser(userId);
                if (!canAccessUser)
                {
                    return Forbid();
                }

                var user = await _userService.GetUserById(userId);
                var verifyUser = await _userService.VerifyCode(userId, user.Email, user.PhoneNumber, verifyCodeRequest);

            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while send attribute verify code:{ex.Message}");
                throw;
            }
            return Ok();
        }


        [HttpPost]
        [Route("me/send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] OtpRequest otpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var optValidator = new OtpValidator();
            var validationResult = optValidator.Validate(otpRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors[0].ErrorMessage);
            }
            try
            {
                await _userService.SendOtp(otpRequest.Email);
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while send otp on requested email:{ex.Message}");
                throw;
            }
            return Ok();
        }

        [HttpPost]
        [Route("me/reset-password")]
        public async Task<IActionResult> ResetUserPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resetPasswordValidator = new ResetPasswordValidator();
                var validationResult = resetPasswordValidator.Validate(resetPasswordRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors[0].ErrorMessage);
                }
                await _userService.ResetUserPassword(resetPasswordRequest);
                return Ok();
            }
            catch (ValidationException vx)
            {
                return BadRequest(vx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while reset user password:{ex.Message}");
                throw;
            }
        }


        private async Task CreateHospiceMemberForInternalUser(User user, IEnumerable<int> roleIds)
        {
            var allRoles = await _rolesService.GetAllRoles(new SieveModel());
            var rolesForUser = allRoles.Where(r => roleIds.Contains(r.Id));
            foreach (var roleViewModel in rolesForUser)
            {
                if (IsRoleEligibleForInternalHospiceMember(roleViewModel))
                {
                    await _hospiceMemberService.CreateInternalHospiceMember(user);
                    break;
                }
            }
        }

        private async Task RemoveHospiceMemberForInternalUser(int userId, IEnumerable<int> roleIds)
        {
            var allRoles = await _rolesService.GetAllRoles(new SieveModel());
            var rolesForUser = allRoles.Where(r => roleIds.Contains(r.Id));
            foreach (var roleViewModel in rolesForUser)
            {
                if (IsRoleEligibleForInternalHospiceMember(roleViewModel))
                {
                    return;
                }
            }
            await _hospiceMemberService.DeleteInternalHospiceMember(userId);

        }

        private bool IsRoleEligibleForInternalHospiceMember(Role role)
        {
            return string.Equals(role.RoleType, BusinessLayer.Enums.RoleTypes.Internal.ToString(), StringComparison.OrdinalIgnoreCase) &&
                    role.Permissions.Any(p => string.Equals($"{PermissionNounConstants.PERMISSION_PREFIX}{p}", $"{PermissionNounConstants.ORDERS}:{PermissionVerbConstants.CREATE}", StringComparison.OrdinalIgnoreCase)
                                                    || string.Equals($"{PermissionNounConstants.PERMISSION_PREFIX}{p}", $"{PermissionNounConstants.ORDERS}:{PermissionVerbConstants.APPROVE}", StringComparison.OrdinalIgnoreCase));
        }
    }
}
