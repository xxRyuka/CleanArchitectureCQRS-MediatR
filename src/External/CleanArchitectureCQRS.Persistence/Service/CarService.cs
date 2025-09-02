using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Dtos;
using CleanArchitectureCQRS.Application.Services;
using CleanArchitectureCQRS.Domain.Entities;
using CleanArchitectureCQRS.Persistence.Context;

namespace CleanArchitectureCQRS.Persistence.Service;

public class CarService : ICarService
{
    private readonly AppDbContext _context;

    public CarService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateCarResponse>> CreateCar(CreateCarCommand command,CancellationToken cancellationToken )
    {
        var car = new Car
        {
            Name = command.Name,
            Model = command.Model,
            EnginePower = command.EnginePower
        };
        await _context.Cars.AddAsync(car, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken); 
        
        // CreateCarResponse dto donmeli miyim ? 
        return Result<CreateCarResponse>.Success(new CreateCarResponse(car.Id, car.Name, car.Model, car.EnginePower));
    }
}