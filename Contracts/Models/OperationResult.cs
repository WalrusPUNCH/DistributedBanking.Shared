namespace Contracts.Models;

public class OperationResult
{
    public OperationStatus Status { get; }
    public IEnumerable<string> Messages { get; }

    protected OperationResult(OperationStatus status, IEnumerable<string>? messages = null)
    {
        Status = status;
        Messages = messages ?? Array.Empty<string>();
    }

    public static OperationResult Success(params string[] messages) => new(OperationStatus.Success, messages);
    public static OperationResult Fail(params string[] messages) => new(OperationStatus.Fail, messages);
    public static OperationResult InternalFail(params string[] messages) => new(OperationStatus.InternalFail, messages);
    public static OperationResult BadRequest(params string[] messages) => new(OperationStatus.BadRequest, messages);
    public static OperationResult Processing(params string[] messages)
    {
        var allMessages = messages.ToList();
        allMessages.Add("Your operation is still processing. Check for it results later");
        
        return new OperationResult(OperationStatus.Processing, allMessages);
    }

    public override string ToString()
    {
        return $"{Status}: {string.Join(", ", Messages.ToList())}";
    }
}

public class OperationResult<T> : OperationResult
{
    public T? Response { get; set; }

    private OperationResult(OperationStatus status, T? response, IEnumerable<string>? messages = null) 
        : base(status, messages)
    {
        Response = response;
    }
    
    public static OperationResult<T> Success(T? response, params string[] messages) => new(OperationStatus.Success, response, messages);
    public static OperationResult<T> Fail(T? response, params string[] messages) => new(OperationStatus.Fail, response, messages);
    public static OperationResult<T> InternalFail(T? response, params string[] messages) => new(OperationStatus.InternalFail, response, messages);
    public static OperationResult<T> BadRequest(T? response, params string[] messages) => new(OperationStatus.BadRequest, response, messages);
    public static OperationResult<T> Processing(T? response, params string[] messages)
    {
        var allMessages = messages.ToList();
        allMessages.Add("Your operation is still processing. Check for it results later");
        
        return new OperationResult<T>(OperationStatus.Processing, response, allMessages);
    }
}
