using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.EHRIntegration.FHIRServices.Config;
using HMSDigital.EHRIntegration.FHIRServices.DTOs.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Patient.FHIR.BusinessLayer.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FhirModel = Hl7.Fhir.Model;

namespace HMSDigital.EHRIntegration.FHIRServices
{
    public class FHIRInserter
    {
        private readonly ITokenService _tokenService;
        private readonly FHIRConfig _fhirConfig;
        private readonly IFHIRQueueService<FhirModel.Patient> _fhirQueueService;

        private HttpClient _httpClient = new HttpClient();

        public FHIRInserter(IOptions<FHIRConfig> fhirConfigOptions, ITokenService tokenService, IFHIRQueueService<FhirModel.Patient> fhirQueueService)
        {
            _fhirConfig = fhirConfigOptions.Value;
            _tokenService = tokenService;           
        }

        [Function("FHIRInserter")]
        public async Task Run([Microsoft.Azure.Functions.Worker.ServiceBusTrigger("fhirqueue", Connection = "AzureServiceBusConnection")] string inputQueueItem, ILogger log)
        {
            var patientJson = JsonConvert.DeserializeObject<string>(inputQueueItem);

            #region FHIR Patient Insertion

            var authToken = await _tokenService.GetAccessTokenByClientCredentials(_fhirConfig.IdentityClient);                        

            StringContent content = new StringContent(patientJson, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(_fhirConfig + "/Patient"),
                Content = content,
                Method = HttpMethod.Post
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);            

            var response = await _httpClient.SendAsync(request, new CancellationTokenSource().Token);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content?.ReadAsStringAsync();

                Console.WriteLine(responseContent);
            }

            #endregion
        }
    }
}
