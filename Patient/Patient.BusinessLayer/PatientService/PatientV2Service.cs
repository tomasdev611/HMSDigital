using System.Threading.Tasks;
using HMSDigital.Patient.ViewModels;
using HMSDigital.Patient.BusinessLayer.Interfaces;
using HMSDigital.Common.ViewModels;
using Sieve.Models;
using HMSDigital.Patient.BusinessLayer.Validations;
using System.ComponentModel.DataAnnotations;
using System;
using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using HMSDigital.Common.BusinessLayer.Enums;
using Microsoft.Extensions.Logging;
using HMSDigital.Patient.Data.Repositories.Interfaces;
using CommonEnums = HMSDigital.Common.BusinessLayer;
using HMSDigital.Patient.Data.Models;
using System.Linq;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Http;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Patient.BusinessLayer.Enums;
using PatientOrderRequest = HMSDigital.Patient.ViewModels.PatientOrderRequest;
using HMSDigital.Patient.FHIR.Models;

namespace HMSDigital.Patient.BusinessLayer
{
    public class PatientV2Service : IPatientV2Service
    {
        private readonly IMapper _mapper;

        private readonly IFHIRService _fHIRService;

        private readonly IPaginationService _paginationService;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly IFHIRQueueService<FHIRPatientDetail> _fhirQueueService;

        private readonly IPatientsRepository _patientsRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly ILogger<PatientV2Service> _logger;

        private readonly HttpContext _httpContext;

        public PatientV2Service(IFHIRService fHIRService,
            IPaginationService paginationService,
            IAddressStandardizerService addressStandardizerService,
            IFHIRQueueService<FHIRPatientDetail> fhirQueueService,
            IPatientsRepository patientsRepository,
            IUsersRepository usersRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PatientV2Service> logger)
        {
            _fHIRService = fHIRService;
            _paginationService = paginationService;
            _mapper = mapper;
            _addressStandardizerService = addressStandardizerService;
            _fhirQueueService = fhirQueueService;
            _patientsRepository = patientsRepository;
            _usersRepository = usersRepository;
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<PaginatedList<PatientDetail>> GetAllPatients(SieveModel sieveModel, bool ignoreFilter = false)
        {
            _patientsRepository.SieveModel = sieveModel;
            var patientPredicate = await GetPatientPredicate(ignoreFilter);
            var totalRecords = await _patientsRepository.GetCountAsync(patientPredicate);
            var patientModels = await _patientsRepository.GetManyAsync(patientPredicate);

            var integratedPatients = await GetIntegratedPatientsByHmsPatients(patientModels);

            return _paginationService.GetPaginatedList(integratedPatients, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PatientDetail> GetPatientById(int patientId)
        {
            var patientModel = await _patientsRepository.GetByIdAsync(patientId);

            if(!patientModel.FhirPatientId.HasValue || patientModel.FhirPatientId == Guid.Empty)
            {
                return _mapper.Map<PatientDetail>(patientModel);
            }

            var fhirPatient = await _fHIRService.GetPatientById(patientModel.FhirPatientId.ToString());
            return _mapper.Map(patientModel, fhirPatient);
        }

        public async Task<PaginatedList<PatientDetail>> SearchPatientsBySearchQuery(SieveModel sieveModel, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return null;
            }
            searchQuery = searchQuery.Trim();

            _patientsRepository.SieveModel = sieveModel;

            Guid patientUniqueId;
            if (Guid.TryParse(searchQuery, out patientUniqueId))
            {
                return await SearchPatientsByUniqueId(sieveModel, patientUniqueId);
            }

            DateTime patientDateOfBirth;
            if (DateTime.TryParse(searchQuery, out patientDateOfBirth))
            {
                var fhirPatients = await _fHIRService.SearchPatientsByBirthDate(patientDateOfBirth);
                return await GetIntegratedPatientsByFhirPatients(sieveModel, fhirPatients);
            }

            int patientYearOfBirth;
            if (int.TryParse(searchQuery, out patientYearOfBirth))
            {
                var fhirPatients = await _fHIRService.SearchPatientsByYearOfBirth(patientYearOfBirth);
                return await GetIntegratedPatientsByFhirPatients(sieveModel, fhirPatients);
            }

            return await SearchPatientsByName(sieveModel, searchQuery);
        }

        public async Task RecordPatientOrder(PatientOrderRequest patientOrderRequest)
        {
            await _fHIRService.RecordPatientOrder(patientOrderRequest);
        }

        public async Task<PatientDetail> UpdatePatientFhirId(Guid patientUuid, Guid patientFhirId)
        {
            try
            {
                var patientModel = await _patientsRepository.GetAsync(p => p.UniqueId == patientUuid);

                if (patientModel == null)
                {
                    throw new ValidationException($"Patient with unique Id {patientUuid} not found");
                }

                patientModel.FhirPatientId = patientFhirId;

                await _patientsRepository.UpdateAsync(patientModel);
                return _mapper.Map<PatientDetail>(patientModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while updating patient FHIR id with unique Id({patientUuid}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while updating patient FHIR id with unique Id({patientUuid}): {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<PatientDetail>> SearchPatients(PatientSearchRequest patientSearchRequest)
        {
            var searchType = GetSerachType(patientSearchRequest);

            if (searchType == PatientSearchType.HmsBasedSearch)
            {
                var patientModels = await SearchHmsPatients(patientSearchRequest);
                return await GetIntegratedPatientsByHmsPatients(patientModels);
            }

            var fhirPatients = await _fHIRService.SearchPatients(patientSearchRequest);

            if (searchType == PatientSearchType.FhirBasedSearch)
            {
                return await GetIntegratedPatientsByFhirPatients(fhirPatients);
            }

            var mixedSearchPatientModels = await SearchHmsPatients(patientSearchRequest, fhirPatients);
            return GetConbinedPatients(mixedSearchPatientModels, fhirPatients);
        }

        private PatientSearchType GetSerachType(PatientSearchRequest patientSearchRequest) 
        {
            var isHmsSearch = !string.IsNullOrEmpty(patientSearchRequest.FirstName) 
                                || !string.IsNullOrEmpty(patientSearchRequest.FirstName) 
                                || patientSearchRequest.HospiceId.HasValue;

            var isFhirSearch = patientSearchRequest.DateOfBirth.HasValue
                                || (patientSearchRequest.Address != null && (
                                        !string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine1)
                                        || !string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine2)
                                        || !string.IsNullOrEmpty(patientSearchRequest.Address.AddressLine3)
                                        || !string.IsNullOrEmpty(patientSearchRequest.Address.State)
                                        || !string.IsNullOrEmpty(patientSearchRequest.Address.City)
                                        || patientSearchRequest.Address.ZipCode != 0));

            if (isHmsSearch)
            {
                if(isFhirSearch)
                {
                    return PatientSearchType.MixedSearch;
                }
                return PatientSearchType.HmsBasedSearch;
            }
            return PatientSearchType.FhirBasedSearch;
        }

        private async Task<IEnumerable<PatientDetails>> SearchHmsPatients(PatientSearchRequest patientSearchRequest, IEnumerable<FHIRPatientDetail> fhirPatients = null) 
        {
            var predicate = PredicateBuilder.New<PatientDetails>(true);
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
            predicateHospice.And(predicate);

            if (fhirPatients != null) 
            {
                var fhirPatientIds = fhirPatients.Select(x => x.FhirPatientId).ToList();
                predicateHospice.And(d => fhirPatientIds.Contains(d.FhirPatientId.Value));
            }

            try
            {
                var patientModels = await _patientsRepository.GetManyAsync(predicateHospice);
                return await GetAccessiblePatients(patientModels);
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

        private async Task<PatientDetail> ValidateFhirPatient(PatientCreateRequest patientCreateRequest) 
        {
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
                    if (!Enum.IsDefined(typeof(PhoneNumberType), phoneNumber.NumberTypeId))
                    {
                        throw new ValidationException($"Request patient phone number type {phoneNumber.NumberTypeId} is not valid");
                    }
                }
            }

            var patientDetail = _mapper.Map<PatientDetail>(patientCreateRequest);
            foreach (var patientAddress in patientDetail.PatientAddress)
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
                if (patientAddress.Address.AddressUuid == Guid.Empty)
                {
                    patientAddress.Address.AddressUuid = Guid.NewGuid();
                }
            }

            return patientDetail;
        }

        private async Task<PatientDetails> CreateHmsPatient(PatientCreateRequest patientCreateRequest)
        {
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
                    if (!Enum.IsDefined(typeof(PhoneNumberType), phoneNumber.NumberTypeId))
                    {
                        throw new ValidationException($"Request patient phone number type {phoneNumber.NumberTypeId} is not valid");
                    }
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
                    if (patientAddressModel.Address.AddressUuid == Guid.Empty)
                    {
                        patientAddressModel.Address.AddressUuid = Guid.NewGuid();
                    }
                    patientAddressModels.Add(patientAddressModel);
                }
                patientModel.PatientAddress = patientAddressModels;
                patientModel.UniqueId = Guid.NewGuid();
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

        private async Task<PatientDetail> PatchHmsPatient(JsonPatchDocument<PatientDetail> patientPatchDocument, PatientDetails patientModel) 
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
                if (patientAddress.Address.AddressUuid == Guid.Empty)
                {
                    patientAddress.Address.AddressUuid = Guid.NewGuid();
                }
            }

            await _patientsRepository.UpdateAsync(patientModel);
            return _mapper.Map<PatientDetail>(patientModel);
        }

        private async Task<PaginatedList<PatientDetail>> SearchPatientsByUniqueId(SieveModel sieveModel, Guid uniqueId)
        {
            var totalCountById = await _patientsRepository.GetCountAsync(p =>
                p.UniqueId == uniqueId
            );
            var patientModelsById = await _patientsRepository.GetManyAsync(p =>
                p.UniqueId == uniqueId
            );

            var accessiblePatientModelsById = await GetAccessiblePatients(patientModelsById);
            var integratedPatients = await GetIntegratedPatientsByHmsPatients(accessiblePatientModelsById);

            return _paginationService.GetPaginatedList(integratedPatients, totalCountById, sieveModel.Page, sieveModel.PageSize);
        }

        private async Task<IEnumerable<PatientDetail>> GetIntegratedPatientsByHmsPatients(IEnumerable<PatientDetails> patientModels) 
        {
            var fhirIds = patientModels.Where(x => x.FhirPatientId.HasValue && x.FhirPatientId != Guid.Empty).Select(x => x.FhirPatientId.Value).Distinct();
            var fhirPatients = await _fHIRService.GetPatientsByIds(fhirIds);

            return GetConbinedPatients(patientModels, fhirPatients);
        }

        private async Task<IEnumerable<PatientDetail>> GetIntegratedPatientsByFhirPatients(IEnumerable<FHIRPatientDetail> fhirPatients)
        {
            var fhirIds = fhirPatients.Select(x => x.FhirPatientId).ToList();
            var patientModels = await _patientsRepository.GetManyAsync(x => x.FhirPatientId.HasValue 
                                                                         && x.FhirPatientId != Guid.Empty 
                                                                         && fhirIds.Contains(x.FhirPatientId.Value));

            return GetConbinedPatients(patientModels, fhirPatients);
        }

        private async Task<IEnumerable<PatientDetails>> GetAccessiblePatients(IEnumerable<PatientDetails> patientModels, bool ignoreFilter = false)
        {
            return patientModels.Where(await GetPatientPredicate(ignoreFilter));
        }

        private async Task<ExpressionStarter<PatientDetails>> GetPatientPredicate(bool ignoreFilter = false)
        {
            var patientPredicate = PredicateBuilder.New<PatientDetails>(false);
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

        private int GetLoggedInUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return -1;
            }
            return int.Parse(userIdClaim.Value);
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

                var accessiblePatientModels = await GetAccessiblePatients(patientModels);
                var integratedPatients = await GetIntegratedPatientsByHmsPatients(accessiblePatientModels);

                return _paginationService.GetPaginatedList(integratedPatients, totalCount, sieveModel.Page, sieveModel.PageSize);
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

            var accessiblePatientModelsByName = await GetAccessiblePatients(patientModelsByName);
            var integratedPatientsByName = await GetIntegratedPatientsByHmsPatients(accessiblePatientModelsByName);

            return _paginationService.GetPaginatedList(integratedPatientsByName, totalCountByName, sieveModel.Page, sieveModel.PageSize);
        }

        private async Task<PaginatedList<PatientDetail>> GetIntegratedPatientsByFhirPatients(SieveModel sieveModel, IEnumerable<FHIRPatientDetail> fhirPatients) 
        {
            var fhirPatientIds = fhirPatients.Select(x => x.FhirPatientId).ToList();

            var patientModels = await _patientsRepository.GetManyAsync(x => x.FhirPatientId.HasValue 
                                                                            && x.FhirPatientId != Guid.Empty 
                                                                            && fhirPatientIds.Contains(x.FhirPatientId.Value));

            var totalRecords = await _patientsRepository.GetCountAsync(x => x.FhirPatientId.HasValue 
                                                                            && x.FhirPatientId != Guid.Empty 
                                                                            && fhirPatientIds.Contains(x.FhirPatientId.Value));

            var accessiblePatientModels = await GetAccessiblePatients(patientModels);
            var integratedPatients = GetConbinedPatients(accessiblePatientModels, fhirPatients);

            return _paginationService.GetPaginatedList(integratedPatients, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        private IEnumerable<PatientDetail> GetConbinedPatients(IEnumerable<PatientDetails> patientModels, IEnumerable<FHIRPatientDetail> fhirPatients)
        {
            List<PatientDetail> integratedPatients = new List<PatientDetail>();

            if (fhirPatients == null || !fhirPatients.Any())
            {
                integratedPatients = _mapper.Map<List<PatientDetail>>(patientModels.ToList());
            }
            else
            {
                foreach (var patientModel in patientModels)
                {
                    if (!patientModel.FhirPatientId.HasValue || patientModel.FhirPatientId == Guid.Empty)
                    {
                        integratedPatients.Add(_mapper.Map<PatientDetail>(patientModel));
                    }
                    else
                    {
                        var fhirPatient = fhirPatients.FirstOrDefault(x => x.FhirPatientId == patientModel.FhirPatientId);
                        if (fhirPatient != null)
                        {
                            integratedPatients.Add(_mapper.Map(patientModel, fhirPatient));
                        }
                        else
                        {
                            integratedPatients.Add(_mapper.Map<PatientDetail>(patientModel));
                        }
                    }
                }
            }

            return integratedPatients;
        }
    }
}
