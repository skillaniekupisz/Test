using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Games.Infrastructure.Telemetry.ApplicationInsights.TelemetryProcessors
{
    public class Ignore404s : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;

        public Ignore404s(ITelemetryProcessor next)
        {
            _next = next;
        }

        public void Process(ITelemetry telemetry)
        {
            if (telemetry is RequestTelemetry requestTelemetry && requestTelemetry.ResponseCode == "404")
                return;

            _next.Process(telemetry);
        }
    }
}
