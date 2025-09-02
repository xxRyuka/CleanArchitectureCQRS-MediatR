namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Dtos;

public record CreateCarResponse(
    int Id,
    string Name,
    string Model,
    int EnginePower);