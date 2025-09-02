using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Dtos;
using CleanArchitectureCQRS.Application.Services;
using MediatR;

namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;

public class CreateCarHandler : IRequestHandler<CreateCarCommand, Result<CreateCarResponse>>
{
    private readonly ICarService _carService;

    public CreateCarHandler(ICarService carService)
    {
        _carService = carService;
    }

    public async Task<Result<CreateCarResponse>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
       return await _carService.CreateCar(request,cancellationToken);
    }
}