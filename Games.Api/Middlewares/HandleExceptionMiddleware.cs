using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Games.Api.Middlewares
{
    public class HandleExceptionMiddleware : IMiddleware
    {

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public HandleExceptionMiddleware(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            _jsonSerializerSettings.Converters.Add(new ProblemDetailsConverter());
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {


                var statusCode = GetStatusCode(ex);
                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = ReasonPhrases.GetReasonPhrase(statusCode),
                    Type = $"https://httpstatuses.com{statusCode}",
                    Detail = ex is Core.Exceptions.ValidationException ? ex.Message : default
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails, _jsonSerializerSettings));
            }
        }

        private int GetStatusCode(Exception ex) => ex switch
        {
            Core.Exceptions.ValidationException _ => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
