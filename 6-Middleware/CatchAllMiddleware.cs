using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Matrix;

public class CatchAllMiddleware : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        string message =  GetInnerMessage(context.Exception);
        InternalServerError<string> error = new InternalServerError<string>(message);
        JsonResult result = new JsonResult(error);
        result.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = result;
        context.ExceptionHandled = true;
        // Log to serilog
        Log.Error("ERROR - Internal Server Error: '" + message + "'");
    }

    private string GetInnerMessage(Exception ex)
    {
        if (ex == null) return ""; // Just In Case

        if (ex.InnerException == null)
            return ex.Message;

        return GetInnerMessage(ex.InnerException);
    }
}
