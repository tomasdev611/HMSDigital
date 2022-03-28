using AutoMapper;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.ViewModels;
using HMSDigital.Patient.FHIR.BusinessLayer.Constants;
using HMSDigital.Patient.FHIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FhirModels = Hl7.Fhir.Model;

namespace Patient.FHIRIntegration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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
                .ForMember(dest => dest.NumberType, opt => opt.MapFrom(src => src.Use.ToString()))
                .ForMember(dest => dest.ReceiveEtaTextmessage, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.ETA_MESSAGE)))
                .ForMember(dest => dest.ReceiveSurveyTextMessage, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.SURVEY_MESSAGE)))
                .ForMember(dest => dest.IsSelfPhone, opt => opt.MapFrom(src => GetBooleanValue(src.Extension, PatientFhirKeys.IS_SELF_PHONE)));

            CreateMap<FhirModels.Annotation, PatientNote>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Text.Value))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => (src.Author as FhirModels.FhirString).Value))
                .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => src.Time));
        }

        private T GetExtensionValue<T>(List<FhirModels.Extension> attributes, string key) where T : FhirModels.Element
        {
            var patientExtension = attributes.FirstOrDefault(a => string.Equals(a.Url, key, StringComparison.OrdinalIgnoreCase));
            return patientExtension?.Value as T;
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

        private bool GetBooleanValue(List<FhirModels.Extension> attributes, string key)
        {
            var extension = GetExtensionValue<FhirModels.FhirBoolean>(attributes, key);
            if (extension != null)
            {
                return extension.Value.Value;
            }
            return false;
        }

        private string GetStringValue(List<FhirModels.Extension> attributes, string key)
        {
            return GetExtensionValue<FhirModels.FhirString>(attributes, key)?.Value;
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
