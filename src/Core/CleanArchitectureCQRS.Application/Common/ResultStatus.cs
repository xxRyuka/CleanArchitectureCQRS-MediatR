namespace CleanArchitectureCQRS.Application.Common;

public enum ResultStatus
{
    Success,
    Created,
    NoContent,
    NotFound,
    ValidationError,
    Error,
    UnAuthorized,
    InternalServerError,
}