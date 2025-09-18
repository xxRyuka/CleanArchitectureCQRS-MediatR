using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Services;
using MediatR;

namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Command.UpdateCar;

public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, Result>
{
    private readonly ICarService _carService;

    public UpdateCarCommandHandler(ICarService carService)
    {
        _carService = carService;
    }

    public async Task<Result> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
         return await _carService.UpdateCar(request, cancellationToken);
        
    }
}