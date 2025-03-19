using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Matrix;

public class CatchAllMiddleware : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        InternalServerError<Exception> error = new InternalServerError<Exception>(context.Exception);
        JsonResult result = new JsonResult(error);
        result.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = result;
        context.ExceptionHandled = true;

        // Log to serilog
        Log.Error("ERROR - Internal Server Error: '" + context.Exception + "'");
    }
}
