using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Games.Infrastructure.Telemetry.ApplicationInsights.TelemtryInitializer
{
    public class LogUserDetails : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogUserDetails(IHttpContextAccessor httpContextAccessor = null)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemtry = telemetry as RequestTelemetry;
            var httpContext = _httpContextAccessor?.HttpContext;

            if (httpContext == null || requestTelemtry == null)
                return;

            var userNAme = httpContext.User.Claims.FirstOrDefault(x => x.Type == "NickName")?.Value;
            if (userNAme != null)
                requestTelemtry.Properties.Add("User", userNAme);
        }
    }
}
