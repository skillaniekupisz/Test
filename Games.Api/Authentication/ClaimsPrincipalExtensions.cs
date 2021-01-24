using System.Linq;
using System.Security.Claims;

namespace Games.Api.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "NickName")?.Value;
    }
}
