using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Formatter;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IMapper _mapper;

        private readonly IFacilityPatientRepository _facilityPatientRepository;

        private readonly IFacilityPatientHistoryRepository _facilityPatientHistoryRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly IPaginationService _paginationService;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly IFileService _fileService;

        private readonly ICsvMappingRepository _csvMappingRepository;

        private readonly ILogger<FacilityService> _logger;

        public FacilityService(IMapper mapper,
            IHospiceRepository hospiceRepository,
            IFacilityRepository facilityRepository,
            IFacilityPatientRepository facilityPatientRepository,
            IFacilityPatientHistoryRepository facilityPatientHistoryRepository,
            ISitesRepository sitesRepository,
            IPaginationService paginationService,
            IHospiceLocationRepository hospiceLocationRepository,
            IAddressStandardizerService addressStandardizerService,
            IFileService fileService,
            ICsvMappingRepository csvMappingRepository,
            ILogger<FacilityService> logger)
        {
            _mapper = mapper;
            _facilityRepository = facilityRepository;
            _hospiceRepository = hospiceRepository;
            _facilityPatientRepository = facilityPatientRepository;
            _facilityPatientHistoryRepository = facilityPatientHistoryRepository;
            _sitesRepository = sitesRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _paginationService = paginationService;
            _addressStandardizerService = addressStandardizerService;
            _fileService = fileService;
            _csvMappingRepository = csvMappingRepository;
            _logger = logger;
        }

        public async Task<PaginatedList<Facility>> GetAllFacilities(int hospiceId, SieveModel sieveModel)
        {
            _facilityRepository.SieveModel = sieveModel;
            var predicate = PredicateBuilder.New<Facilities>(true);
            if (hospiceId != 0)
            {
                predicate.And(f => f.HospiceId == hospiceId);
            }
            var totalRecords = await _facilityRepository.GetCountAsync(predicate);
            var facilityModels = await _facilityRepository.GetManyAsync(predicate);
            var facilities = _mapper.Map<IEnumerable<Facility>>(facilityModels);
            return _paginationService.GetPaginatedList(facilities, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }
        public async Task<Facility> GetFacilityById(int id)
        {
            var facility = await _facilityRepository.GetByIdAsync(id);
            return _mapper.Map<Facility>(facility);
        }
        public async Task<IEnumerable<Facility>> SearchFacilities(string searchQuery)
        {
            var facilities = await _facilityRepository.GetManyAsync(f => f.Name.Contains(searchQuery));
            return _mapper.Map<IEnumerable<Facility>>(facilities);
        }

        public async Task<Facility> CreateFacility(FacilityRequest facilityRequest)
        {
            var facilityValidator = new FacilityValidator();
            var validationResult = facilityValidator.Validate(facilityRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var hospice = await _hospiceRepository.GetByIdAsync(facilityRequest.HospiceId);
            if (hospice == null)
            {
                throw new ValidationException($"Invalid Hospice Id {facilityRequest.HospiceId}");
            }
            await ValidateSite(facilityRequest.SiteId);
            await ValidateHospiceLocation(facilityRequest.HospiceLocationId);

            var facilityModel = _mapper.Map<Facilities>(facilityRequest);
            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(facilityRequest.Address));
                if (standardizedAddress != null)
                {
                    facilityModel.Address = _mapper.Map<Addresses>(standardizedAddress);
                }
            }
            catch (ValidationException vx)
            {
                _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
            }
            if (facilityModel.Address.AddressUuid == Guid.Empty)
            {
                facilityModel.Address.AddressUuid = Guid.NewGuid();
            }
            await _facilityRepository.AddAsync(facilityModel);
            return _mapper.Map<Facility>(facilityModel);
        }

        public async Task<(IEnumerable<Facility>, IEnumerable<FacilityCsvRequest>, ValidatedList<FacilityCsvRequest>)> CreateFacilitiesFromCsvFile(
           int hospiceId,
           IFormFile facilities,
           bool validateOnly,
           bool parseOnly)
        {
            var facilitiesMapping = await _csvMappingRepository.GetAsync(cm =>
                                                    cm.MappedTable == MappedItemTypes.Facility.ToString() &&
                                                    cm.HospiceId == hospiceId &&
                                                    cm.MappingType == CsvMappingTypes.Input.ToString());
            CsvHeaderMap<FacilityCsvRequest> csvHeaderMap = null;
            if (facilitiesMapping != null)
            {
                var mapping = JsonConvert.DeserializeObject<CsvMapping<InputMappedItem>>(facilitiesMapping.MappingInJson);
                csvHeaderMap = new CsvHeaderMap<FacilityCsvRequest>(mapping);
            }
            else
            {
                csvHeaderMap = new CsvHeaderMap<FacilityCsvRequest>(CsvMapping.GetInputCsvMapping(MappedItemTypes.Facility));
            }
            var facilitiesCsvRequest = _fileService.GetMappedRecords(facilities, csvHeaderMap);
            if (parseOnly)
            {
                return (null, facilitiesCsvRequest, null);
            }

            var validator = new ListValidator<FacilityBulkUploadValidator, FacilityCsvRequest>();
            var validatedFacilityList = validator.Validate(facilitiesCsvRequest);

            if (!validatedFacilityList.IsValid)
            {
                return (null, null, validatedFacilityList);
            }

            var hospiceLocationsFromCsv = facilitiesCsvRequest.Select(f => f.HospiceLocationName).Distinct().ToList();
            var hospiceLocations = await _hospiceLocationRepository.GetManyAsync(l => hospiceLocationsFromCsv.Contains(l.Name));
            if (hospiceLocationsFromCsv.Count() != hospiceLocations.Count())
            {
                var invalidLocation = hospiceLocationsFromCsv.Except(hospiceLocations.Select(i => i.Name));
                var ErrorMessage = "Invalid Hospice Location";
                var invalidFacilitiesName = facilitiesCsvRequest.Where(u => invalidLocation.Contains(u.HospiceLocationName)).Select(f => f.Name);
                validatedFacilityList = validator.GetFormatedList(facilitiesCsvRequest, ErrorMessage, invalidFacilitiesName, f => f.Name);
                return (null, null, validatedFacilityList);
            }

            if (validateOnly)
            {
                return (null, null, validatedFacilityList);
            }
            var facilitiesRequest = _mapper.Map<IEnumerable<FacilityCsvDTO>>(facilitiesCsvRequest);
            foreach (var facilityRequest in facilitiesRequest)
            {
                var hospiceLocation = hospiceLocations.FirstOrDefault(l => l.Name.ToLower() == facilityRequest.HospiceLocationName.ToLower());
                facilityRequest.HospiceLocationId = hospiceLocation.Id;
                facilityRequest.HospiceId = hospiceId;
                facilityRequest.FacilityPhoneNumber = new List<ViewModels.FacilityPhoneNumber>()
                    {
                        new ViewModels.FacilityPhoneNumber
                        {
                            PhoneNumber = new PhoneNumberRequest()
                            {
                                CountryCode = 1,
                                Number = facilityRequest.PhoneNumber ?? 0,
                                IsPrimary = true,
                                IsVerified = true
                            }
                        }
                    };
                try
                {
                    facilityRequest.Address = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(facilityRequest));
                }
                catch (ValidationException vx)
                {
                    facilityRequest.Address = _mapper.Map<Address>(facilityRequest);
                    _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                }
                if (facilityRequest.Address.AddressUuid == Guid.Empty)
                {
                    facilityRequest.Address.AddressUuid = Guid.NewGuid();
                }
            }
            var facilitiesModel = _mapper.Map<IEnumerable<Facilities>>(facilitiesRequest);
            facilitiesModel = await _facilityRepository.AddManyAsync(facilitiesModel);

            return (_mapper.Map<IEnumerable<Facility>>(facilitiesModel), null, null);
        }

        public async Task<Facility> PatchFacility(int facilityId, JsonPatchDocument<Facility> facilityPatchDocument)
        {
            var facilityModel = await _facilityRepository.GetByIdAsync(facilityId);
            if (facilityModel == null)
            {
                throw new ValidationException($"Facility with Id ({facilityId}) not found");
            }
            var allowedPaths = new List<string> {
                "^/name",
                "^/isDisable",
                "^/siteId",
                "^/hospiceLocationId",
                "^/address/addressLine1",
                "^/address/addressLine2",
                "^/address/addressLine3",
                "^/address/city",
                "^/address/state",
                "^/address/zipCode",
                "^/address/county",
                "^/address/plus4Code",
                "^/facilityPhoneNumber/\\d/phoneNumber/number"
            };

            string pattern = string.Join("|", allowedPaths);
            foreach (var op in facilityPatchDocument.Operations)
            {
                if (!new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(op.path))
                {
                    throw new ValidationException($"Attempt to modify data outside of user control. Logged and reported.");
                }
            }
            var modelPatch = _mapper.Map<JsonPatchDocument<Facilities>>(facilityPatchDocument);
            modelPatch.ApplyTo(facilityModel);

            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(facilityModel.Address));
                if (standardizedAddress != null)
                {
                    _mapper.Map<Address, Addresses>(standardizedAddress, facilityModel.Address);
                }
            }
            catch (ValidationException vx)
            {
                _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
            }
            if (facilityModel.Address.AddressUuid == Guid.Empty)
            {
                facilityModel.Address.AddressUuid = Guid.NewGuid();
            }

            var facilityViewModel = _mapper.Map<FacilityRequest>(facilityModel);
            var facilityValidator = new FacilityValidator();
            var validationResult = facilityValidator.Validate(facilityViewModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }
            await ValidateSite(facilityModel.SiteId);
            await ValidateHospiceLocation(facilityModel.HospiceLocationId);
            await _facilityRepository.UpdateAsync(facilityModel);

            return _mapper.Map<Facility>(facilityModel);
        }

        public async Task<IEnumerable<FacilityPatientResponse>> AssignPatientToFacilities(FacilityPatientRequest facilityPatientRequest)
        {
            var facilityIds = facilityPatientRequest.FacilityPatientRooms.Select(fpr => fpr.FacilityId);
            var facilityModels = await _facilityRepository.GetManyAsync(f => facilityIds.Contains(f.Id));
            if (facilityModels.Count() != facilityIds.Count())
            {
                var invalidFacilityIds = facilityIds.Except(facilityModels.Select(f => f.Id));
                throw new ValidationException($"Facilities with Ids ({string.Join(", ", invalidFacilityIds)}) not found");
            }

            await _facilityPatientRepository.DeleteAsync(fp => fp.PatientUuid == facilityPatientRequest.PatientUuid);

            var patientFacilities = facilityPatientRequest.FacilityPatientRooms.Select(fpr =>
                                                                     new FacilityPatient()
                                                                     {
                                                                         FacilityId = fpr.FacilityId,
                                                                         PatientUuid = facilityPatientRequest.PatientUuid,
                                                                         PatientRoomNumber = fpr.PatientRoomNumber
                                                                     });

            var patient = await _facilityPatientRepository.AddManyAsync(patientFacilities);


            await AddFacilityPatientHistory(facilityPatientRequest.PatientUuid, facilityIds);

            return _mapper.Map<IEnumerable<FacilityPatientResponse>>(patientFacilities);
        }

        public async Task<PaginatedList<FacilityPatientResponse>> GetAllAssignedPatients(int hospiceId, SieveModel sieveModel)
        {
            _facilityPatientRepository.SieveModel = sieveModel;
            var totalRecords = await _facilityPatientRepository.GetCountAsync(fp => fp.Facility.HospiceId == hospiceId);
            var facilityPatientModels = await _facilityPatientRepository.GetManyAsync(fp => fp.Facility.HospiceId == hospiceId);
            var facilityPatients = _mapper.Map<IEnumerable<FacilityPatientResponse>>(facilityPatientModels);
            return _paginationService.GetPaginatedList(facilityPatients, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<IEnumerable<FacilityPatientResponse>> GetAssignedPatients(int facilityId)
        {
            var facilityPatients = await _facilityPatientRepository.GetManyAsync(fp => fp.FacilityId == facilityId);
            return _mapper.Map<IEnumerable<FacilityPatientResponse>>(facilityPatients);
        }

        public async Task<IEnumerable<FacilityPatientSearchResponse>> SearchAssignedPatients(int facilityId, SieveModel sieveModel)
        {
            _facilityPatientHistoryRepository.SieveModel = sieveModel;
            var facilityPatients = await _facilityPatientHistoryRepository.GetManyAsync(fp => fp.FacilityId == facilityId);
            return _mapper.Map<IEnumerable<FacilityPatientSearchResponse>>(facilityPatients);
        }

        private async Task AddFacilityPatientHistory(Guid patientUuid, IEnumerable<int> facilityIds)
        {
            var activePatientFacilities = await _facilityPatientHistoryRepository.GetManyAsync(h => h.PatientUuid == patientUuid && h.Active);
            if (activePatientFacilities.Count() == 0)
            {
                activePatientFacilities = facilityIds.Select(fId =>
                                                    new FacilityPatientHistory()
                                                    {
                                                        FacilityId = fId,
                                                        PatientUuid = patientUuid,
                                                        Active = true
                                                    });
                await _facilityPatientHistoryRepository.AddManyAsync(activePatientFacilities);
            }
            else
            {
                var inactivePatientFacilities = activePatientFacilities.Where(apf => !facilityIds.Contains(apf.FacilityId ?? 0));
                foreach (var patientFacility in inactivePatientFacilities)
                {
                    patientFacility.Active = false;
                }
                await _facilityPatientHistoryRepository.UpdateManyAsync(inactivePatientFacilities);

                var newPatientFacilities = facilityIds.Where(fId => !activePatientFacilities.Select(apf => apf.FacilityId).Contains(fId))
                                                    .Select(fId =>
                                                            new FacilityPatientHistory()
                                                            {
                                                                FacilityId = fId,
                                                                PatientUuid = patientUuid,
                                                                Active = true
                                                            });
                await _facilityPatientHistoryRepository.AddManyAsync(newPatientFacilities);
            }

        }

        private async Task<bool> ValidateSite(int? siteId)
        {
            if (siteId != null)
            {
                var site = await _sitesRepository.GetByIdAsync(siteId ?? 0);
                if (site == null)
                {
                    throw new ValidationException($"Site Id ({siteId}) not valid");
                }
            }
            return true;
        }

        private async Task<bool> ValidateHospiceLocation(int? hospiceLocationId)
        {
            if (hospiceLocationId != null)
            {
                var hospiceLocation = await _hospiceLocationRepository.GetByIdAsync(hospiceLocationId ?? 0);
                if (hospiceLocation == null)
                {
                    throw new ValidationException($"hospice location Id ({hospiceLocationId}) not valid");
                }
            }
            return true;
        }
    }
}
