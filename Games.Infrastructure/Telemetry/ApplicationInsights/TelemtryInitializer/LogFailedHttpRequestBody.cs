using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Games.Infrastructure.Telemetry.ApplicationInsights.TelemtryInitializer
{
    public class LogFailedHttpRequestBody : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogFailedHttpRequestBody(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemtry = telemetry as RequestTelemetry;
            var httpContext = _httpContextAccessor?.HttpContext;
            var response = httpContext?.Response;
            var request = httpContext?.Request;

            if (httpContext == null || requestTelemtry?.Success != false || response == null || response.StatusCode == 403)
                return;

            httpContext.Items.TryGetValue("HttpRequestBody", out var requestbody);

            if (requestbody is string requestBodyString)
                requestTelemtry.Properties.Add("RequestBody", requestBodyString);

            requestTelemtry.Properties.Add("PathAndQuery", request.Path + request.QueryString);
        }
    }
}
