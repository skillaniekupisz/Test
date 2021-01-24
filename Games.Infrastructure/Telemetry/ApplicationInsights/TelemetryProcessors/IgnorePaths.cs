using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Games.Infrastructure.Telemetry.ApplicationInsights.TelemetryProcessors
{
    public class IgnorePaths : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationInsightsOptions _options;

        public IgnorePaths(ITelemetryProcessor next, ApplicationInsightsOptions options, IHttpContextAccessor httpContextAccessor = null)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
            _options = options;
        }

        public void Process(ITelemetry telemetry)
        {
            var httpContext = _httpContextAccessor?.HttpContext;

            if (httpContext != null && _options.IgnoredPaths != null)
            {
                var path = httpContext.Request.Path;
                foreach (var ignoredPath in _options.IgnoredPaths)
                {
                    if (Regex.IsMatch(path, $"^{ignoredPath}$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
                        return;
                }
            }

            _next.Process(telemetry);
        }
    }
}
