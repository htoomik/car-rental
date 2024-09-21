namespace CarRental.Domain;

public record ExecutionResult<T>
{
    public bool Success { get; }
    public T? Result { get; }
    public List<string> Errors { get; } = new();

    private ExecutionResult(bool success, T? result)
    {
        Success = success;
        Result = result;
    }

    private ExecutionResult(bool success, List<string> errors)
    {
        Success = success;
        Errors = errors;
    }

    public static ExecutionResult<T> ForSuccess<T>(T result)
    {
        return new ExecutionResult<T>(true, result);
    }

    public static ExecutionResult<T> ForFailure(List<string> errors)
    {
        return new ExecutionResult<T>(false, errors);
    }
}

public record ExecutionResult
{
    public bool Success { get; }
    public List<string> Errors { get; } = new();

    private ExecutionResult(bool success)
    {
        Success = success;
    }

    private ExecutionResult(bool success, List<string> errors)
    {
        Success = success;
        Errors = errors;
    }

    public static ExecutionResult ForSuccess()
    {
        return new ExecutionResult(true);
    }

    public static ExecutionResult ForFailure(List<string> errors)
    {
        return new ExecutionResult(false, errors);
    }
}