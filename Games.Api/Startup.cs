using Autofac;
using Games.Api.Authentication;
using Games.Api.Middlewares;
using Games.Core;
using Games.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Games.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiAuthentication(_configuration.GetSection("Api:Authentication"));
            services.AddAuthorization();

            services.AddControllers()
                .AddNewtonsoftJson(s =>
                {
                    s.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    s.SerializerSettings.Formatting = Formatting.None;
                    s.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                        new List<string>()
                    }
                });

                var appNAme = GetType().Assembly.GetName().Name;
                c.SwaggerDoc(appNAme, new OpenApiInfo { Title = appNAme });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterApi(_configuration.GetSection("Api"));
            builder.RegisterCore(_configuration.GetSection("Core"));
            builder.RegisterInfrastructure(_configuration.GetSection("Infrastructure"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseMiddleware<AddHttpRequestBodyMiddleware>();
            app.UseMiddleware<HandleExceptionMiddleware>();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod());
            app.UseSwaggerUI(c =>
            {
                var appNAme = GetType().Assembly.GetName().Name;
                c.SwaggerEndpoint($"{appNAme}/swagger.json", appNAme);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
