using Castle.Core.Internal;
using Castle.DynamicProxy;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Threading.Tasks;

namespace Games.Infrastructure.Telemetry.ApplicationInsights
{
    public class ApplicationInsightsTelemetryInterceptor : ITelemetryInterceptor
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsTelemetryInterceptor(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptasynchronous(invocation);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptasynchronous<TResult>(invocation);
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            using (TrackOperation(invocation))
            {
                invocation.Proceed();
            }
        }

        private async Task InternalInterceptasynchronous(IInvocation invocation)
        {
            using (TrackOperation(invocation))
            {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
            }
        }
        private async Task<TResult> InternalInterceptasynchronous<TResult>(IInvocation invocation)
        {
            using (TrackOperation(invocation))
            {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                var resut = await task;
                return resut;
            }
        }

        private IDisposable TrackOperation(IInvocation invocation)
        {
            var operationName = $"{invocation.TargetType.Name}.{invocation.Method.Name}";

            if (invocation.Method.GetAttribute<TrackDependency>() != null)
            {
                var opertaion = _telemetryClient.StartOperation<DependencyTelemetry>(operationName);
                opertaion.Telemetry.Type = "InProc";
                return opertaion;
            }

            return null;
        }
    }
}
