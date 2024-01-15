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
