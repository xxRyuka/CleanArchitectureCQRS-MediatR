using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Common.ResultFactory;
using FluentValidation;
using MediatR;

namespace CleanArchitectureCQRS.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : Common.Result

{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IResultFactory _resultFactory;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IResultFactory resultFactory)
    {
        _validators = validators;
        _resultFactory = resultFactory;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(v => v.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
            {
                List<Error> errors = failures.Select(f =>
                {
                    return new Error()
                    {
                        Code = f.ErrorCode,
                        Message = f.ErrorMessage
                    };
                }).ToList();

                //Result Factory ile deneyelim 

                if (typeof(TResponse).IsGenericType &&
                    typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var genericArg = typeof(TResponse).GetGenericArguments()[0];

                    // dynamic cast ile generic overload çağırılabilir
                    var failureResult = typeof(IResultFactory)
                        .GetMethod(nameof(IResultFactory.CreateFailureGeneric))!
                        .MakeGenericMethod(genericArg)
                        .Invoke(_resultFactory, new object[] { errors });

                    return (TResponse)failureResult!;
                }


                else
                {
                    return (TResponse)(object)_resultFactory.CreateFailure(errors);
                }

                // Reflecion ama bu sistemi yoracaktir 
                // Generic Result<T>
                // if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                // {
                //     var genericArg = typeof(TResponse).GetGenericArguments()[0];
                //     // TResponse zaten Result<T>, doğrudan cast ile dönüyoruz
                //     var failureResultType = typeof(Result<>).MakeGenericType(genericArg);
                //     var failureResult = Activator.CreateInstance(
                //         failureResultType,
                //         new object[] { ResultStatus.ValidationError, errors }
                //     );
                //     return (TResponse)failureResult!;
                // }
                // else
                // {
                //     // Non-generic Result
                //     var result = Result.Failure(ResultStatus.ValidationError, errors);
                //     return (TResponse)(object)result;
                // }
            }
        }

        return await next();
    }
}