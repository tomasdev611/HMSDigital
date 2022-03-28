using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services
{
    public class VerifyService : IVerifyService
    {
        private readonly IUserVerifyRepository _userVerifyRepository;

        private readonly IMapper _mapper;

        private readonly ILogger<VerifyService> _logger;

        public VerifyService(IMapper mapper,
            IUserVerifyRepository userVerifyRepository,
            ILogger<VerifyService> logger)
        {
            _mapper = mapper;
            _userVerifyRepository = userVerifyRepository;
            _logger = logger;
        }

        public async Task<string> CreateContactEmailOtpAsync(int contactId, string email)
        {
            return await CreateOtpAsync(contactId, email, 0);
        }

        public async Task<string> CreateContactPhoneNumberOtpAsync(int contactId, long phoneNumber)
        {
            return await CreateOtpAsync(contactId, null, phoneNumber);
        }

        public async Task<string> CreateEmailOtpAsync(string email)
        {
            return await CreateOtpAsync(0, email, 0);
        }

        public async Task<string> CreatePhoneNumberOtpAsync(long phoneNumber)
        {
            return await CreateOtpAsync(0, null, phoneNumber);
        }

        public async Task<bool> ValidateContactEmailOtpAsync(int contactId, string email, string nonce)
        {
            return await ValidateOtpAsync(nonce, contactId, email, 0);
        }

        public async Task<bool> ValidateContactPhoneNumberOtpAsync(int contactId, long phoneNumber, string nonce)
        {
            return await ValidateOtpAsync(nonce, contactId, null, phoneNumber);
        }

        public async Task<bool> ValidateEmailOtpAsync(string email, string nonce)
        {
            return await ValidateOtpAsync(nonce, 0, email, 0);
        }

        public async Task<bool> ValidatePhoneNumberOtpAsync(long phoneNumber, string nonce)
        {
            return await ValidateOtpAsync(nonce, 0, null, phoneNumber);
        }

        private async Task<string> CreateOtpAsync(int contactId, string email, long phoneNumber)
        {
            var userVerify = await _userVerifyRepository.GetAsync(pl => pl.ContactId == contactId
                                                                   && pl.Email == email 
                                                                   && pl.PhoneNumber == phoneNumber);
            var nonce = new Random().Next(100000, 1000000).ToString("D6");


            if (userVerify == null)
            {
                userVerify = new UserVerify
                {
                    ContactId = contactId,
                    Nonce = nonce,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    CreatedDateTime = DateTime.UtcNow,
                    ExpiryDateTime = DateTime.UtcNow.AddDays(3)
                };

                await _userVerifyRepository.AddAsync(userVerify);
            }
            else
            {
                userVerify.Nonce = nonce;
                userVerify.CreatedDateTime = DateTime.UtcNow;
                userVerify.ExpiryDateTime = DateTime.UtcNow.AddDays(3);

                await _userVerifyRepository.UpdateAsync(userVerify);
            }

            return userVerify.Nonce;
        }

        private async Task<bool> ValidateOtpAsync(string nonce, int contactId, string email, long phoneNumber)
        {
            try
            {
                var userVerify = await _userVerifyRepository.GetAsync(pl => pl.ContactId == contactId
                                                                       && pl.Email == email
                                                                       && pl.PhoneNumber == phoneNumber);

                if (userVerify == null)
                {
                    throw new ValidationException($"Confirmation code not requested.");
                }

                if (userVerify.ExpiryDateTime < DateTime.UtcNow)
                {
                    throw new ValidationException($"Confirmation code expired.");
                }

                if (string.Equals(userVerify.Nonce, nonce, StringComparison.OrdinalIgnoreCase))
                {
                    await _userVerifyRepository.DeleteAsync(userVerify);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while validate otp with contactId ({contactId}): {ex.Message}");
                throw;
            }
        }
    }
}
