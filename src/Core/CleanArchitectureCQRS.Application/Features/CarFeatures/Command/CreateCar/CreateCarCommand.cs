using CleanArchitectureCQRS.Application.Common;
using MediatR;

namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;

public record CreateCarCommand(int Id, string Name, string Model, int EnginePower) : IRequest<Result<int>>;