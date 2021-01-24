
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Games.Api.Middlewares
{
    public class AddHttpRequestBodyMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;

            var hasBody = request.Method == HttpMethod.Post.ToString() || request.Method == HttpMethod.Put.ToString();
            if (hasBody)
            {
                await using var bodyStream = new MemoryStream();
                request.Body.Position = 0;
                await request.Body.CopyToAsync(bodyStream);
                request.Body.Position = 0;

                var requestBody = Encoding.UTF8.GetString(bodyStream.ToArray());

                context.Items["HttpRequestBody"] = requestBody;
            }

            await next(context);
        }
    }
}
