using MobileApp.Models;
using Refit;
using System.Threading.Tasks;

namespace MobileApp.Interface
{
    public interface IDriverApi
    {
        [Get("/api/drivers/me")]
        Task<ApiResponse<Driver>> GetDriverDetailsAsync();

        [Get("/api/dispatch/me")]
        Task<ApiResponse<DispatchResponse>> GetDispatchInstructionsAsync();

        [Post("/api/drivers/me/location")]
        Task<ApiResponse<string>> SendDriverLocationAsync([Body] GeoLocation location);
    }
}
