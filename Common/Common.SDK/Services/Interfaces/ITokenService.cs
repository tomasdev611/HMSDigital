using HMSDigital.Common.SDK.Config;
using System.Threading.Tasks;

namespace HMSDigital.Common.SDK.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenByClientCredentials(IdentityClientConfig identityClient);
    }
}
