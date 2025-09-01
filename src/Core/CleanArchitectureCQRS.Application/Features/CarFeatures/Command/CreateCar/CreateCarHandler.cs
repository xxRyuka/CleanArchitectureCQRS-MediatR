using CleanArchitectureCQRS.Application.Common;
using MediatR;

namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;

public class CreateCarHandler : IRequestHandler<CreateCarCommand, Result<int>>
{
    
    public Task<Result<int>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}