using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.EHRIntegration.FHIRServices.Config;
using HMSDigital.EHRIntegration.FHIRServices.DTOs.Requests;
using HMSDigital.EHRIntegration.FHIRServices.DTOs.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ServiceBusTriggerAttribute = Microsoft.Azure.Functions.Worker.ServiceBusTriggerAttribute;
using FhirModel = Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Linq;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;

namespace HMSDigital.EHRIntegration.FHIRServices
{
    public class FHIRConverter
    {
        private readonly ITokenService _tokenService;
        private readonly FHIRConfig _fhirConfig;
        private readonly IFHIRQueueService<FhirModel.Patient> _fhirQueueService;

        private static readonly HttpClient _httpClient = new HttpClient();

        public FHIRConverter(IOptions<FHIRConfig> fhirConfigOptions, ITokenService tokenService, IFHIRQueueService<FhirModel.Patient> fhirQueueService)
        {
            _fhirConfig = fhirConfigOptions.Value;
            _tokenService = tokenService;           
            _fhirQueueService = fhirQueueService;
        }

        [Function("FHIRConverter")]          
        public async Task Run([ServiceBusTrigger("mllpqueue", Connection = "AzureServiceBusConnection")]string inputQueueItem, ILogger logger)
        {
            var eventId = new EventId();

            logger.LogInformation(eventId.Id, "Received Message from HC/HB", inputQueueItem);

            var hl7ADTMessage = JsonConvert.DeserializeObject<HL7ADTDTO>(inputQueueItem);

            var hl7V2Message = CreateHL7V2DTO(hl7ADTMessage);

            #region HL7-ADT to FHIR Convertion

            var authToken = await _tokenService.GetAccessTokenByClientCredentials(_fhirConfig.IdentityClient);                        

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.PostAsJsonAsync<HL7V2DTO>(_fhirConfig.ApiUrl + "$convert-data", hl7V2Message);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content?.ReadAsStringAsync();

                logger.LogInformation(eventId, "Message from HC/HB Converted", hl7V2Message);

                var fhirJsonParser = new FhirJsonParser();

                var fhirBundle = fhirJsonParser.Parse<FhirModel.Bundle>(responseContent);

                var patientModel = fhirBundle?.Entry?.Select(e => e.Resource as FhirModel.Patient).Where(x => x != null).FirstOrDefault();

                if (patientModel != null)
                    await _fhirQueueService.QueueCreateRequest(patientModel);
            }
            else
            {
                var responseContent = await response.Content?.ReadAsStringAsync();

                logger.LogError(eventId, "Message from HC/HB not converted", responseContent);
            }

            #endregion
        }

        private HL7V2DTO CreateHL7V2DTO(HL7ADTDTO hl7ADTMessage) 
        {
            var hl7V2message = new HL7V2DTO()
            {
                ResourceType = "Parameters",
                Parameter = new List<HL7V2ParameterDTO>()
                {
                    new HL7V2ParameterDTO
                    {
                        Name = "inputData",
                        ValueString = $"{hl7ADTMessage.Message}"
                    },
                    new HL7V2ParameterDTO
                    {
                        Name = "inputDataType",
                        ValueString = "Hl7v2"
                    },
                    new HL7V2ParameterDTO
                    {
                        Name = "templateCollectionReference",
                        ValueString = "microsofthealth/fhirconverter:default"
                    },
                    new HL7V2ParameterDTO
                    {
                        Name = "rootTemplate",
                        ValueString = "ADT_A01"
                    }
                }
            };

            return hl7V2message;
        }          
    }
}
