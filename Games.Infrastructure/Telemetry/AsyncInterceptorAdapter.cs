using Castle.DynamicProxy;

namespace Games.Infrastructure.Telemetry
{
    public class AsyncInterceptorAdapter<TAsyncInterceptor> : AsyncDeterminationInterceptor
        where TAsyncInterceptor : IAsyncInterceptor
    {

        public AsyncInterceptorAdapter(TAsyncInterceptor interceptor) : base(interceptor)
        {
        }
    }
}
