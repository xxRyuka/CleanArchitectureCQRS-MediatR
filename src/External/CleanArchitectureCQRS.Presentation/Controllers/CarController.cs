using CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Command.UpdateCar;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureCQRS.Presentation.Controllers;

public class CarController : ApiController
{
    public CarController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar(CreateCarCommand createCarCommand, CancellationToken cancellationToken) =>
        ResultedAction(await _mediator.Send(createCarCommand, cancellationToken));
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCar(UpdateCarCommand updateCarCommand, CancellationToken cancellationToken) =>
        ResultedAction(await _mediator.Send(updateCarCommand, cancellationToken));
}