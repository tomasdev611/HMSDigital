using System.Threading.Tasks;
using MobileApp.Models;
using Refit;

namespace MobileApp.Interface
{
    public interface IAuthApi
    {
        [Post("/auth/token")]
        Task<ApiResponse<Token>> RequestLoginAsync([Body(BodySerializationMethod.UrlEncoded)] UserLogin loginDetails);
    }
}
