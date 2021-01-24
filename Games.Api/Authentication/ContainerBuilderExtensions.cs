using Autofac;
using Microsoft.Extensions.Configuration;

namespace Games.Api.Authentication
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterAuthentication(this ContainerBuilder builder, IConfiguration configuration)
        {
            var options = configuration.Get<AuthenticationOptions>();
            builder.RegisterInstance(options);
            builder.RegisterType<JwtGenerator>();
        }
    }
}
