namespace CleanArchitectureCQRS.Application.Common;

public class Result
{

    public bool IsSuccess => Status == ResultStatus.Success;
    public ResultStatus Status { get; }
    public IReadOnlyList<Error> Errors { get; }

    public Result(ResultStatus status, IEnumerable<Error> errors)
    {
        Status = status;

        // invariants
        if (status == ResultStatus.Success && errors!.ToList().Count > 0)
            throw new InvalidOperationException("Success result cannot contain errors.");

        if (status != ResultStatus.Success && errors!.ToList().Count == 0)
            throw new InvalidOperationException("Failure result must contain at least one error.");
        Errors = (errors ?? Enumerable.Empty<Error>()).ToList().AsReadOnly();
    }


    public static Result Success() => new Result(ResultStatus.Success, null);
    
    

    public static Result Failure(ResultStatus status, IEnumerable<Error> errors)
    {
        // checking invariants
        if (status == ResultStatus.Success)
            throw new ArgumentException("Failure cannot have Success status.", nameof(status));

        if (errors is null || errors.ToList().Count == 0)
            throw new ArgumentException("Provide at least one error.", nameof(errors));


        return new Result(status, errors);
    }
}

public sealed class Result<T> : Result
{
    public T Data { get; }

    public Result(T data ) : base(ResultStatus.Success, null)
    {
        // checking invariants
        if (data == null)
            throw new ArgumentNullException(nameof(data), "Data cannot be null for a successful result.");
        Data = data;
    }


    public Result(ResultStatus status, IEnumerable<Error> errors) : base(status, errors)
    {
        Data = default!;
    }
    
    public static Result<T> Success(T data) => new Result<T>(data);

    public static Result<T> Failure(ResultStatus status, IEnumerable<Error> errors)
    {
        return new Result<T>(status, errors);
    }
}