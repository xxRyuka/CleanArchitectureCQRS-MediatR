namespace CleanArchitectureCQRS.Application.Common.ResultFactory;

public interface IResultFactory
{
    Result CreateFailure(IEnumerable<Error> errors);
    Result<T> CreateFailureGeneric<T>(IEnumerable<Error> errors);
}