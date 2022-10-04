using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace ETicaretAPI.API.Extensions
{
    static public class ConfigureExceptionHandlerExtension
    {

        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                   var contextfeatures= context.Features.Get<IExceptionHandlerFeature>();
                    if (contextfeatures != null)
                    {
                        logger.LogError(contextfeatures.Error.Message);

                      await  context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            StatusCode=context.Response.StatusCode,
                            Message=contextfeatures.Error.Message,
                            Title="Hata alındı!"
                        }));
                    }
                });
            });
        }
    }
}
