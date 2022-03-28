using AutoMapper;
using HMSDigital.Common.BusinessLayer.API;
using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Common.BusinessLayer.Enums;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Common.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestEase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services
{
    public class MelissaService : IAddressStandardizerService
    {
        private readonly IMapper _mapper;

        private readonly ILogger<MelissaService> _logger;

        private readonly MelissaConfig _melissaConfig;

        public MelissaService(IOptions<MelissaConfig> melissaOptions, IMapper mapper, ILogger<MelissaService> logger)
        {
            _melissaConfig = melissaOptions.Value;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        ///  Calls Melissa API to get a verify address
        /// </summary>
        /// <param name="address">Use to get address verify.</param>
        /// <returns name="isVerified">boolean value, indicate that address is verified for Melissa and it's true when is completely or partially verify.</returns>
        /// <returns name="isValid">boolean value, indicate that address is verified for Melissa and it's true only when is completely verifiy.</returns>
        public async Task<Address> GetStandardizedAddress(Address address)
        {
            var validator = new AddressValidator();
            var result = validator.Validate(address);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }
            if (address.Country == null)
            {
                address.Country = "United States";
            }
            var addressParams = new SortedDictionary<string, string>()
            {
                {"a1", address.AddressLine1 },
                {"a2", address.AddressLine2 },
                {"a3", address.AddressLine3 },
                {"city", address.City },
                {"subadmarea",address.County },
                {"state", address.State },
                {"ctry", address.Country },
                {"postal", address.ZipCode == 0 ? null : address.ZipCode.ToString() },
                {"cols","Latitude,Longitude,CountryName,Plus4,CountyName"},
                {"act", "Verify,Check,Append" },
                {"format", "JSON" }
            };
            var melissaPersonatorAPI = GetPersonatorApiClient();
            var addressResponse = await melissaPersonatorAPI.DoContactVerify(addressParams);
            var transmissionResults = addressResponse.TransmissionResults;
            if (Enum.TryParse(transmissionResults, true, out VerifyAddressTransmissionError transmissionCode))
            {
                _logger.LogWarning($"Address verification failed due to ({transmissionCode}) transmission code");
                throw new ValidationException("Address verification failed due to ({transmissionCode}) transmission code");
            }

            if (addressResponse.Records == null)
            {
                return address;
            }
            var standardizedAddress = addressResponse.Records.FirstOrDefault();
            address = _mapper.Map(standardizedAddress, address);
            switch (address.Results)
            {
                case string results when results.Contains(Constants.VerifyAddresses.RESULT_AS01):
                    address.Results = Constants.VerifyAddresses.ADDRESS_VERIFIED;
                    address.IsVerified = true;
                    address.IsValid = true;
                    break;

                case string results when results.Contains(Constants.VerifyAddresses.RESULT_AS02):
                    address.Results = Constants.VerifyAddresses.ADDRESS_VERIFIED;
                    address.IsVerified = true;
                    address.IsValid = false;
                    break;

                default:
                    address.Results = Constants.VerifyAddresses.ADDRESS_NOT_VERIFIED;
                    address.IsVerified = false;
                    address.IsValid = false;
                    break;
            }
            return address;
        }

        /// <summary>
        ///  Calls Melissa API to get a suggestions address
        /// </summary>
        /// <param name="address">Use to get address sugestions.</param>
        /// <param name="suite">Indicate suite to address, optional and nullable.</param>
        /// <param name="maxRecords">Is the max number records that suggests, optional and default value is 10.</param>
        public async Task<List<SuggestionResponse>> GetAddressSuggestions(Address address, string suite, string maxRecords)
        {
            var validator = new AddressValidator();
            var result = validator.Validate(address);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors[0].ErrorMessage);
            }
            if (address.Country == null)
            {
                address.Country = "United States";
            }
            var addressParams = new SortedDictionary<string, string>()
            {
                {"address1", address.AddressLine1 },
                {"address2", address.AddressLine2 },
                {"address3", address.AddressLine3 },
                {"locality", address.City },
                {"administrativearea", address.State },
                {"country", address.Country },
                {"postalcode", address.ZipCode == 0 ? null : address.ZipCode.ToString() },
                {"maxrecords", string.IsNullOrWhiteSpace(maxRecords) ? "10" : maxRecords },
                {"format", "JSON" }
            };
            var melissaExpressEntryAPI = GetExpressEntryApiClient();
            var addressResponse = await melissaExpressEntryAPI.GlobalExpressAddress(addressParams);
            var addresses = _mapper.Map<List<SuggestionResponse>>(addressResponse.Records);
            switch (addressResponse.TransmissionResults)
            {
                case string results when results.Contains(Constants.VerifyAddresses.RESULT_GE04):
                    _logger.LogWarning($"Error Occurred while verify address ({Constants.VerifyAddresses.RESULT_GE04_DESCRIPTION})");
                    break;
                case string results when results.Contains(Constants.VerifyAddresses.RESULT_GE05):
                    _logger.LogWarning($"Error Occurred while verify address ({Constants.VerifyAddresses.RESULT_GE05_DESCRIPTION})");
                    break;
                case string results when results.Contains(Constants.VerifyAddresses.RESULT_GE06):
                    _logger.LogWarning($"Error Occurred while verify address ({Constants.VerifyAddresses.RESULT_GE06_DESCRIPTION})");
                    break;
            }
            return addresses;
        }

        private IMelissaPersonatorAPI GetPersonatorApiClient()
        {
            var melissaPersonatorAPI = RestClient.For<IMelissaPersonatorAPI>(_melissaConfig.PersonatorApiBaseUrl);
            melissaPersonatorAPI.LicenseKey = _melissaConfig.LicenseKey;
            return melissaPersonatorAPI;
        }

        private IMelissaExpressEntryAPI GetExpressEntryApiClient()
        {
            var melissaExpressEntryAPI = RestClient.For<IMelissaExpressEntryAPI>(_melissaConfig.ExpressEntryApiBaseUrl);
            melissaExpressEntryAPI.LicenseKey = _melissaConfig.LicenseKey;
            return melissaExpressEntryAPI;
        }
    }
}
