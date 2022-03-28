namespace HMSDigital.EHRIntegration.FHIRServices.Config
{
    public class AzureServiceBusConfig
    {
        public string ConnectionString { get; set ; }
        public string InputQueueName { get; set; }
        public string OutputQueueName { get; set; }
    }
}
