using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc.ViewFeatures; 


public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandler>();
    }

}

public class ErrorHandler
{
    private readonly ITempDataDictionaryFactory temDataFactory;

    public ErrorHandler(ITempDataDictionaryFactory temDataFactory)
    {
        this.temDataFactory = temDataFactory;
    }
    private readonly RequestDelegate next;

    public ErrorHandler(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            throw;
 
             
        }
    } 
}