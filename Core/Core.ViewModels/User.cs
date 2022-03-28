using System;

namespace HMSDigital.Core.ViewModels
{
    public class User : UserBase
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public Uri ProfilePictureUrl { get; set; }
    }
}
