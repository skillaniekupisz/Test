using System;

namespace Games.Api.Authentication
{
    public class AuthenticationOptions
    {
        public int ApplicationKeySize { get; set; } = 2048;
        public string Key { get; set; }
        public TimeSpan TokenExpiration { get; set; } = TimeSpan.FromMinutes(60);
    }
}
