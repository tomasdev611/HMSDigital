using HMSDigital.Common.ViewModels;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class SiteMember : UserBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int SiteId { get; set; }

        public string Designation { get; set; }

        public string CognitoUserId { get; set; }

        public Site Site { get; set; }

        public IEnumerable<Role> Roles { get; set; }
    }
}
