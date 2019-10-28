using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IndividualsDirectory.Helpers.CustomMiddlewares
{
    public static class GlobalExceptionHandlerExtensions
    {
        public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app, ILogger logger, string errorPage)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;
                    var errorDetails = $"Global exception handler:\r\n" +
                                       $"Exception: {exception.Message}\r\n" +
                                       $"StackTrace: {exception.StackTrace}";

                    logger.LogCritical(errorDetails);

                    var matchText = "JSON";
                    var isJsonRequest = context.Request.GetTypedHeaders().Accept
                        .Any(i => i.Suffix.Value?.ToUpper() == matchText || i.SubTypeWithoutSuffix.Value?.ToUpper() == matchText);

                    if (isJsonRequest)
                    {
                        var json = JsonConvert.SerializeObject(new
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                            Message = "Oops. Something went wrong. Please contact administrator."
                        });

                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(json);
                    }
                    else
                    {
                        context.Response.Redirect(errorPage);

                        await Task.CompletedTask;
                    }
                });
            });
        }
    }
}
