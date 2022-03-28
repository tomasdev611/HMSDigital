using Microsoft.AspNetCore.Mvc;

namespace HMSDigital.Core.ViewModels
{
    public class LoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; }

        [FromForm(Name = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
