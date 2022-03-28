using System.Collections.Generic;
using System.Linq;
using MobileApp.Models;
using Newtonsoft.Json.Linq;

namespace MobileApp.Methods
{
    public static class CommonUtility
    {
        public static string AddressToString(Address address)
        {
            var addressFields = new List<string>
            {
                address.AddressLine1,
                address.AddressLine2,
                address.City,
                address.State,
            };

            var addressString = string.Join(",", addressFields.Where(af => !string.IsNullOrEmpty(af)));
            return $"{addressString} - {address.ZipCode}";
        }

        public static IDictionary<string, string> ConvertJArrayToDictionary(JArray eqipmentSettings)
        {
            return eqipmentSettings.ToDictionary(k => ((JObject)k).Properties().First().Name, v => v.Values().First().Value<string>());
        }

        public static bool CompareAddress(Address address1, Address address2)
        {
            if (address1 != null && address2 != null)
            {
                return address1.AddressUuid == address2.AddressUuid;
            }
            return false;
        }
    }
}
