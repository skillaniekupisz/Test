using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System.Collections.Generic;
using System.Diagnostics;

namespace Games.Infrastructure.Telemetry.ApplicationInsights.TelemtryInitializer
{
    public class LogActivityTags : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var activity = Activity.Current;
            var operationTelemetry = telemetry as OperationTelemetry;

            if (activity == null || operationTelemetry == null) return;

            foreach (var keyValue in activity.Tags)
            {
                operationTelemetry.Properties.TryAdd(keyValue.Key, keyValue.Value);
            }
        }
    }
}
