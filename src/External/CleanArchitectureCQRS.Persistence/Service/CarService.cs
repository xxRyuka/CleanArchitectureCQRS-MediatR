using AutoMapper;
using CleanArchitectureCQRS.Application.Common;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Command.UpdateCar;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Dtos;
using CleanArchitectureCQRS.Application.Services;
using CleanArchitectureCQRS.Domain.Entities;
using CleanArchitectureCQRS.Persistence.Context;

namespace CleanArchitectureCQRS.Persistence.Service;

public class CarService : ICarService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public CarService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
        // CreateCarResponse dto donmeli miyim ? yes sir 
        return Result<CreateCarResponse>.Success(_mapper.Map<CreateCarResponse>(car));
    }
    
    public async Task<Result> UpdateCar(UpdateCarCommand command, CancellationToken cancellationToken)
    {
        var car = _context.Cars.Find(command.carId);
        if (car == null)
        {
            return Result.Failure(ResultStatus.NotFound, new List<Error>());
        }
        car.Name = command.Name;
        car.Model = command.Model;
        car.EnginePower = command.EnginePower;
        _context.Cars.Update(car);
       await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}