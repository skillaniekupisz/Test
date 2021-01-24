using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Games.Infrastructure.Telemetry.ApplicationInsights.TelemtryInitializer
{
    public class NormalizedHttpRequestName : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NormalizedHttpRequestName(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemtry = telemetry as RequestTelemetry;
            var request = _httpContextAccessor?.HttpContext?.Request;
            if (request == null || requestTelemtry == null)
                return;

            requestTelemtry.Name = $"{request.Method} {request.Path.Value?.ToLowerInvariant()}";
            requestTelemtry.Context.Operation.Name = requestTelemtry.Name;
        }
    }
}
