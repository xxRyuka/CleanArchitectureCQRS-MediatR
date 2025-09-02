using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Dtos;
using MediatR;

namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;

public record CreateCarCommand(string Name, string Model, int EnginePower) : IRequest<Result<CreateCarResponse>>;