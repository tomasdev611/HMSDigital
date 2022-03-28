
namespace HospiceSource.Digital.NetSuite.SDK.Config
{
    public class NetSuiteConfig
    {
        public RestletsConfig Restlets { get; set; }

        public int InternalUsersHostCustomerId { get; set; }

        public int PatientWarehouseId { get; set; }

        public int UrlListMaxLength { get; set; }

        public string ClientId { get; set; }
    }
}
