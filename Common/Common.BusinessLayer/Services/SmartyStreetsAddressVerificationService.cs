using AutoMapper;
using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.BusinessLayer.Validations;
using HMSDigital.Common.ViewModels;
using Microsoft.Extensions.Options;
using RestEase;
using SmartyStreets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lookup = SmartyStreets.USStreetApi.Lookup;

namespace HMSDigital.Common.BusinessLayer.Services
{
    public class SmartyStreetsAddressVerificationService : IAddressStandardizerService
    {
        private readonly IMapper _mapper;

        private readonly SmartyStreetsConfig _smartyStreetsOptions;

        public SmartyStreetsAddressVerificationService(IOptions<SmartyStreetsConfig> smartyStreetsOptions, IMapper mapper)
        {
            _smartyStreetsOptions = smartyStreetsOptions.Value;
            _mapper = mapper;
        }

        /// <summary>
        ///  Calls SmartyStreets API to get a verify address
        /// </summary>
        /// <param name="address">Use to get address verify.</param>
        /// <returns name="isValid">boolean value, indicate that address is verified for SmartyStreets and it's true when is completely or partially verify.</returns>
        /// <returns name="isVerified">boolean value, indicate that address is verified for SmartyStreets and it's true only when is completely verifiy.</returns>
        public async Task<Address> GetStandardizedAddress(Address address)
        {
            try
            {
                var validator = new AddressValidator();
                var result = await validator.ValidateAsync(address);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors[0].ErrorMessage);
                }

                if (address.Country == null)
                {
                    address.Country = _smartyStreetsOptions.SmartyCountryAddress;
                }

                var client = new ClientBuilder(_smartyStreetsOptions.SmartyAuthId, _smartyStreetsOptions.SmartyAuthToken)
                    .BuildUsStreetApiClient();

                var lookup = new Lookup
                {
                    Street = address.AddressLine1,
                    Street2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    ZipCode = address.ZipCode.ToString(),
                    MaxCandidates = _smartyStreetsOptions.SmartyMaxCandidates,
                    MatchStrategy = Lookup.INVALID
                };
                client.Send(lookup);

                if (!lookup.Result.Any())
                {
                    address.IsVerified = false;
                    return address;
                }

                var standardizedAddress = lookup.Result.First();
                var analysisResult = lookup.Result.First().Analysis;

                address = _mapper.Map(standardizedAddress, address);

                // Y — Confirmed; entire address is present in the USPS data. 
                // A1 — Address is invalid.
                address.IsValid = !analysisResult.DpvFootnotes.ToUpper().Contains("A1") && analysisResult.DpvMatchCode.ToUpper().Equals("Y");

                address.IsVerified = true;
                address.VerifiedBy = "SmartyStreets";

                address.Results = address.IsValid ? "Valid Address" : "Invalid Address";
                return address;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Address verification failed: " + ex.Message);
            }
        }

        /// <summary>
        ///  Calls SmartyStreets API to get address suggestions
        /// </summary>
        /// <param name="address">Use to get address suggestions.</param>
        /// <param name="suite">Indicate suite to address, optional and nullable.</param>
        /// <param name="maxRecords">Is the max number records that suggests, optional and default value is 10.</param>
        public async Task<List<SuggestionResponse>> GetAddressSuggestions(Address address, string suite, string maxRecords)
        {
            try
            {
                var validator = new AddressValidator();
                var result = await validator.ValidateAsync(address);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.Errors[0].ErrorMessage);
                }

                if (address.Country == null)
                {
                    address.Country = _smartyStreetsOptions.SmartyCountryAddress;
                }

                var smartyStreetsApi = RestClient.For<ISmartyStreetsApi>(_smartyStreetsOptions.SmartyProURL);
                smartyStreetsApi.Referer = _smartyStreetsOptions.SmartyRefer;
                var response = await smartyStreetsApi.GetSuggestionsAsync(_smartyStreetsOptions.SmartySecretKey, address.AddressLine1, address.City, address.State);
                var addresses = _mapper.Map<List<SuggestionResponse>>(response.Suggestions);

                return addresses;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Address suggestions failed: " + ex.Message);
            }
        }
    }
}
