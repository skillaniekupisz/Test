using Autofac;
using Games.Core.Services;
using Games.Core.Services.Registration;
using Games.Core.Services.UserManagement;
using Microsoft.Extensions.Configuration;

namespace Games.Core
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterCore(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<GameManagementService>();
            builder.RegisterType<CommentService>();
            builder.RegisterType<RegistrationService>();
            builder.RegisterType<UserManagementService>();
        }
    }
}
