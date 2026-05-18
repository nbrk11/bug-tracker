namespace BugTracker.Application;

public class ResponseWrapper<T>
{
    public T? Value { get; }
    public string? Error { get; } 
    public bool IsSuccess => Error is null;

    private ResponseWrapper(T value) => Value = value;
    private ResponseWrapper(string error) => Error = error;

    public static ResponseWrapper<T> Success(T value)
    {
        return new ResponseWrapper<T>(value);
    }

    public static ResponseWrapper<T> Fail(string error)
    {
        return new ResponseWrapper<T>(error);
    }
}