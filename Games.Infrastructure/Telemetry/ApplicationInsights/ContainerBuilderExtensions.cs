using Autofac;
using Autofac.Extensions.DependencyInjection;
using Games.Common.Configuration;
using Games.Infrastructure.Telemetry.ApplicationInsights.TelemetryProcessors;
using Games.Infrastructure.Telemetry.ApplicationInsights.TelemtryInitializer;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Games.Infrastructure.Telemetry.ApplicationInsights
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterApplicationInsightsTelemetry(this ContainerBuilder builder, IConfiguration configuration)
        {
            var options = configuration.GetValueOrDefault<ApplicationInsightsOptions>();

            builder.RegisterInstance(options);
            builder.RegisterType<ApplicationInsightsTelemetryInterceptor>().AsImplementedInterfaces().SingleInstance();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddApplicationInsightsTelemetry(options.InstrumentationKey);

            serviceCollection.AddApplicationInsightsTelemetryProcessor<Ignore404s>();
            serviceCollection.AddApplicationInsightsTelemetryProcessor<IgnorePaths>();

            serviceCollection.AddSingleton<ITelemetryInitializer, NormalizedHttpRequestName>();
            serviceCollection.AddSingleton<ITelemetryInitializer, LogActivityTags>();
            serviceCollection.AddSingleton<ITelemetryInitializer, Set401sAsSuccess>();
            serviceCollection.AddSingleton<ITelemetryInitializer, LogUserDetails>();
            serviceCollection.AddSingleton<ITelemetryInitializer, LogFailedHttpRequestBody>();

            builder.Populate(serviceCollection);
        }
    }
}
