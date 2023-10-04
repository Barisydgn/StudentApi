using Entities.ErrorModels;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Service.Contracts;

namespace EmployeeApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        //public static void ConfigureExceptionHandler(this WebApplication app,ILogService logger)
        //{
        //    app.UseExceptionHandler(options =>
        //    {
        //        options.Run(async context =>
        //        {
        //            context.Response.ContentType = "application/json";
        //            var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();


        //            logger.LogError($"Bir şeyler ters gitti : {contextFeatures.Error}");

        //            await context.Response.WriteAsync(new ErrorDetails()
        //            {
        //                StatusCode=context.Response.StatusCode,
        //                Message=contextFeatures.Error.Message
        //            }.ToString());

        //        });
        //    });
        //}


        public static void ConfigureExceptionHandler(this WebApplication app,ILogService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeatures=context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeatures is not null)
                    {
                        context.Response.StatusCode = contextFeatures.Error switch
                        {
                            EmployeeNotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Bir şeyler ters gitti {contextFeatures.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeatures.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
