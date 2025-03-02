using System.Text.Json;

namespace Matrix;

public class BaseError
{
    public string Message { get; set; } = null!;

    public BaseError(string message)
    {
        Message = message;
    }
}

public class ResourceNotFoundError : BaseError
{
    public ResourceNotFoundError(string resourceId) : base($"No resource with the id of {resourceId} was found.") { }
}

public class ValidationError : BaseError
{
    public ValidationError(string message) : base (message) { }
}
