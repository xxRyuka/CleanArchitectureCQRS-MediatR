using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Dtos;
using MediatR;

namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Command.UpdateCar;

public record UpdateCarCommand(int carId,string Name, string Model, int EnginePower) : IRequest<Result>;
