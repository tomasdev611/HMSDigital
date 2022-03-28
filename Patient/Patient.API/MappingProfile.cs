using AutoMapper;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;
using PatientModels = HMSDigital.Patient.Data.Models;
using HMSDigital.Common.BusinessLayer.Enums;
using Patient.ViewModels.NetSuite;
using FhirModels = Hl7.Fhir.Model;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using SmartyStreets.USStreetApi;
using HMSDigital.Patient.FHIR.BusinessLayer.Constants;
using HMSDigital.Patient.FHIR.Models;
using HMSDigital.Patient.Data.Enums;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using Newtonsoft.Json;

namespace HMSDigital.Patient.API
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PatientCreateRequest, PatientModels.PatientDetails>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Date));

            CreateMap<PatientModels.PatientDetails, PatientDetail>()
                .Include<PatientModels.PatientDetails, FHIRPatientDetail>()
                .ForMember(dest => dest.Status, opt =>
                 {
                     opt.PreCondition(src => src.StatusId != null);
                     opt.MapFrom(src => (PatientStatusTypes)src.StatusId);
                 })
                 .ForMember(dest => dest.StatusId, opt =>
                 {
                     opt.PreCondition(src => src.StatusId != null);
                     opt.MapFrom(src => (PatientStatusTypes)src.StatusId);
                 })
                 .ForMember(dest => dest.StatusReason, opt =>
                 {
                     opt.PreCondition(src => src.StatusReasonId != null);
                     opt.MapFrom(src => (PatientStatusTypes)src.StatusReasonId);
                 })
                .ForMember(dest => dest.StatusColor, opt =>
                 {
                     opt.PreCondition(src => src.StatusId != null);
                     opt.MapFrom(src => src.Status.Color);
                 })
                 .ForMember(dest => dest.StatusInfo, opt =>
                {
                    opt.PreCondition(src => src.StatusId.HasValue || src.StatusReasonId.HasValue);
                    opt.MapFrom(src => GetStatusMessage(src));
                });

            CreateMap<PatientModels.PatientDetails, FHIRPatientDetail>()
                 .ForMember(dest => dest.Status, opt =>
                 {
                     opt.PreCondition(src => src.StatusId != null);
                     opt.MapFrom(src => (PatientStatusTypes)src.StatusId);
                 })
                 .ForMember(dest => dest.StatusReason, opt =>
                 {
                     opt.PreCondition(src => src.StatusReasonId != null);
                     opt.MapFrom(src => (PatientStatusTypes)src.StatusReasonId);
                 });

            CreateMap<PatientDetail, PatientCreateRequest>()
                .ReverseMap();
            CreateMap<PatientAddressRequest, PatientAddress>()
                .ReverseMap();

            CreateMap<PatientAddressRequest, PatientModels.PatientAddress>();
            CreateMap<AddressMinimal, PatientModels.Addresses>();
            CreateMap<AddressMinimal, Address>();
            CreateMap<StandardizedAddress, Address>()
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.PostalCode) ? null : src.PostalCode))
                .ForMember(dest => dest.Plus4Code, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Plus4Code) ? null : src.Plus4Code))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Latitude) ? null : src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Longitude) ? null : src.Longitude))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => true));

            CreateMap<AddressSuggestionResult, SuggestionResponse>()
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.PostalCode.Split("-", StringSplitOptions.None)[0]))
                .ForMember(dest => dest.Plus4Code, opt => opt.MapFrom(src => (src.Address.PostalCode.Split("-", StringSplitOptions.None).Length == 2) ?
                                                                                 src.Address.PostalCode.Split("-", StringSplitOptions.None)[1] : null))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address.AddressLine1))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
                .ForMember(dest => dest.AddressKey, opt => opt.MapFrom(src => src.Address.AddressKey))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Address.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Address.Longitude))
                .ForMember(dest => dest.AddressKey, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => "United States of America"))
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => "Address Verified"));

            CreateMap<Candidate, Address>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Metadata.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Metadata.Longitude))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Components.ZipCode))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.DeliveryLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.DeliveryLine2))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Components.State))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Components.CityName))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.Metadata.CountyName))
                .ForMember(dest => dest.Plus4Code, opt => opt.MapFrom(src => src.Components.Plus4Code))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => "United States of America"));

            CreateMap<PatientModels.PatientAddress, PatientAddress>()
                .ReverseMap();
            CreateMap<PatientModels.Addresses, Address>()
                .ReverseMap();
            CreateMap<JsonPatchDocument<PatientDetail>, JsonPatchDocument<PatientModels.PatientDetails>>();
            CreateMap<Operation<PatientDetail>, Operation<PatientModels.PatientDetails>>();

            CreateMap<PatientModels.PhoneNumbers, PhoneNumberMinimal>()
                .ForMember(dest => dest.NumberType, opt => opt.MapFrom(src => (PhoneNumberType)src.NumberTypeId))
                .ReverseMap()
                .ForMember(dest => dest.NumberType, opt => opt.Ignore());

            CreateMap<PatientModels.PatientNotes, PatientNote>()
                .ReverseMap();

            CreateMap<PatientDetail, PatientLookUp>()
                .ForMember(dest => dest.PatientUuid, opt => opt.MapFrom(src => src.UniqueId));

            CreateMap<PatientModels.PatientDetails, PatientLookUp>()
                .ForMember(dest => dest.PatientUuid, opt => opt.MapFrom(src => src.UniqueId));

            CreateMap<FhirModels.Patient, FHIRPatientDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FhirPatientId, opt => opt.MapFrom(src => new Guid(src.Id)))
                .ForMember(dest => dest.FhirOrganizationId, opt => opt.MapFrom(src => GetFhirOrganizationId(src.ManagingOrganization)))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => string.Join(" ", src.Name.FirstOrDefault().Given)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.FirstOrDefault().Family))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.PatientAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.Telecom))
                .ForMember(dest => dest.PatientHeight, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.HEIGHT)))
                .ForMember(dest => dest.PatientWeight, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.WEIGHT)))
                .ForMember(dest => dest.IsInfectious, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.IS_INFECTIOUS)))
                .ForMember(dest => dest.HospiceId, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.HOSPICE_ID)))
                .ForMember(dest => dest.HospiceLocationId, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.LOCATION_ID)))
                .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => GetStringValue(src.Extension, PatientFhirKeys.DIAGNOSIS)))
                .ForMember(dest => dest.LastOrderNumber, opt => opt.MapFrom(src => GetStringValue(src.Extension, PatientFhirKeys.LAST_ORDER_NUMBER)))
                .ForMember(dest => dest.LastOrderDateTime, opt => opt.MapFrom(src => GetStringValue(src.Extension, PatientFhirKeys.LAST_ORDER_DATE_TIME)))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.PATIENT_CREATED_BY_USER_ID)))
                .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => GetStringValue(src.Extension, PatientFhirKeys.PATIENT_CREATED_DATE_TIME)))
                .ForMember(dest => dest.PatientNotes, opt => opt.MapFrom(src => src.Extension.Where(e => string.Equals(e.Url, PatientFhirKeys.NOTES, StringComparison.OrdinalIgnoreCase))
                                                                                .Select(v => v.Value as FhirModels.Annotation)));

            CreateMap<FhirModels.Address, PatientAddress>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.AddressTypeId, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.ADDRESS_TYPE_ID)))
                .ReverseMap();

            CreateMap<FhirModels.Address, Address>()
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Line.FirstOrDefault()))
                .ForMember(dest => dest.AddressLine2, opt =>
                {
                    opt.PreCondition(src => src.Line != null && src.Line.Count() > 1);
                    opt.MapFrom(src => src.Line.ElementAt(1));
                })
                .ForMember(dest => dest.AddressLine3, opt =>
                {
                    opt.PreCondition(src => src.Line != null && src.Line.Count() > 2);
                    opt.MapFrom(src => src.Line.ElementAt(2));
                })
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => GetZipCodeValue(src.PostalCode)))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.LATITUDE)))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.LONGITUDE)))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.IS_VERIFIED)))
                .ForMember(dest => dest.Plus4Code, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.PLUS4_CODE)))
                .ForMember(dest => dest.AddressUuid, opt => opt.MapFrom(src => GetStringValue(src.Extension, PatientFhirKeys.ADDRESS_UUID)))
                .ReverseMap();

            CreateMap<FhirModels.ContactPoint, PhoneNumberMinimal>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => Convert.ToInt64(Regex.Replace(src.Value, "[(),-/ ]", ""))))
                .ForMember(dest => dest.NumberTypeId, opt => opt.MapFrom(src => GetDecimalValue(src.Extension, PatientFhirKeys.PHONE_NUMBER_TYPE)))
                .ForMember(dest => dest.NumberType, opt => opt.MapFrom(src => GetPhoneNumberType(src.Extension, PatientFhirKeys.PHONE_NUMBER_TYPE)))
                .ForMember(dest => dest.ReceiveEtaTextmessage, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.ETA_MESSAGE)))
                .ForMember(dest => dest.ReceiveSurveyTextMessage, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.SURVEY_MESSAGE)))
                .ForMember(dest => dest.IsSelfPhone, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.IS_SELF_PHONE)))
                .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => GetStringValue(src.Extension, PatientFhirKeys.PHONE_NUMBER_CONTACT_NAME)));

            CreateMap<FhirModels.Annotation, PatientNote>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Text.Value))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => (src.Author as FhirModels.FhirString).Value))
                .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => src.Time));

            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>));

            CreateMap<PatientStatusRequest, PatientStatusValidationRequest>().ReverseMap();

            CreateMap<MergePatientRequest, MergePatientBaseRequest>().ReverseMap();

            CreateMap<PatientModels.PatientMergeHistory, PatientMergeHistoryResponse>()
                .ForMember(dest => dest.ChangeLog, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<MergePatientBaseRequest>(src.ChangeLog)))
                .ForMember(dest => dest.MergedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(dest => dest.MergedDateTime, opt => opt.MapFrom(src => src.CreatedDateTime))
                .ReverseMap();

        }

        private string GetStatusMessage(PatientModels.PatientDetails patientDetail)
        {
            var status = patientDetail.StatusId.HasValue ? (PatientStatusTypes)patientDetail.StatusId : PatientStatusTypes.Inactive;
            switch (status)
            {
                //Hover shows: "Active patient, no open order(s)" 
                case PatientStatusTypes.Active:
                    return "Active Patient, No open order(s)";

                //Hover shows: Last order pick up reason "Discharge/Deceased/Respite/Not needed, [effective date]" 
                case PatientStatusTypes.Inactive:
                    if (patientDetail.StatusReason != null)
                    {
                        return $"{patientDetail.StatusReason.Name}, {patientDetail.StatusChangedDate?.ToShortDateString()}";
                    }
                    return string.Empty;

                case PatientStatusTypes.PendingActive:
                    return "Active Patient, Pending Order(s)";
                case PatientStatusTypes.Pending:
                    return "Pending Patient, Pending Order(s)";
                case PatientStatusTypes.Blank:
                    return "No current inventory and no open order(s)";
                default:
                    return string.Empty;
            }
        }

        private T GetExtensionValue<T>(List<FhirModels.Extension> attributes, string key) where T : FhirModels.Element
        {
            var patientExtension = attributes.FirstOrDefault(a => string.Equals(a.Url, key, StringComparison.OrdinalIgnoreCase));
            return patientExtension?.Value as T;
        }

        private string GetStringValue(List<FhirModels.Extension> attributes, string key)
        {
            return GetExtensionValue<FhirModels.FhirString>(attributes, key)?.Value;
        }

        private bool GetBooleanValue(List<FhirModels.Extension> attributes, string key)
        {
            var extension = GetExtensionValue<FhirModels.FhirBoolean>(attributes, key);
            if (extension != null)
            {
                return extension.Value.Value;
            }
            return false;
        }

        private decimal GetDecimalValue(List<FhirModels.Extension> attributes, string key)
        {
            var extension = GetExtensionValue<FhirModels.FhirDecimal>(attributes, key);
            if (extension != null)
            {
                return extension.Value.Value;
            }
            return 0;
        }

        private int GetZipCodeValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return 0;
            }

            var zipComponents = key.Split('-');
            if (zipComponents.Length > 0)
            {
                return Convert.ToInt32(zipComponents[0]);
            }
            return 0;
        }

        private string GetPhoneNumberType(List<FhirModels.Extension> attributes, string key)
        {
            int phoneNumberTypeId = Convert.ToInt32(GetDecimalValue(attributes, key));
            return ((PhoneNumberType)phoneNumberTypeId).ToString();
        }

        private Guid? GetFhirOrganizationId(FhirModels.ResourceReference fhirOrganizationReference)
        {
            if (fhirOrganizationReference == null)
            {
                return null;
            }

            return new Guid(fhirOrganizationReference.ElementId);
        }
    }
}