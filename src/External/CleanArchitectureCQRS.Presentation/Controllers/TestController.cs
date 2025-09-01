
using CleanArchitectureCQRS.Domain.Entities;
using CleanArchitectureCQRS.Persistence.Context;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureCQRS.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public TestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("TestController is working!");
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        Car newCar = new Car
        { 
            Model = "Camry",
            Name = "das",
            EnginePower = 250,
        };
        
        _context.Cars.Add(newCar);
        await _context.SaveChangesAsync();
        return Ok(newCar);
    }
}