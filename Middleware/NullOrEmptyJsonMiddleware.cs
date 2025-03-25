using System.Text.Json;

namespace Matrix;

public class NullOrEmptyJsonMiddleware
{
    private readonly RequestDelegate _next;

    public NullOrEmptyJsonMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only apply the logic to POST, PUT, or PATCH requests
        if (context.Request.Method != HttpMethods.Post && context.Request.Method != HttpMethods.Put && context.Request.Method != HttpMethods.Patch)
        {
            await _next(context); // Continue request pipeline
            return;
        }

        // Check for empty request
        if (context.Request.ContentLength == 0)        {
            await FailedCheck(context);
            return;
        }

        // Ensure Content-Type header is JSON
        if (!context.Request.ContentType?.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) ?? true)
        {
            await FailedCheck(context);
            return;
        }

        // --- Read request data --- //
        context.Request.EnableBuffering();
        using StreamReader reader = new StreamReader(context.Request.Body);
        string body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        // Ensure the request data is in valid json format & either an object or an array
        try
        {
            using var jsonDoc = JsonDocument.Parse(body);
            
            if (jsonDoc.RootElement.ValueKind != JsonValueKind.Object &&
                jsonDoc.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidDataException(); // Throw to catch area
            }
        }
        catch
        {
            await FailedCheck(context);
            return;
        }

        await _next(context);
    }

    // Return failed response
    private async Task FailedCheck(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new RequestDataError());
    }
}
