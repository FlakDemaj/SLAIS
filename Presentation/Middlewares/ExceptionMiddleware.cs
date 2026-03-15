using System.Net;
using System.Text.Json.Serialization;
using Application.Utils;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Presentation.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SAISException e)
        {
            await HandleException(context, e);
        }
    }

    private Task HandleException(HttpContext context, SAISException e)
    {
        context.Response.ContentType = "application/json";
        
        var errorCodeCategory = GetErrorCategory(e.ErrorCode);

        switch (errorCodeCategory)
        {
            case 0:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            case 1:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
        }

        var response = new
        {
            ErrorCode = e.ErrorCode,
            ErrorMessage = e.ErrorMessage,
        };
        
        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
    
    private static int GetErrorCategory(int errorCode)
    {
        var absoluteCode = Math.Abs(errorCode);
        var codeString = absoluteCode.ToString();

        return int.Parse(codeString[0].ToString());
    }
}