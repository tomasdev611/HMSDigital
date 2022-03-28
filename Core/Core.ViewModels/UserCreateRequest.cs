using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public class UserCreateRequest : UserMinimal
    {
        public IEnumerable<UserRoleBase> UserRoles { get; set; }
    }
}
