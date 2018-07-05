using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Taxology.Site.Models;


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


            var request = context.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            if (request.Host.Port.HasValue)
            {
                uriBuilder.Port = request.Host.Port.Value;
            }
            uriBuilder.Path = "Home/Error";

            var url = uriBuilder.Uri;


          //  var result = new RedirectResult(url.AbsolutePath);

            //
            //            string message = ex.Message;
            //
            ErrorViewModel errModel = new ErrorViewModel();
            errModel.Message = ex.Message;




         //   var tempData = temDataFactory.GetTempData(context);
            //TODO need to remove magic string --- but also probably just need
            //to remove this whole temp data thing. prod users won't nee to see the details
            
          //  tempData["error"] = ex.Message;
           // tempData.Save();

           
            var result = new RedirectToActionResult("Error", "Home", errModel );
            //var result = new RedirectToActionResult("Error", "Home", null);

            RouteData routeData = context.GetRouteData();
            if (routeData == null) routeData = new RouteData();
            ActionDescriptor actionDescriptor = new ActionDescriptor();
            ActionContext actionContext = new ActionContext(context, routeData, actionDescriptor);
            await result.ExecuteResultAsync(actionContext);
             
        }
    } 
}