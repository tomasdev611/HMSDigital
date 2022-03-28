using AutoMapper;
using Hl7.Fhir.Rest;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.SDK.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HMSDigital.Patient.FHIR.BusinessLayer.Constants;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HMSDigital.Patient.FHIR.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FhirModel = Hl7.Fhir.Model;
using PatientOrderRequest = HMSDigital.Patient.ViewModels.PatientOrderRequest;
using HMSDigital.Common.BusinessLayer.Config;

namespace HMSDigital.Patient.FHIR.BusinessLayer.Services
{
    public class FHIRService : IFHIRService
    {
        private readonly FHIRConfig _fhirConfig;

        private readonly IMapper _mapper;

        private readonly ILogger<FHIRService> _logger;

        private readonly IPaginationService _paginationService;

        private readonly ITokenService _tokenService;

        public FHIRService(
            IOptions<FHIRConfig> fhirConfigOptions,
            IMapper mapper,
            IPaginationService paginationService,
            ITokenService tokenService,
            ILogger<FHIRService> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _fhirConfig = fhirConfigOptions.Value;
            _paginationService = paginationService;
            _tokenService = tokenService;
        }

        public async Task<PaginatedList<FHIRPatientDetail>> GetAllPatients()
        {
            var client = await GetFHIRClient();
            var response = (await client.GetAsync("Patient")) as FhirModel.Bundle;
            var patientModels = response.Entry.Select(e => e.Resource as FhirModel.Patient);

            var patients = _mapper.Map<IEnumerable<FHIRPatientDetail>>(patientModels);
            return _paginationService.GetPaginatedList(patients, patients.Count(), null, null);
        }

        public async Task<FHIRPatientDetail> GetPatientById(string patientId)
        {
            var client = await GetFHIRClient();
            var patientModel = await ReadFhirPatient(client, patientId);
            return _mapper.Map<FHIRPatientDetail>(patientModel);
        }

        public async Task<IEnumerable<FHIRPatientDetail>> SearchPatientsByBirthDate(DateTime patientDateOfBirth)
        {
            var searchCriterias = new List<string>();
            searchCriterias.Add($"birthdate={patientDateOfBirth:yyyy-MM-dd}");
            return await GetPatientsByCriterias(searchCriterias);
        }

        public async Task<IEnumerable<FHIRPatientDetail>> SearchPatientsByYearOfBirth(int patientYearOfBirth)
        {
            var searchCriterias = new List<string>();
            var birthDateFrom = new DateTime(patientYearOfBirth, 1, 1);
            var birthDateTo = birthDateFrom.AddYears(1);
            searchCriterias.Add($"birthdate=ge{birthDateFrom:yyyy-MM-dd}");
            searchCriterias.Add($"birthdate=lt{birthDateTo:yyyy-MM-dd}");
            return await GetPatientsByCriterias(searchCriterias);
        }

        public async Task<FHIRPatientDetail> CreatePatient(FHIRPatientDetail patientDetail)
        {
            var client = await GetFHIRClient();
            var fhirPatient = GetFhirPatient(patientDetail);
            var result = await client.CreateAsync(fhirPatient);
            return _mapper.Map<FHIRPatientDetail>(result);
        }

        public async Task<FHIRPatientDetail> UpdatePatient(Guid patientId, FHIRPatientDetail patientDetail)
        {
            var client = await GetFHIRClient();
            var fhirPatient = GetFhirPatient(patientDetail);
            fhirPatient.Id = patientId.ToString();
            var result = await client.UpdateAsync(fhirPatient);
            return _mapper.Map<FHIRPatientDetail>(result);
        }

        public async Task DeletePatient(string patientId)
        {
            var client = await GetFHIRClient();
            var patientModel = await ReadFhirPatient(client, patientId);
            patientModel.Id = patientId;
            await client.DeleteAsync(patientModel);
        }

        public async Task RecordPatientOrder(PatientOrderRequest patientOrderRequest)
        {
            var client = await GetFHIRClient();

            var fhirPatient = await ReadFhirPatient(client, patientOrderRequest.PatientUUID.ToString());

            var lastOrderDateTime = GetExtensionByKey(fhirPatient.Extension, PatientFhirKeys.LAST_ORDER_DATE_TIME);
            lastOrderDateTime.Value = new FhirModel.FhirString(DateTime.UtcNow.ToString());

            var lastOrderNumber = GetExtensionByKey(fhirPatient.Extension, PatientFhirKeys.LAST_ORDER_NUMBER);
            lastOrderNumber.Value = new FhirModel.FhirString(patientOrderRequest.OrderNumber);

            await client.UpdateAsync(fhirPatient);
        }

        public async Task<FhirModel.Organization> CreateOrganization(FHIRHospice hospice)
        {
            var client = await GetFHIRClient();
            var fhirOrganization = GetFhirOrganization(hospice);
            var result = client.Create(fhirOrganization);
            return result;
        }

        public async Task<FhirModel.Organization> UpdateOrganization(FHIRHospice hospice)
        {
            var client = await GetFHIRClient();
            var fhirOrganization = GetFhirOrganization(hospice);
            var result = await client.UpdateAsync(fhirOrganization);
            return result;
        }

        public async Task<IEnumerable<FHIRPatientDetail>> GetPatientsByCriterias(List<string> searchCriterias)
        {
            if (searchCriterias == null) 
            {
                return null;
            }

            var client = await GetFHIRClient();
            var patientModels = new List<FhirModel.Patient>();

            var searchResult = await client.SearchUsingPostAsync<FhirModel.Patient>(searchCriterias.ToArray(), pageSize:1000);
            while (searchResult != null)
            {
                patientModels.AddRange(searchResult.Entry.Select(e => e.Resource as FhirModel.Patient));
                searchResult = await client.ContinueAsync(searchResult);
            }
            return _mapper.Map<IEnumerable<FHIRPatientDetail>>(patientModels);
        }

        public async Task<IEnumerable<FHIRPatientDetail>> GetPatientsByIds(IEnumerable<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return null;
            }

            var searchCriterias = new List<string>();
            var stringIds = string.Join(',', ids);
            searchCriterias.Add($"_id={stringIds}");
            return await GetPatientsByCriterias(searchCriterias);
        }

        public async Task<IEnumerable<FHIRPatientDetail>> SearchPatients(PatientSearchRequest patientSearchRequest)
        {
            var searchCriterias = new List<string>();

            if (patientSearchRequest.HospiceId != null)
            {
                searchCriterias.Add($"hospice-id={patientSearchRequest.HospiceId}");
            }
            if (!string.IsNullOrEmpty(patientSearchRequest.FirstName) || !string.IsNullOrEmpty(patientSearchRequest.LastName))
            {
                var names = new List<string>();
                if (!string.IsNullOrEmpty(patientSearchRequest.FirstName)) 
                {
                    names.Add(patientSearchRequest.FirstName);
                }
                if (!string.IsNullOrEmpty(patientSearchRequest.LastName)) 
                {
                    names.Add(patientSearchRequest.LastName);
                }
                searchCriterias.Add($"name={string.Join(",", names)}");
            }
            if (patientSearchRequest.DateOfBirth != null && patientSearchRequest.DateOfBirth != DateTime.MinValue)
            {
                searchCriterias.Add($"birthdate={patientSearchRequest.DateOfBirth:yyyy-MM-dd}");
            }
            if (patientSearchRequest.Address != null)
            {
                if (!string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine1))
                {
                    searchCriterias.Add($"address={patientSearchRequest.Address.AddressLine1}");
                }

                if (!string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine2))
                {
                    searchCriterias.Add($"address={patientSearchRequest.Address.AddressLine2}");
                }

                if (!string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine3))
                {
                    searchCriterias.Add($"address={patientSearchRequest.Address.AddressLine3}");
                }

                if (!string.IsNullOrEmpty(patientSearchRequest.Address.City))
                {
                    searchCriterias.Add($"address-city={patientSearchRequest.Address.City}");
                }
                if (!string.IsNullOrEmpty(patientSearchRequest.Address.State))
                {
                    searchCriterias.Add($"address-state={patientSearchRequest.Address.State}");
                }
                if (patientSearchRequest.Address.ZipCode != 0)
                {
                    searchCriterias.Add($"address-postalcode={patientSearchRequest.Address.ZipCode}");
                }
            }
            return await GetPatientsByCriterias(searchCriterias);
        }

        private async Task<FhirClient> GetFHIRClient()
        {
            try
            {
                var accessToken = await _tokenService.GetAccessTokenByClientCredentials(_fhirConfig.IdentityClient);

                var client = new FhirClient(_fhirConfig.ApiUrl, true)
                {
                    PreferredFormat = ResourceFormat.Json,
                    UseFormatParam = true,
                    PreferredReturn = Prefer.ReturnRepresentation
                };
                client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
                {
                    e.RawRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
                };
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while getting fhir client: {ex.Message}");
                throw ex;
            }
        }

        private List<FhirModel.Address> GenerateAddresses(PatientDetail patientDetail)
        {
            var fhirAddresses = new List<FhirModel.Address>();
            foreach (var patientAddress in patientDetail.PatientAddress)
            {
                var address = GenerateAddress(patientAddress.Address);
                var addressExtensionAddressTypeId = GetDecimalExtension(PatientFhirKeys.ADDRESS_TYPE_ID, patientAddress.AddressTypeId);
                address.Extension.Add(addressExtensionAddressTypeId);
                fhirAddresses.Add(address);
            }
            return fhirAddresses;
        }

        private FhirModel.Address GenerateAddress(Address addressRequest)
        {
            var addressExtensionLatitude = GetDecimalExtension(PatientFhirKeys.LATITUDE, addressRequest.Latitude);
            var addressExtensionLongitude = GetDecimalExtension(PatientFhirKeys.LONGITUDE, addressRequest.Longitude);
            var addressExtensionPlus4Code = GetDecimalExtension(PatientFhirKeys.PLUS4_CODE, addressRequest.Plus4Code);
            var addressExtensionisVerified = GetBooleanExtension(PatientFhirKeys.IS_VERIFIED, addressRequest.IsVerified);
            var addressExtensionAddressUuid = GetStringExtension(PatientFhirKeys.ADDRESS_UUID, addressRequest.AddressUuid.ToString());

            var address = new FhirModel.Address()
            {
                Line = new string[] { addressRequest.AddressLine1, addressRequest.AddressLine2, addressRequest.AddressLine3 },
                City = addressRequest.City,
                State = addressRequest.State,
                PostalCode = addressRequest.ZipCode.ToString(),
                Country = addressRequest.Country
            };
            address.Extension.Add(addressExtensionLatitude);
            address.Extension.Add(addressExtensionLongitude);
            address.Extension.Add(addressExtensionPlus4Code);
            address.Extension.Add(addressExtensionisVerified);
            address.Extension.Add(addressExtensionAddressUuid);
            return address;
        }

        private FhirModel.Extension GetStringExtension(string url, string addressText)
        {
            return new FhirModel.Extension()
            {
                Url = url,
                Value = new FhirModel.FhirString(addressText)
            };
        }

        private List<FhirModel.Extension> GetExtensions(FHIRPatientDetail patientDetail)
        {

            var extensions = new List<FhirModel.Extension>()
            {
                GetBooleanExtension(PatientFhirKeys.IS_INFECTIOUS, patientDetail.IsInfectious),

                GetStringExtension(PatientFhirKeys.DIAGNOSIS, patientDetail.Diagnosis),
                GetStringExtension(PatientFhirKeys.PATIENT_CREATED_DATE_TIME, patientDetail.CreatedDateTime.ToString()),

                GetDecimalExtension(PatientFhirKeys.HEIGHT, patientDetail.PatientHeight),
                GetDecimalExtension(PatientFhirKeys.WEIGHT, patientDetail.PatientWeight),
                GetDecimalExtension(PatientFhirKeys.LOCATION_ID, patientDetail.HospiceLocationId),
                GetDecimalExtension(PatientFhirKeys.HOSPICE_ID, patientDetail.HospiceId),
                GetDecimalExtension(PatientFhirKeys.PATIENT_CREATED_BY_USER_ID, patientDetail.CreatedByUserId)
            };

            if (patientDetail.PatientNotes != null && patientDetail.PatientNotes.Count() > 0)
            {
                foreach (var patientNote in patientDetail.PatientNotes)
                {
                    var patientNotes = new FhirModel.Annotation()
                    {
                        Text = new FhirModel.Markdown(patientNote.Note),
                        Author = new FhirModel.FhirString(patientNote.CreatedByUserId.ToString()),
                        Time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")
                    };
                    extensions.Add(new FhirModel.Extension(PatientFhirKeys.NOTES, patientNotes));
                }
            }
            return extensions;
        }

        private FhirModel.Extension GetBooleanExtension(string url, bool? flag)
        {
            return new FhirModel.Extension()
            {
                Url = url,
                Value = new FhirModel.FhirBoolean(flag)
            };
        }

        private FhirModel.Extension GetDecimalExtension(string url, decimal? value)
        {
            return new FhirModel.Extension()
            {
                Url = url,
                Value = new FhirModel.FhirDecimal(value)
            };
        }

        private FhirModel.Patient GetFhirPatient(FHIRPatientDetail patientDetail)
        {
            var fhirPatient = new FhirModel.Patient();
            var name = new FhirModel.HumanName().WithGiven(patientDetail.FirstName).AndFamily(patientDetail.LastName);
            name.Use = FhirModel.HumanName.NameUse.Official;
            fhirPatient.Name.Add(name);

            fhirPatient.BirthDate = patientDetail.DateOfBirth.ToString("yyyy-MM-dd");

            fhirPatient.Address.AddRange(GenerateAddresses(patientDetail));

            fhirPatient.Extension = GetExtensions(patientDetail);
            foreach (var phoneNumber in patientDetail.PhoneNumbers)
            {
                var telecom = new FhirModel.ContactPoint
                {
                    Value = phoneNumber.Number.ToString(),
                    Rank = phoneNumber.IsPrimary.Equals(true) ? 1 : 0
                    //Use = (FhirModel.ContactPoint.ContactPointUse)phoneNumber.NumberTypeId
                };
                telecom.Extension.Add(GetBooleanExtension(PatientFhirKeys.ETA_MESSAGE, phoneNumber.ReceiveEtaTextmessage));
                telecom.Extension.Add(GetBooleanExtension(PatientFhirKeys.SURVEY_MESSAGE, phoneNumber.ReceiveSurveyTextMessage));
                telecom.Extension.Add(GetBooleanExtension(PatientFhirKeys.IS_SELF_PHONE, phoneNumber.IsSelfPhone));
                telecom.Extension.Add(GetDecimalExtension(PatientFhirKeys.PHONE_NUMBER_TYPE, phoneNumber.NumberTypeId));
                telecom.Extension.Add(GetStringExtension(PatientFhirKeys.PHONE_NUMBER_CONTACT_NAME, phoneNumber.ContactName));

                fhirPatient.Telecom.Add(telecom);
            }

            if (patientDetail.FhirOrganizationId.HasValue && patientDetail.FhirOrganizationId != Guid.Empty) 
            {
                var organizationReference = new FhirModel.ResourceReference();
                organizationReference.ElementId = patientDetail.FhirOrganizationId.Value.ToString();
                fhirPatient.ManagingOrganization = organizationReference;
            }

            return fhirPatient;
        }

        private FhirModel.Organization GetFhirOrganization(FHIRHospice hospice)
        {
            var fhirOrganization = new FhirModel.Organization();

            var netsuiteId = new FhirModel.Identifier
            {
                System = PatientFhirKeys.NETSUITE_ID,
                Value = hospice.NetSuiteCustomerId.ToString()
            };
            fhirOrganization.Identifier.Add(netsuiteId);

            fhirOrganization.Name = hospice.Name;

            if (hospice.Address != null)
            {
                var address = GenerateAddress(_mapper.Map<Address>(hospice.Address));
                fhirOrganization.Address.Add(address);
            }

            if (hospice.PhoneNumber != null)
            {
                var telecom = new FhirModel.ContactPoint
                {
                    Value = hospice.PhoneNumber.Number.ToString()
                };
                fhirOrganization.Telecom.Add(telecom);
            }

            if (hospice.HospiceLocations != null && hospice.HospiceLocations.Any())
            {
                foreach (var location in hospice.HospiceLocations)
                {
                    if (location.Address != null)
                    {
                        var address = GenerateAddress(_mapper.Map<Address>(location.Address));
                        fhirOrganization.Address.Add(address);
                    }

                    if (location.PhoneNumber != null)
                    {
                        var telecom = new FhirModel.ContactPoint
                        {
                            Value = location.PhoneNumber.Number.ToString()
                        };
                        fhirOrganization.Telecom.Add(telecom);
                    }
                }
            }

            if (hospice.FhirOrganizationId.HasValue && hospice.FhirOrganizationId != Guid.Empty) 
            {
                fhirOrganization.Id = hospice.FhirOrganizationId.Value.ToString();
            }

            return fhirOrganization;
        }

        private async Task<FhirModel.Patient> ReadFhirPatient(FhirClient client, string patientUuid)
        {
            try
            {
                return await client.ReadAsync<FhirModel.Patient>($"Patient/{patientUuid}");
            }
            catch (FhirOperationException)
            {
                throw new ValidationException($"Patient UUID ({patientUuid}) is not valid");
            }
        }

        private FhirModel.Extension GetExtensionByKey(List<FhirModel.Extension> extensions, string key)
        {
            var extension = extensions.FirstOrDefault(e => string.Equals(e.Url, key, StringComparison.OrdinalIgnoreCase));

            if (extension == null)
            {
                extension = new FhirModel.Extension()
                {
                    Url = key
                };
                extensions.Add(extension);
            }
            return extension;
        }
    }
}
