using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.ViewModels;
using NSViewModel = HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.Http;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IHospiceMemberService
    {
        Task<PaginatedList<HospiceMember>> GetAllHospiceMembers(int hospiceId, SieveModel sieveModel, string roleName);

        Task<HospiceApprover> GetApproverContacts();

        Task<HospiceMember> GetHospiceMemberById(int hospiceId, int memberId);

        Task<HospiceMember> CreateHospiceMember(int hospiceId, HospiceMemberRequest hospiceMemberRequest);

        Task<HospiceMember> UpdateHospiceMember(int hospiceId, int memberId, HospiceMemberRequest memberUpdateRequest);

        Task SetMemberPassword(int hospiceId, int memberId, UserPasswordRequest userPasswordRequest);

        Task SendMemberPasswordResetLink(int hospiceId, int memberId, NotificationBase resetPasswordNotification);

        Task UpsertApprovers(NSViewModel.ApproverRequest approverRequest);

        Task<NSViewModel.HospiceContactResponse> UpdateHospiceContact(NSViewModel.HospiceContactRequest hospiceContactRequest);

        Task<(IEnumerable<HospiceMember>, IEnumerable<HospiceMemberCsvRequest>, ValidatedList<HospiceMemberCsvRequest>)> CreateHospiceMembersFromCsvFile(
            int hospiceId, 
            IFormFile members, 
            bool validateOnly, 
            bool parseOnly);

        Task DeleteHospiceMember(int hospiceId, int memberId);

        Task<HospiceMember> CreateInternalHospiceMember(User user);

        Task DeleteInternalHospiceMember(int userId);

        Task<HospiceMember> UpdateHospiceMemberInNetSuite(int userId);
    }
}
