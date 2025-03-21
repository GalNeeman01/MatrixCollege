using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Matrix;

public class CatchAllFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Send to client minimal error message
        InternalServerError<string> error = new InternalServerError<string>(context.Exception.Message);
        JsonResult result = new JsonResult(error);
        result.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = result;
        context.ExceptionHandled = true;

        // Log to serilog full error details for development debugging
        Log.Error("ERROR - Internal Server Error: '" + context.Exception + "'");
    }
}
