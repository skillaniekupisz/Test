using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Games.Infrastructure.Telemetry.ApplicationInsights.TelemtryInitializer
{
    public class Set401sAsSuccess : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is RequestTelemetry requestTelemetry && requestTelemetry.ResponseCode == "401")
                requestTelemetry.Success = true;
        }
    }
}
