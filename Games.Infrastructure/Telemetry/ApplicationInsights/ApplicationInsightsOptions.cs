using System.Collections.Generic;

namespace Games.Infrastructure.Telemetry.ApplicationInsights
{
    public class ApplicationInsightsOptions: TelemetryOptions
    {
        public string InstrumentationKey { get; set; }
        public IList<string> IgnoredPaths { get; set; }
    }
}
