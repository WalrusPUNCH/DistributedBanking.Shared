namespace Contracts;

public class OperationStatusModel
{
    public bool EndedSuccessfully { get; set; }
    public string? Message { get; set; }

    public static OperationStatusModel Success() => new()
    {
        EndedSuccessfully = true
    };

    public static OperationStatusModel Fail(string? message) => new()
    {
        EndedSuccessfully = false,
        Message = message
    };
}

public class OperationStatusModel<T> : OperationStatusModel
{
    public T? ResponseValue { get; set; }

    public static OperationStatusModel<T> Success(T responseValue) => new()
    {
        EndedSuccessfully = true,
        ResponseValue = responseValue
    };
    
    public new static OperationStatusModel<T> Fail(string? message) => new()
    {
        EndedSuccessfully = false,
        Message = message,
        ResponseValue = default
    };
}
