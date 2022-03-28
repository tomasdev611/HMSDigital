using HMSDigital.Common.ViewModels;
using System;
using System.Threading.Tasks;
using NSViewModel = HMSDigital.Core.ViewModels.NetSuite;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IHospiceV2Service
    {
        Task<NSViewModel.HospiceResponse> UpsertHospiceWithLocation(NSViewModel.NSHospiceRequest hospiceRequest);

        Task<Hospice> UpdateHospiceFhirOrganizationId(int hospiceId, Guid fhirOrganizationId);
    }
}
