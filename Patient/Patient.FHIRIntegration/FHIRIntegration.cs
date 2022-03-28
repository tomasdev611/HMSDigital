using CoreSDK.Interfaces;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HMSDigital.Patient.FHIR.Models;
using System;
using System.Threading.Tasks;
using HMSDigital.Common.BusinessLayer.Enums;

namespace HMSDigital.Patient.FhirIntegration
{
    public class FHIRIntegration
    {
        private readonly IFHIRService _IfhirService;
        private readonly IFHIRStorageService _fhirStorageService;
        private readonly IPatientService _patientService;
        private readonly ICoreService _coreService;

        public FHIRIntegration(
            IFHIRService IfhirService, 
            IPatientService patientService, 
            IFHIRStorageService fhirStorageService,
            ICoreService coreService)
        {
            _IfhirService = IfhirService;
            _patientService = patientService;
            _fhirStorageService = fhirStorageService;
            _coreService = coreService;
        }

        [Function("FHIRIntegration")]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task Run([ServiceBusTrigger("%FHIRQueueName%", Connection = "QueueConnectionString")] string queueItem, FunctionContext context)
        {
            var logger = context.GetLogger("FHIRIntegration");

            await _fhirStorageService.UploadJsonFile(queueItem);

            var request = JsonConvert.DeserializeObject<FHIRQueueRequestWrapper>(queueItem);

            if (request.ResourceType == typeof(FHIRPatientDetail).FullName)
            {
                await ProcessPatientRequest(request, logger);
            }

            if (request.ResourceType == typeof(FHIRHospice).FullName)
            {
                await ProcessOrganizationRequest(request, logger);
            }
        }

        private async Task ProcessPatientRequest(FHIRQueueRequestWrapper patientRequest, ILogger logger) 
        {
            var patientDetail = JsonConvert.DeserializeObject<FHIRPatientDetail>(Convert.ToString(patientRequest.Resource));
            if ((FHIRQueueRequestTypes)patientRequest.RequestType == FHIRQueueRequestTypes.Create)
            {
                var patient = await _IfhirService.CreatePatient(patientDetail);
                try
                {
                    await _patientService.UpdatePatientFhirId(new Guid(patientDetail.UniqueId), patient.FhirPatientId.Value);
                }
                catch
                {
                    logger.LogInformation("Exception occurred while updating FHIR id on HMS patient");
                    await _IfhirService.DeletePatient(patient.UniqueId);
                    throw;
                }
                logger.LogInformation("Patient inserted on FHIR server");
            }
            else
            {
                await _IfhirService.UpdatePatient(patientDetail.FhirPatientId.Value, patientDetail);
                logger.LogInformation("Patient updated on FHIR server");
            }
        }

        private async Task ProcessOrganizationRequest(FHIRQueueRequestWrapper organizationRequest, ILogger logger)
        {
            var hospice = JsonConvert.DeserializeObject<FHIRHospice>(Convert.ToString(organizationRequest.Resource));
            if ((FHIRQueueRequestTypes)organizationRequest.RequestType == FHIRQueueRequestTypes.Create)
            {
                var organization = await _IfhirService.CreateOrganization(hospice);
                try
                {
                    await _coreService.UpdateHospiceFhirOrganizationId(hospice.Id, new Guid(organization.Id));
                }
                catch
                {
                    logger.LogInformation("Exception occurred while updating FHIR id on HMS hospice");
                    //await _IfhirService.DeleteOrganization(organization.Id); TODO
                    throw;
                }
                logger.LogInformation("Hospice inserted on FHIR server");
            }
            else
            {
                await _IfhirService.UpdateOrganization(hospice);
                logger.LogInformation("Hospice updated on FHIR server");
            }
        }
    }
}
