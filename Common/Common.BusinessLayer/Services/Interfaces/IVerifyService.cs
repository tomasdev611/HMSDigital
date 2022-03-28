using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services.Interfaces
{
    public interface IVerifyService
    {
        Task<string> CreateEmailOtpAsync(string email);

        Task<string> CreateContactEmailOtpAsync(int contactId, string email);

        Task<string> CreatePhoneNumberOtpAsync(long phoneNumber);

        Task<string> CreateContactPhoneNumberOtpAsync(int contactId, long phoneNumber);

        Task<bool> ValidateEmailOtpAsync(string email, string nonce);

        Task<bool> ValidateContactEmailOtpAsync(int contactId, string email, string nonce);

        Task<bool> ValidatePhoneNumberOtpAsync(long phoneNumber, string nonce);

        Task<bool> ValidateContactPhoneNumberOtpAsync(int contactId, long phoneNumber, string nonce);
    }
}
