using Autofac;
using Games.Infrastructure.Repositories;
using Games.Infrastructure.Telemetry;
using JsonNet.ContractResolvers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Games.Infrastructure
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterInfrastructure(this ContainerBuilder builder, IConfiguration configuration)
        {

            builder.Register(x => CreateDefaultJsonSerializerSettings()).InstancePerDependency();
            JsonConvert.DefaultSettings = CreateDefaultJsonSerializerSettings;

            builder.RegisterRepositories(configuration.GetSection("Repositories"));
            builder.RegisterTelemetry(configuration.GetSection("Telemetry"));
        }

        private static JsonSerializerSettings CreateDefaultJsonSerializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterAndCtorCamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyy-MM-ddTHH:mm:ss.ffffffK",
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };

            serializerSettings.Converters.Add(new StringEnumConverter());

            return serializerSettings;
        }
    }
}
