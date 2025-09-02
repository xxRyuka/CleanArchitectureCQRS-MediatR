using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Dtos;

namespace CleanArchitectureCQRS.Application.Services;

public interface ICarService
{
    Task<Result<CreateCarResponse>> CreateCar(CreateCarCommand command,CancellationToken cancellationToken );
    
    
}