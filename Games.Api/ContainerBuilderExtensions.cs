using Autofac;
using Games.Api.Authentication;
using Games.Api.Middlewares;
using Games.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace Games.Api
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterApi(this ContainerBuilder builder, IConfiguration configuration)
        {
            var options = configuration.GetValueOrDefault<ApiOptions>();

            builder.RegisterAuthentication(configuration.GetSection("Authentication"));
            builder.RegisterType<AddHttpRequestBodyMiddleware>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<HandleExceptionMiddleware>().AsSelf().AsImplementedInterfaces();
        }
    }
}
