namespace Matrix;

public abstract class BaseError<T>
{
    public T Errors { get; set; }

    public BaseError(T message)
    {
        Errors = message;
    }
}

public class GeneralError : BaseError<string>
{
    public GeneralError(string message) : base(message) { }
}

public class ResourceNotFoundError : BaseError<string>
{
    public ResourceNotFoundError(string resourceId) : base($"No resource with the id of {resourceId} was found.") { }
}

public class ValidationError<T> : BaseError<T>
{
    public ValidationError(T errors) : base(errors) { }
}

public class RequestDataError : BaseError<string>
{
    public RequestDataError() : base("The request data is in invalid.") { }
}

public class InternalServerError<T> : BaseError<T>
{
    public InternalServerError(T errors) : base(errors) { }
}

