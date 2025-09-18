using FluentValidation;

namespace CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Car name is required.")
            .MaximumLength(100).WithMessage("Car name must not exceed 100 characters.");

        RuleFor(c => c.Model)
            .NotEmpty().WithMessage("Car model is required.")
            .MaximumLength(100).WithMessage("Car model must not exceed 100 characters.");

        RuleFor(c => c.EnginePower)
            .GreaterThan(0).WithMessage("Engine power must be greater than zero.");
    }
}