using Autofac;
using Games.Infrastructure.Telemetry.ApplicationInsights;
using Microsoft.Extensions.Configuration;

namespace Games.Infrastructure.Telemetry
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterTelemetry(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterGeneric(typeof(AsyncInterceptorAdapter<>)).SingleInstance();

            var type = configuration.GetValue<TelemetryType>(nameof(TelemetryOptions.Type));
            switch (type)
            {
                case TelemetryType.ApplicationInsights:
                    builder.RegisterApplicationInsightsTelemetry(configuration);
                    break;
                default:
                    break;
            }
        }
    }
}
