using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Patient.ViewModels;
using Sieve.Models;
using HMSDigital.Patient.BusinessLayer.Interfaces;
using System.ComponentModel.DataAnnotations;
using HMSDigital.Patient.BusinessLayer.Validations;
using Microsoft.AspNetCore.JsonPatch;
using LinqKit;
using HMSDigital.Patient.Data.Repositories.Interfaces;
using HMSDigital.Patient.Data.Models;
using CommonEnums = HMSDigital.Common.BusinessLayer;
using PatientEnums = HMSDigital.Patient.Data.Enums;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using Patient.ViewModels.NetSuite;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Patient.FHIR.Models;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using Newtonsoft.Json;

namespace HMSDigital.Patient.BusinessLayer
{
    public class PatientService : IPatientService
    {

        private readonly IMapper _mapper;

        private readonly ILogger<PatientService> _logger;

        private readonly IPatientsRepository _patientsRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly IAddressesRepository _addressesRepository;

        private readonly IPatientAddressRepository _patientAddressRepository;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IPaginationService _paginationService;

        private readonly IPatientNotesRepository _patientNotesRepository;

        private readonly IPatientMergeHistoryRepository _patientMergeHistoryRepository;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly IFHIRQueueService<FHIRPatientDetail> _fhirQueueService;

        private readonly IFHIRService _fHIRService;

        private readonly HttpContext _httpContext;

        public PatientService(
            IPatientsRepository patientsRepository,
            IPatientNotesRepository patientNotesRepository,
            IPatientAddressRepository patientAddressRepository,
            IUsersRepository usersRepository,
            IAddressesRepository adressesesRepository,
            IHospiceRepository hospiceRepository,
            IMapper mapper,
            IPaginationService paginationService,
            IAddressStandardizerService addressStandardizerService,
            IPatientMergeHistoryRepository patientMergeHistoryRepository,
            IFHIRQueueService<FHIRPatientDetail> fhirQueueService,
            IFHIRService fHIRService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PatientService> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _usersRepository = usersRepository;
            _addressesRepository = adressesesRepository;
            _patientMergeHistoryRepository = patientMergeHistoryRepository;
            _patientsRepository = patientsRepository;
            _patientNotesRepository = patientNotesRepository;
            _patientAddressRepository = patientAddressRepository;
            _hospiceRepository = hospiceRepository;
            _paginationService = paginationService;
            _addressStandardizerService = addressStandardizerService;
            _fhirQueueService = fhirQueueService;
            _fHIRService = fHIRService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<PaginatedList<PatientDetail>> GetAllPatients(SieveModel sieveModel, bool ignoreFilter = false)
        {
            _patientsRepository.SieveModel = sieveModel;
            var patientPredicate = await GetPatientPredicate(ignoreFilter);
            var totalRecords = await _patientsRepository.GetCountAsync(patientPredicate);
            var patientModels = await _patientsRepository.GetManyAsync(patientPredicate);
            var patients = _mapper.Map<IEnumerable<PatientDetail>>(patientModels);
            return _paginationService.GetPaginatedList(patients, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PatientDetail> GetPatientById(int patientId)
        {
            var patientModel = await _patientsRepository.GetByIdAsync(patientId);
            return _mapper.Map<PatientDetail>(await GetAccessiblePatient(patientModel));
        }

        public async Task<IEnumerable<PatientNote>> GetPatientNotes(int patientId, bool ignoreFilter)
        {
            var patientModel = await GetAccessiblePatient(await _patientsRepository.GetByIdAsync(patientId), ignoreFilter);
            if (patientModel == null)
            {
                return new List<PatientNote>();
            }
            var patientNotes = _mapper.Map<IEnumerable<PatientNote>>(patientModel.PatientNotes);
            var userIds = patientNotes.Select(pn => pn.CreatedByUserId);
            var users = await _usersRepository.GetManyAsync(u => userIds.Contains(u.Id));
            foreach (var patientNote in patientNotes)
            {
                var createdByUser = users.FirstOrDefault(u => patientNote.CreatedByUserId == u.Id);
                if (createdByUser != null)
                {
                    patientNote.CreatedByUserName = $"{createdByUser.FirstName} {createdByUser.LastName}";
                }
            }
            return patientNotes;
        }

        public async Task<PatientLookUp> GetPatientByPatientUuid(string patientUuid)
        {
            var PatientModel = await _patientsRepository.GetAsync(p => p.UniqueId.ToString() == patientUuid);
            return _mapper.Map<PatientLookUp>(PatientModel);
        }

        public async Task<PaginatedList<PatientDetail>> SearchPatientsBySearchQuery(SieveModel sieveModel, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return null;
            }
            searchQuery = searchQuery.Trim();

            _patientsRepository.SieveModel = sieveModel;

            Guid patientUniqueId = Guid.Empty;
            if (Guid.TryParse(searchQuery, out patientUniqueId))
            {
                return await SearchPatientsByUniqueId(sieveModel, patientUniqueId);
            }

            DateTime patientDateOfBirth = DateTime.MinValue;
            if (DateTime.TryParse(searchQuery, out patientDateOfBirth))
            {
                return await SearchPatientsByBirthDate(sieveModel, patientDateOfBirth);
            }

            int patientYearOfBirth = 0;
            if (int.TryParse(searchQuery, out patientYearOfBirth))
            {
                return await SearchPatientsByYearOfBirth(sieveModel, patientYearOfBirth);
            }

            return await SearchPatientsByName(sieveModel, searchQuery);
        }

        private async Task<PaginatedList<PatientDetail>> SearchPatientsByUniqueId(SieveModel sieveModel, Guid uniqueId)
        {
            var totalCountById = await _patientsRepository.GetCountAsync(p =>
                p.UniqueId == uniqueId
            );
            var patientModelsById = await _patientsRepository.GetManyAsync(p =>
                p.UniqueId == uniqueId
            );

            var patientsById = _mapper.Map<IEnumerable<PatientDetail>>(await GetAccessiblePatients(patientModelsById));
            return _paginationService.GetPaginatedList(patientsById, totalCountById, sieveModel.Page, sieveModel.PageSize);
        }

        private async Task<PaginatedList<PatientDetail>> SearchPatientsByBirthDate(SieveModel sieveModel, DateTime dateOfBirth)
        {
            var totalCountByBirthDate = await _patientsRepository.GetCountAsync(p =>
                p.DateOfBirth != null
                && p.DateOfBirth.Value.Date == dateOfBirth.Date
            );
            var patientModelsByBirthDate = await _patientsRepository.GetManyAsync(p =>
                p.DateOfBirth != null
                && p.DateOfBirth.Value.Date == dateOfBirth.Date
            );

            var patientsByBirthDate = _mapper.Map<IEnumerable<PatientDetail>>(await GetAccessiblePatients(patientModelsByBirthDate));
            return _paginationService.GetPaginatedList(patientsByBirthDate, totalCountByBirthDate, sieveModel.Page, sieveModel.PageSize);
        }

        private async Task<PaginatedList<PatientDetail>> SearchPatientsByYearOfBirth(SieveModel sieveModel, int yearOfBirth)
        {
            var totalCountByYearOfBirth = await _patientsRepository.GetCountAsync(p =>
                p.DateOfBirth != null
                && p.DateOfBirth.Value.Year == yearOfBirth
            );
            var patientModelsByYearOfBirth = await _patientsRepository.GetManyAsync(p =>
                p.DateOfBirth != null
                && p.DateOfBirth.Value.Year == yearOfBirth
            );

            var patientsByYearOfBirth = _mapper.Map<IEnumerable<PatientDetail>>(await GetAccessiblePatients(patientModelsByYearOfBirth));
            return _paginationService.GetPaginatedList(patientsByYearOfBirth, totalCountByYearOfBirth, sieveModel.Page, sieveModel.PageSize);
        }

        private async Task<PaginatedList<PatientDetail>> SearchPatientsByName(SieveModel sieveModel, string name)
        {
            name = name.ToLower();
            if (name.Split(' ').Length == 1)
            {
                var totalCount = await _patientsRepository.GetCountAsync(p =>
                    p.FirstName.ToLower().Contains(name)
                    || p.LastName.ToLower().Contains(name)
                );
                var patientModels = await _patientsRepository.GetManyAsync(p =>
                    p.FirstName.ToLower().Contains(name)
                    || p.LastName.ToLower().Contains(name)
                );

                var patients = _mapper.Map<IEnumerable<PatientDetail>>(await GetAccessiblePatients(patientModels));
                return _paginationService.GetPaginatedList(patients, totalCount, sieveModel.Page, sieveModel.PageSize);
            }

            var totalCountByName = await _patientsRepository.GetCountAsync(p =>
                    p.FirstName.ToLower().Contains(name)
                    || p.LastName.ToLower().Contains(name)
                    || (p.FirstName.ToLower() + " " + p.LastName.ToLower()).Contains(name)
                    || (p.LastName.ToLower() + " " + p.FirstName.ToLower()).Contains(name)
            );
            var patientModelsByName = await _patientsRepository.GetManyAsync(p =>
                    p.FirstName.ToLower().Contains(name)
                    || p.LastName.ToLower().Contains(name)
                    || (p.FirstName.ToLower() + " " + p.LastName.ToLower()).Contains(name)
                    || (p.LastName.ToLower() + " " + p.FirstName.ToLower()).Contains(name)
            );

            var patientsByName = _mapper.Map<IEnumerable<PatientDetail>>(await GetAccessiblePatients(patientModelsByName));
            return _paginationService.GetPaginatedList(patientsByName, totalCountByName, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PatientDetail> CreatePatient(PatientCreateRequest patientCreateRequest)
        {
            var patientModel = await CreateHmsPatient(patientCreateRequest);
            return _mapper.Map<PatientDetail>(patientModel);
        }

        public async Task<PatientDetail> PatchPatient(int patientId, JsonPatchDocument<PatientDetail> patientPatchDocument, bool doNotVerifyAddress)
        {
            var patientModel = await ValidatePatchDocument(patientId, patientPatchDocument);
            await PatchHmsPatient(patientModel, patientPatchDocument, doNotVerifyAddress);
            return _mapper.Map<PatientDetail>(patientModel);
        }

        public async Task<PatientDetail> UpdatePatientStatus(int patientId, PatientStatusRequest patientStatusRequest)
        {
            try
            {
                int? reasonId = null;
                if (!Enum.TryParse(patientStatusRequest.Status, true, out PatientEnums.PatientStatusTypes patientStatus))
                {
                    throw new ValidationException($"Requested Patient status type {patientStatusRequest.Status} is not valid");
                }

                if (patientStatusRequest.StatusChangedDate == null || patientStatusRequest.StatusChangedDate == DateTime.MinValue)
                {
                    throw new ValidationException($"Status changed date cannot be null or empty");
                }

                if (!string.IsNullOrEmpty(patientStatusRequest.Reason))
                {
                    if (!Enum.TryParse(patientStatusRequest.Reason, true, out PatientEnums.PatientStatusTypes reason))
                    {
                        throw new ValidationException($"Requested Patient reason {patientStatusRequest.Reason} is not valid");
                    }
                    reasonId = (int)reason;
                }

                var patientModel = await _patientsRepository.GetByIdAsync(patientId);

                if (patientModel == null || await GetAccessiblePatient(patientModel) == null)
                {
                    throw new ValidationException($"Patient with Id {patientId} not found");
                }

                patientModel.StatusId = (int)patientStatus;
                patientModel.StatusChangedDate = patientStatusRequest.StatusChangedDate;
                patientModel.StatusReasonId = reasonId;

                await _patientsRepository.UpdateAsync(patientModel);
                return _mapper.Map<PatientDetail>(patientModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while updating patient status with Id({patientId}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while updating patient status with Id({patientId}): {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<PatientStatusValidationResponse>> ValidatePatientStatus(IEnumerable<PatientStatusValidationRequest> patientStatusRequests, bool applyFix)
        {
            var validationResults = new List<PatientStatusValidationResponse>();
            var orderStatusValidator = new PatientStatusValidator();

            foreach (var patientStatusRequest in patientStatusRequests)
            {
                var validationResult = await orderStatusValidator.ValidateAsync(patientStatusRequest);

                validationResults.Add(new PatientStatusValidationResponse { HasValidStatus = validationResult.IsValid, PatientUuid = patientStatusRequest.PatientUuid });

                if (applyFix && !validationResult.IsValid)
                {
                    var patientStatus = _mapper.Map<PatientStatusRequest>(patientStatusRequest);
                    patientStatus.StatusChangedDate = DateTime.UtcNow;
                    await FixPatientStatus(patientStatus);
                }
            }

            return validationResults;
        }


        public async Task FixPatientStatus(PatientStatusRequest patientStatusRequest)
        {
            try
            {
                var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientStatusRequest.PatientUuid);
                if (patientModel == null)
                {
                    throw new ValidationException($"Patient with uniqueId {patientStatusRequest.PatientUuid} not found");
                }

                await UpdateStatus(patientStatusRequest.Reason, patientStatusRequest.IsDMEEquipmentLeft, patientStatusRequest.HasOpenOrders, patientStatusRequest.StatusChangedDate, patientModel, patientStatusRequest.HasOrders);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Recording order fulfillment with uniqueId({patientStatusRequest.PatientUuid}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Recording order fulfillment with uniqueId({patientStatusRequest.PatientUuid}): {ex.Message}");
                throw ex;
            }
        }

        private async Task<PatientDetail> UpdateStatus(string statusReason, bool hasInventory, bool hasOpenOrders, DateTime statusChangedDate, PatientDetails patientModel, bool hasOrders)
        {
            var reasonId = patientModel.StatusReasonId;
            if (statusChangedDate == null || statusChangedDate == DateTime.MinValue)
            {
                throw new ValidationException($"Status changed date cannot be null or empty");
            }

            if (!string.IsNullOrEmpty(statusReason))
            {
                if (!Enum.TryParse(statusReason.Replace(" ", string.Empty), true, out PatientEnums.PatientStatusTypes reason))
                {
                    throw new ValidationException($"Requested Patient reason {statusReason} is not valid");
                }

                reasonId = (int)reason;
            }

            PatientEnums.PatientStatusTypes status;

            //If patient doesn't have inventory and all of its orders are closed, patient status should be Red I (Inactive). 
            //If patient doesn't have inventory and all of its open orders meet the following conditions: status = partially fulfilled and is marked as exception, patient status should be Red I (Inactive). 
            if (!hasInventory && !hasOpenOrders)
            {
                status = hasOrders ? PatientEnums.PatientStatusTypes.Inactive : PatientEnums.PatientStatusTypes.Blank;
            }

            //If patient has inventory and all of its orders are closed, patient status should be Green A (Active). 
            //If patient has inventory and all of its open orders meet the following conditions: status = partially fulfilled and is marked as exception, patient status should be Green A (Active). 
            else if (hasInventory && !hasOpenOrders)
            {
                status = PatientEnums.PatientStatusTypes.Active;
            }

            //Patients with open orders
            else
            {
                status = hasInventory ? PatientEnums.PatientStatusTypes.PendingActive : PatientEnums.PatientStatusTypes.Pending;
            }

            patientModel.StatusId = (int)status;
            patientModel.StatusChangedDate ??= statusChangedDate;
            patientModel.StatusReasonId = reasonId;

            await _patientsRepository.UpdateAsync(patientModel);

            return _mapper.Map<PatientDetail>(patientModel);
        }

        public async Task UpdatePatientStatusByPatientUuid(Guid patientUuid, PatientStatusRequest patientStatusRequest)
        {
            try
            {
                int? reasonId = null;
                if (!Enum.TryParse(patientStatusRequest.Status, true, out PatientEnums.PatientStatusTypes patientStatus))
                {
                    throw new ValidationException($"Requested Patient status type {patientStatusRequest.Status} is not valid");
                }

                if (patientStatusRequest.StatusChangedDate == null || patientStatusRequest.StatusChangedDate == DateTime.MinValue)
                {
                    throw new ValidationException($"Status changed date cannot be null or empty");
                }

                if (!string.IsNullOrEmpty(patientStatusRequest.Reason))
                {
                    if (!Enum.TryParse(patientStatusRequest.Reason, true, out PatientEnums.PatientStatusTypes reason))
                    {
                        throw new ValidationException($"Requested Patient reason {patientStatusRequest.Reason} is not valid");
                    }
                    reasonId = (int)reason;
                }

                var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientUuid);

                if (patientModel == null)
                {
                    throw new ValidationException($"Patient with uniqueId {patientUuid} not found");
                }

                patientModel.StatusId = (int)patientStatus;
                patientModel.StatusChangedDate = patientStatusRequest.StatusChangedDate;
                patientModel.StatusReasonId = reasonId;

                await _patientsRepository.UpdateAsync(patientModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while updating patient status with uniqueId({patientUuid}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while updating patient status with uniqueId({patientUuid}): {ex.Message}");
                throw ex;
            }
        }

        public async Task RecordOrderFulfillment(FulfillmentRecordRequest fulfillmentRecordRequest)
        {
            try
            {
                var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == fulfillmentRecordRequest.PatientUUID);
                if (patientModel == null)
                {
                    throw new ValidationException($"Patient with uniqueId {fulfillmentRecordRequest.PatientUUID} not found");
                }

                await UpdateStatus(fulfillmentRecordRequest.Reason, fulfillmentRecordRequest.IsDMEEquipmentLeft, fulfillmentRecordRequest.HasOpenOrders, fulfillmentRecordRequest.StatusChangedDate, patientModel, true);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Recording order fulfillment with uniqueId({fulfillmentRecordRequest.PatientUUID}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Recording order fulfillment with uniqueId({fulfillmentRecordRequest.PatientUUID}): {ex.Message}");
                throw ex;
            }
        }

        public async Task UpdatePatientHospiceByPatientUuid(Guid patientUuid, PatientHospiceRequest patientHospiceRequest)
        {
            try
            {
                var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientUuid);

                if (patientModel == null)
                {
                    throw new ValidationException($"Patient with uniqueId {patientUuid} not found");
                }

                patientModel.HospiceId = patientHospiceRequest.HospiceId;
                patientModel.HospiceLocationId = patientHospiceRequest.HospiceLocationId;

                await _patientsRepository.UpdateAsync(patientModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while updating patient hospice with uniqueId({patientUuid}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while updating patient hospice with uniqueId({patientUuid}): {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<PatientDetail>> SearchPatients(PatientSearchRequest patientSearchRequest)
        {
            var predicate = PredicateBuilder.New<PatientDetails>(false);
            var predicateHospice = PredicateBuilder.New<PatientDetails>(false);
            if (patientSearchRequest.HospiceId != null)
            {
                predicateHospice.And(d => d.HospiceId == patientSearchRequest.HospiceId);
            }
            if (!string.IsNullOrEmpty(patientSearchRequest.FirstName))
            {
                predicate.Or(d => d.FirstName == patientSearchRequest.FirstName);
            }
            if (!string.IsNullOrEmpty(patientSearchRequest.LastName))
            {
                predicate.Or(d => d.LastName == patientSearchRequest.LastName);
            }
            if (patientSearchRequest.DateOfBirth != null && patientSearchRequest.DateOfBirth != DateTime.MinValue)
            {
                predicate.Or(d => d.DateOfBirth == patientSearchRequest.DateOfBirth);
            }
            if (patientSearchRequest.Address != null)
            {
                if (!string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine1))
                {
                    predicate.Or(d => d.PatientAddress.Any(pa => pa.Address.AddressLine1 == patientSearchRequest.Address.AddressLine1));
                }

                if (!string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine2))
                {
                    predicate.Or(d => d.PatientAddress.Any(pa => pa.Address.AddressLine2 == patientSearchRequest.Address.AddressLine2));
                }

                if (!string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine3))
                {
                    predicate.Or(d => d.PatientAddress.Any(pa => pa.Address.AddressLine3 == patientSearchRequest.Address.AddressLine3));
                }

                if (!string.IsNullOrEmpty(patientSearchRequest.Address.City))
                {
                    predicate.Or(d => d.PatientAddress.Any(pa => pa.Address.City == patientSearchRequest.Address.City));
                }
                if (!string.IsNullOrEmpty(patientSearchRequest.Address.State))
                {
                    predicate.Or(d => d.PatientAddress.Any(pa => pa.Address.State == patientSearchRequest.Address.State));
                }
                if (patientSearchRequest.Address.ZipCode != 0)
                {
                    predicate.Or(d => d.PatientAddress.Any(pa => pa.Address.ZipCode == patientSearchRequest.Address.ZipCode));
                }
            }
            predicateHospice.And(predicate);
            try
            {
                var patientModels = await _patientsRepository.GetManyAsync(predicateHospice);
                return _mapper.Map<IEnumerable<PatientDetail>>(await GetAccessiblePatients(patientModels));
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while search patients: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while search patients: {ex.Message}");
                throw ex;
            }
        }

        public async Task<Address> GetPatientAddressByUuid(Guid addressUUID)
        {
            var address = await _addressesRepository.GetAsync(a => a.AddressUuid == addressUUID);
            return _mapper.Map<Address>(address);
        }

        public async Task<PaginatedList<PatientLookUp>> GetAllPatientLookUp(SieveModel sieveModel)
        {
            var patientPaginatedList = await GetAllPatients(sieveModel, true);
            return _mapper.Map<PaginatedList<PatientLookUp>>(patientPaginatedList);
        }

        public async Task<PatientDetail> CreatePatientV2(PatientCreateRequest patientCreateRequest)
        {
            var patientModel = await CreateHmsPatient(patientCreateRequest);
            var fhirPatientDetail = _mapper.Map<FHIRPatientDetail>(patientModel);
            fhirPatientDetail.FhirOrganizationId = await GetFhirOrganizationId(fhirPatientDetail.HospiceId);
            try
            {
                await _fhirQueueService.QueueCreateRequest(fhirPatientDetail);
            }
            catch
            {
                _logger.LogInformation($"Exception occurred while sending FHIR patient request to service bus");
                await DeleteHmsPatient(patientModel.Id);
                throw;
            }
            return fhirPatientDetail;
        }

        public async Task<PatientDetail> PatchPatientV2(int patientId, JsonPatchDocument<PatientDetail> patientPatchDocument, bool doNotVerifyAddress)
        {
            var patientModel = await ValidatePatchDocument(patientId, patientPatchDocument);

            if (!patientModel.FhirPatientId.HasValue || patientModel.FhirPatientId == Guid.Empty)
            {
                throw new ValidationException($"Patient with Id ({patientId}) has not been created on FHIR server yet");
            }

            var fhirPatient = await PatchFhirPatient(patientPatchDocument, patientModel.FhirPatientId.Value, doNotVerifyAddress);
            await PatchHmsPatient(patientModel, patientPatchDocument, doNotVerifyAddress);
            return fhirPatient;
        }

        private async Task<PatientDetails> CreateHmsPatient(PatientCreateRequest patientCreateRequest)
        {
            if (patientCreateRequest.Hms2Id.HasValue)
            {
                var existingPatientModel = await _patientsRepository.GetAsync(p => p.Hms2Id == patientCreateRequest.Hms2Id.Value);
                if (existingPatientModel != null)
                {
                    return existingPatientModel;
                }
            }
            var patientValidator = new PatientValidator();
            var validationResult = patientValidator.Validate(patientCreateRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            if (patientCreateRequest.PatientAddress != null)
            {
                foreach (var patientAddress in patientCreateRequest.PatientAddress)
                {
                    if (!Enum.IsDefined(typeof(CommonEnums.Enums.AddressType), patientAddress.AddressTypeId))
                    {
                        throw new ValidationException($"Request patient address type {patientAddress.AddressTypeId} is not valid");
                    }
                }
            }

            if (patientCreateRequest.PhoneNumbers != null)
            {
                foreach (var phoneNumber in patientCreateRequest.PhoneNumbers)
                {
                    if (!Enum.IsDefined(typeof(CommonEnums.Enums.PhoneNumberType), phoneNumber.NumberTypeId))
                    {
                        throw new ValidationException($"Request patient phone number type {phoneNumber.NumberTypeId} is not valid");
                    }
                }
            }

            if (patientCreateRequest.StatusId != null)
            {
                if (!Enum.IsDefined(typeof(Data.Enums.PatientStatusTypes), patientCreateRequest.StatusId))
                {
                    throw new ValidationException($"Request patient status id {patientCreateRequest.StatusId} is not valid");
                }
            }

            try
            {
                var patientModel = _mapper.Map<PatientDetails>(patientCreateRequest);
                var patientAddressModels = new List<Data.Models.PatientAddress>();
                foreach (var patientAddress in patientCreateRequest.PatientAddress)
                {
                    var patientAddressModel = _mapper.Map<Data.Models.PatientAddress>(patientAddress);
                    if (!patientAddress.DoNotVerifyAddress)
                    {
                        try
                        {
                            var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(patientAddress.Address));
                            if (standardizedAddress != null)
                            {
                                _mapper.Map(standardizedAddress, patientAddressModel.Address);
                            }
                        }
                        catch (ValidationException vx)
                        {
                            _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                        }
                    }
                    if (patientAddressModel.Address.AddressUuid == null || patientAddressModel.Address.AddressUuid == Guid.Empty)
                    {
                        patientAddressModel.Address.AddressUuid = Guid.NewGuid();
                    }
                    patientAddressModels.Add(patientAddressModel);
                }
                patientModel.PatientAddress = patientAddressModels;
                patientModel.UniqueId = Guid.NewGuid();
                patientModel.StatusId = patientCreateRequest.StatusId ?? (int)Data.Enums.PatientStatusTypes.Blank;
                patientModel.StatusChangedDate = DateTime.UtcNow;
                await _patientsRepository.AddAsync(patientModel);
                return patientModel;
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Creating patient: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Creating patient: {ex.Message}");
                throw ex;
            }
        }

        private async Task PatchHmsPatient(PatientDetails patientModel, JsonPatchDocument<PatientDetail> patientPatchDocument, bool doNotVerifyAddress)
        {
            var modelPatch = _mapper.Map<JsonPatchDocument<PatientDetails>>(patientPatchDocument);

            modelPatch.ApplyTo(patientModel);
            patientModel.DateOfBirth = patientModel.DateOfBirth?.Date;

            var validator = new PatientValidator();
            var result = validator.Validate(_mapper.Map<PatientCreateRequest>(_mapper.Map<PatientDetail>(patientModel)));
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }

            foreach (var patientAddress in patientModel.PatientAddress)
            {
                if (!doNotVerifyAddress)
                {
                    try
                    {
                        var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(patientAddress.Address));
                        if (standardizedAddress != null)
                        {
                            _mapper.Map(standardizedAddress, patientAddress.Address);
                        }
                    }
                    catch (ValidationException vx)
                    {
                        _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                    }
                }
                if (patientAddress.Address.AddressUuid == Guid.Empty || patientPatchDocument.Operations.Any(op => new Regex("^/patientAddress/\\d.*", RegexOptions.IgnoreCase).IsMatch(op.path)))
                {
                    patientAddress.Address.AddressUuid = Guid.NewGuid();
                }
            }

            await _patientsRepository.UpdateAsync(patientModel);
        }

        private async Task<PatientDetails> ValidatePatchDocument(int patientId, JsonPatchDocument<PatientDetail> patientPatchDocument)
        {
            var patientModel = await _patientsRepository.GetByIdAsync(patientId);
            if (patientModel == null)
            {
                throw new ValidationException($"Patient with Id ({patientId}) not found");
            }

            var allowedPaths = new List<string> {
                "^/firstName",
                "^/lastName",
                "^/hospiceId",
                "^/hospiceLocationId",
                "^/dateOfBirth",
                "^/patientHeight",
                "^/patientWeight",
                "^/isInfectious",
                "^/diagnosis",
                "^/patientAddress/\\d/address",
                "^/patientAddress/\\d",
                "^/patientAddress/\\d/address/addressLine1",
                "^/patientAddress/\\d/address/addressLine2",
                "^/patientAddress/\\d/address/addressLine3",
                "^/patientAddress/\\d/address/city",
                "^/patientAddress/\\d/address/state",
                "^/patientAddress/\\d/address/zipCode",
                "^/phoneNumbers/\\d/number",
                "^/phoneNumbers/\\d",
                "^/phoneNumbers/\\d/countryCode",
                "^/phoneNumbers/\\d/receiveEtaTextmessage",
                "^/phoneNumbers/\\d/receiveSurveyTextMessage",
                "^/phoneNumbers/\\d/contactName",
                "^/phoneNumbers/\\d/isSelfPhone",
                "^/patientNotes/\\d/note",
                "^/patientNotes"
            };

            string pattern = string.Join("|", allowedPaths);
            foreach (var op in patientPatchDocument.Operations)
            {
                if (!new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(op.path))
                {
                    throw new ValidationException($"Attempt to modify data outside of user control. Logged and reported.");
                }
            }

            var hospiceIdChanged = patientPatchDocument.Operations.Any(op => new Regex("^/hospiceId", RegexOptions.IgnoreCase).IsMatch(op.path));

            if (patientModel.LastOrderDateTime != null && hospiceIdChanged)
            {
                throw new ValidationException($"Cannot change hospice because orders for patient are already placed or fulfilled");
            }

            return patientModel;
        }

        private async Task<IEnumerable<PatientDetails>> GetAccessiblePatients(IEnumerable<PatientDetails> patientModels, bool ignoreFilter = false)
        {
            return patientModels.Where(await GetPatientPredicate(ignoreFilter));
        }

        private async Task<PatientDetails> GetAccessiblePatient(PatientDetails patientModel, bool ignoreFilter = false)
        {
            var patients = await GetAccessiblePatients(new List<PatientDetails>() { patientModel }, ignoreFilter);
            return patients.FirstOrDefault();
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return -1;
            }
            return int.Parse(userIdClaim.Value);
        }

        private async Task<ExpressionStarter<PatientDetails>> GetPatientPredicate(bool ignoreFilter = false)
        {
            var patientPredicate = PredicateBuilder.New<Data.Models.PatientDetails>(false);
            if (ignoreFilter)
            {
                return patientPredicate.Or(o => true);
            }

            var userId = GetLoggedInUserId();
            var hospiceIds = await _usersRepository.GetHospiceAccessByUserId(userId);
            if (hospiceIds.Contains("*"))
            {
                return patientPredicate.Or(o => true);
            }
            patientPredicate.Or(o => hospiceIds.Contains(o.HospiceId.ToString()));

            var hospiceLocationIds = await _usersRepository.GetHospiceLocationAccessByUserId(userId);
            if (hospiceLocationIds.Contains("*"))
            {
                return patientPredicate.Or(o => o.HospiceLocationId != null);
            }
            patientPredicate.Or(o => hospiceLocationIds.Contains(o.HospiceLocationId.ToString()));

            return patientPredicate;
        }

        public async Task RecordPatientOrder(PatientOrderRequest patientOrderRequest)
        {
            var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientOrderRequest.PatientUUID);
            if (patientModel == null)
            {
                throw new ValidationException($"Patient UUID ({patientOrderRequest.PatientUUID}) is not valid");
            }

            var patientStatus = patientOrderRequest.HasDMEEquipment ? PatientEnums.PatientStatusTypes.PendingActive : PatientEnums.PatientStatusTypes.Pending;

            patientModel.StatusId = (int)patientStatus;
            patientModel.StatusChangedDate = DateTime.UtcNow;

            patientModel.LastOrderNumber = patientOrderRequest.OrderNumber;
            patientModel.LastOrderDateTime = DateTime.UtcNow;
            await _patientsRepository.UpdateAsync(patientModel);
        }

        public async Task<PaginatedList<Address>> GetNonVerifiedAddresses(SieveModel sieveModel)
        {
            _addressesRepository.SieveModel = sieveModel;
            var totalRecords = await _addressesRepository.GetCountAsync(a => !a.IsVerified);
            var addressModels = await _addressesRepository.GetManyAsync(a => !a.IsVerified);
            var addresses = _mapper.Map<IEnumerable<Address>>(addressModels);
            return _paginationService.GetPaginatedList(addresses, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<Address> FixNonVerifiedAddress(int addressId)
        {
            var address = await _addressesRepository.GetByIdAsync(addressId);
            if (address == null)
            {
                throw new ValidationException($"Address with Id({addressId}) does not exist");
            }
            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(address));
                if (standardizedAddress != null && standardizedAddress.IsVerified)
                {
                    await _addressesRepository.UpdateAsync(_mapper.Map(standardizedAddress, address));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Address verification failed : ({ex.Message})");
                throw new ValidationException($"Address verification failed.");
            }
            return _mapper.Map<Address>(address);
        }

        public async Task<long> FixNonVerifiedAddresses()
        {
            var addresses = await _addressesRepository.GetManyAsync(a => !a.IsVerified);
            var updatedAddressCount = 0;
            foreach (var address in addresses)
            {
                try
                {
                    var standardizedAddress =
                        await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(address));
                    if (standardizedAddress != null && standardizedAddress.IsVerified)
                    {
                        await _addressesRepository.UpdateAsync(_mapper.Map(standardizedAddress, address));
                        updatedAddressCount++;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return updatedAddressCount;
        }

        public async Task<long> FixMissingFhirPatients()
        {
            var patientModels = await _patientsRepository.GetManyAsync(p => !p.FhirPatientId.HasValue);
            patientModels.Select(x => { x.FhirPatientId = Guid.Empty; return x; }).ToList();
            await _patientsRepository.UpdateManyAsync(patientModels);

            var fhirPatients = _mapper.Map<IEnumerable<FHIRPatientDetail>>(patientModels);
            _fhirQueueService.QueueCreateRequestList(fhirPatients);
            return patientModels.Count();
        }

        private async Task VerifyAddresses(IEnumerable<Addresses> addresses)
        {
            foreach (var address in addresses)
            {
                try
                {
                    var standardizedAddress =
                        await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(address));
                    if (standardizedAddress != null && standardizedAddress.IsVerified)
                    {
                        await _addressesRepository.UpdateAsync(_mapper.Map(standardizedAddress, address));
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        private async Task<Guid?> GetFhirOrganizationId(int hospiceId)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel != null)
            {
                return hospiceModel.FhirOrganizationId;
            }
            return null;
        }

        private async Task DeleteHmsPatient(int patientId)
        {
            var patientModel = await _patientsRepository.GetByIdAsync(patientId);
            if (patientModel == null)
            {
                return;
            }
            await _patientsRepository.DeleteAsync(patientModel);
        }

        private async Task<FHIRPatientDetail> PatchFhirPatient(JsonPatchDocument<PatientDetail> patientPatchDocument, Guid fhirPatientId, bool doNotVerifyAddress)
        {
            var fhirPatientDetail = await _fHIRService.GetPatientById(fhirPatientId.ToString());

            patientPatchDocument.ApplyTo(fhirPatientDetail);
            fhirPatientDetail.DateOfBirth = fhirPatientDetail.DateOfBirth.Date;
            var validator = new PatientValidator();
            var result = validator.Validate(_mapper.Map<PatientCreateRequest>(fhirPatientDetail));
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }

            foreach (var patientAddress in fhirPatientDetail.PatientAddress)
            {
                if (!doNotVerifyAddress)
                {
                    try
                    {
                        var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(patientAddress.Address);
                        if (standardizedAddress != null)
                        {
                            _mapper.Map(standardizedAddress, patientAddress.Address);
                        }
                    }
                    catch (ValidationException vx)
                    {
                        _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                    }
                }
                if (patientAddress.Address.AddressUuid == Guid.Empty)
                {
                    patientAddress.Address.AddressUuid = Guid.NewGuid();
                }
            }

            var hospiceIdChanged = patientPatchDocument.Operations.Any(op => new Regex("^/hospiceId", RegexOptions.IgnoreCase).IsMatch(op.path));

            if (hospiceIdChanged || !fhirPatientDetail.FhirOrganizationId.HasValue)
            {
                fhirPatientDetail.FhirOrganizationId = await GetFhirOrganizationId(fhirPatientDetail.HospiceId);
            }

            await _fhirQueueService.QueueUpdateRequest(fhirPatientDetail);
            return fhirPatientDetail;
        }

        public async Task<PatientDetail> UpdateHms2PatientId(Guid patientUuid, int hms2Id)
        {
            var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientUuid);

            if (patientModel == null)
            {
                throw new ValidationException($"Patient with Id ({patientUuid}) not found");
            }
            if (patientModel.Hms2Id.HasValue && patientModel.Hms2Id.Value != hms2Id)
            {
                throw new ValidationException($"Patient ({patientModel.FirstName} {patientModel.LastName}) is already associated with hms2 patient");
            }

            patientModel.Hms2Id = hms2Id;
            await _patientsRepository.UpdateAsync(patientModel);
            return _mapper.Map<PatientDetail>(patientModel);
        }

        public async Task<PatientDetail> MergePatients(Guid patientUuid, MergePatientRequest mergePatientRequest, bool updateFhirPatients = false)
        {
            var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientUuid);
            if (patientModel == null)
            {
                throw new ValidationException($"No patient found with ({patientUuid}) Unique Id");
            }

            var duplicatePatientModel = await _patientsRepository.GetAsync(p => p.UniqueId == mergePatientRequest.DuplicatePatientUUID);
            if (duplicatePatientModel == null)
            {
                throw new ValidationException($"No duplicate patient found with ({mergePatientRequest.DuplicatePatientUUID}) Unique Id");
            }

            if (patientModel.HospiceId != duplicatePatientModel.HospiceId || patientModel.HospiceLocationId != duplicatePatientModel.HospiceLocationId)
            {
                throw new ValidationException($"Patient ({patientModel.FirstName} {patientModel.LastName}) and ({duplicatePatientModel.FirstName} {duplicatePatientModel.LastName}) belong to different hospice/hospice location");
            }

            if (updateFhirPatients)
            {
                if (!patientModel.FhirPatientId.HasValue || patientModel.FhirPatientId == Guid.Empty)
                {
                    throw new ValidationException($"Patient with ({patientModel.UniqueId}) Unique Id has not been created on FHIR server yet.");
                }

                if (!duplicatePatientModel.FhirPatientId.HasValue || duplicatePatientModel.FhirPatientId == Guid.Empty)
                {
                    throw new ValidationException($"Duplicated patient with ({duplicatePatientModel.UniqueId}) Unique Id has not been created on FHIR server yet.");
                }
            }

            patientModel.FirstName = mergePatientRequest.FirstName;
            patientModel.LastName = mergePatientRequest.LastName;
            patientModel.DateOfBirth = mergePatientRequest.DateOfBirth;
            patientModel.PatientHeight = (double)mergePatientRequest.PatientHeight;
            patientModel.PatientWeight = mergePatientRequest.PatientWeight;
            patientModel.IsInfectious = mergePatientRequest.IsInfectious;
            patientModel.Diagnosis = mergePatientRequest.Diagnosis;
            patientModel.PhoneNumbers = _mapper.Map<ICollection<PhoneNumbers>>(mergePatientRequest.PhoneNumbers);

            var duplicatePatientNotes = duplicatePatientModel.PatientNotes;
            foreach (var patientNote in duplicatePatientNotes)
            {
                patientNote.PatientId = patientModel.Id;
            }
            patientModel.PatientNotes.Concat(duplicatePatientNotes);

            var duplicatePatientAddresses = duplicatePatientModel.PatientAddress;
            foreach (var patientAddress in duplicatePatientAddresses)
            {
                patientAddress.PatientId = patientModel.Id;
            }
            patientModel.PatientAddress.Concat(duplicatePatientAddresses);

            await _patientsRepository.UpdateAsync(patientModel);

            await UpdateStatus(patientModel.StatusReason?.Name, mergePatientRequest.IsDMEEquipmentLeft, mergePatientRequest.HasOpenOrders, DateTime.UtcNow, patientModel, !string.IsNullOrEmpty(patientModel.LastOrderNumber));

            duplicatePatientModel.StatusId = (int)PatientEnums.PatientStatusTypes.Inactive;
            duplicatePatientModel.StatusReasonId = (int)PatientEnums.PatientStatusTypes.Duplicate;
            duplicatePatientModel.StatusChangedDate = DateTime.UtcNow;
            await _patientsRepository.UpdateAsync(duplicatePatientModel);

            var patientPayload = _mapper.Map<MergePatientBaseRequest>(mergePatientRequest);
            var patientMergeHistoryLog = new PatientMergeHistory()
            {
                PatientUuid = patientUuid,
                DuplicatePatientUuid = mergePatientRequest.DuplicatePatientUUID,
                ChangeLog = JsonConvert.SerializeObject(patientPayload)
            };

            await _patientMergeHistoryRepository.AddAsync(patientMergeHistoryLog);

            if (updateFhirPatients)
            {
                var fhirPatientsToUpdate = new List<FHIRPatientDetail>
                {
                    _mapper.Map<FHIRPatientDetail>(patientModel),
                    _mapper.Map<FHIRPatientDetail>(duplicatePatientModel)
                };

                try
                {
                    await _fhirQueueService.QueueUpdateRequestList(fhirPatientsToUpdate);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Exception occurred while sending FHIR patient request to service bus: {ex.Message}");
                }
            }

            return _mapper.Map<PatientDetail>(patientModel);
        }

        public async Task<PatientDetail> MovePatientToHospiceLocation(Guid patientUuid, int hospiceId, int hospiceLocationId, DateTime movementDate)
        {
            var oldPatientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientUuid);
            if (oldPatientModel == null)
            {
                throw new ValidationException($"No patient found with ({patientUuid}) Unique Id");
            }

            if(oldPatientModel.CreatedDateTime > movementDate)
            {
                throw new ValidationException($"Movement date cannot be before patient creation date");
            }

            var patientCreateRequest = _mapper.Map<PatientCreateRequest>(_mapper.Map<PatientDetail>(oldPatientModel));

            patientCreateRequest.HospiceId = hospiceId;
            patientCreateRequest.HospiceLocationId = hospiceLocationId;

            patientCreateRequest.Hms2Id = null;
            patientCreateRequest.DataBridgeRunUuid = null;
            patientCreateRequest.DataBridgeRunDateTime = null;
            patientCreateRequest.LastOrderDateTime = null;

            var newPatientModel = await CreatePatient(patientCreateRequest);

            await UpdateStatus(PatientEnums.PatientStatusTypes.Discharged.ToString(), false, false, DateTime.UtcNow, oldPatientModel, false);

            return _mapper.Map<PatientDetail>(newPatientModel);
        }

        public async Task<PaginatedList<PatientMergeHistoryResponse>> GetPatientMergeHistory(SieveModel sieveModel)
        {
            _patientMergeHistoryRepository.SieveModel = sieveModel;

            var totalRecordCount = await _patientMergeHistoryRepository.GetCountAsync(ph => true);
            var patientMergeHistoryModel = await _patientMergeHistoryRepository.GetAllAsync();

            var patientMergeHistory = _mapper.Map<IEnumerable<PatientMergeHistoryResponse>>(patientMergeHistoryModel);
            var userIds = patientMergeHistory.Where(pmh => pmh.MergedByUserId.HasValue).Select(pmh => pmh.MergedByUserId.Value);

            var users = await _usersRepository.GetManyAsync(u => userIds.Contains(u.Id));
            foreach (var patientMergeHistoryLog in patientMergeHistory)
            {
                var createdByUser = users.FirstOrDefault(u => patientMergeHistoryLog.MergedByUserId == u.Id);
                if (createdByUser != null)
                {
                    patientMergeHistoryLog.MergedByUserName = $"{createdByUser.FirstName} {createdByUser.LastName}";
                }
            }

            return _paginationService.GetPaginatedList(patientMergeHistory, totalRecordCount, sieveModel.Page, sieveModel.PageSize);
        }

    }
}
