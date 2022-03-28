using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class HospiceMember : UserBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int HospiceId { get; set; }

        public string Designation { get; set; }

        public IEnumerable<HospiceLocation> HospiceLocations { get; set; }

        public IEnumerable<HospiceLocationMember> HospiceLocationMembers { get; set; }

        public string CognitoUserId { get; set; }

        public int NetSuiteContactId { get; set; }

        public bool CanAccessWebStore { get; set; }

        public bool CanApproveOrder { get; set; }
    }
}
