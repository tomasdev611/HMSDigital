using System;
using System.Linq;
using System.Threading.Tasks;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Service;

namespace MobileApp.Utils
{
    public class SiteDetailsUtil
    {
        public static async Task<SiteDetail> GetCurrentSiteDetailsAsync()
        {
            var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();
            try
            {
                var siteDetail = await CacheManager.GetSiteDetailByIdFromCache(currentSiteId ?? 0);

                var address = siteDetail.Address;

                return new SiteDetail()
                {
                    Address = CommonUtility.AddressToString(address),
                    SiteAdmin = siteDetail.Name,
                    ContactNumber = siteDetail.SitePhoneNumber.FirstOrDefault().PhoneNumber.Number.ToString(),
                    Latitude = Decimal.ToDouble(address.Latitude),
                    Longitude = Decimal.ToDouble(address.Longitude)
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
