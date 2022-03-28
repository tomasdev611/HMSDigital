namespace HMSDigital.Common.BusinessLayer
{
    public class AWSConfig
    {
        public string Region { get; set; }

        public string UserPoolId { get; set; }

        public string UserPoolClientId { get; set; }

        public string RedirectUri { get; set; }

        public string ResponseType { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }
    }
}
