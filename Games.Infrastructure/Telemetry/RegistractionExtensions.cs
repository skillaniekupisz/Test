using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;

namespace Games.Infrastructure.Telemetry
{
    public static class RegistractionExtensions
    {
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> EnableTelemetry<TLimit, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration)
            => registration.EnableClassInterceptors().InterceptedBy(typeof(AsyncInterceptorAdapter<ITelemetryInterceptor>));

        public static IRegistrationBuilder<TLimit, TConreteReflectionActivatorData, TRegistrationStyle> EnableTelemetry<TLimit, TConreteReflectionActivatorData, TRegistrationStyle>(
         this IRegistrationBuilder<TLimit, TConreteReflectionActivatorData, TRegistrationStyle> registration)
            where TConreteReflectionActivatorData : ConcreteReflectionActivatorData
             => registration.EnableClassInterceptors().InterceptedBy(typeof(AsyncInterceptorAdapter<ITelemetryInterceptor>));
    }
}
