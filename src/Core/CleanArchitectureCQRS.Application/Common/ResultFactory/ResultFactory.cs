namespace CleanArchitectureCQRS.Application.Common.ResultFactory;

public class ResultFactory : IResultFactory
{
    public Result CreateFailure(IEnumerable<Error> errors)
    {
        return Result.Failure(ResultStatus.ValidationError, errors);
    }

    public Result<T> CreateFailureGeneric<T>(IEnumerable<Error> errors)
    {
        return Result<T>.Failure(ResultStatus.ValidationError, errors);
    }
}