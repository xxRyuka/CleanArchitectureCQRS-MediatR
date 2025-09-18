using CleanArchitectureCQRS.Application.Behaviors;
using CleanArchitectureCQRS.Application.DI;
using CleanArchitectureCQRS.Application.Features.CarFeatures.Command.CreateCar;
using CleanArchitectureCQRS.Application.Services;
using CleanArchitectureCQRS.Persistence;
using CleanArchitectureCQRS.Persistence.Service;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Application servisleri
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddApplicationServices(builder.Configuration);
// Controllers
builder.Services.AddControllers()
    .AddApplicationPart(typeof(CleanArchitectureCQRS.Presentation.PresentationAssembly).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Validatorlar
builder.Services.AddValidatorsFromAssemblyContaining<CreateCarCommandValidator>();

// MediatR + Pipeline Behaviors
builder.Services.AddMediatR(opt =>
{
    // Application assembly
    opt.RegisterServicesFromAssemblies(
        typeof(CleanArchitectureCQRS.Application.ApplicationAssembly).Assembly,
        typeof(ValidationBehavior<,>).Assembly // burada Behavior assembly’si de ekleniyor
    );
});

// Pipeline behavior kaydı
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Persistence
builder.Services.AddPersistanceServices(builder.Configuration);

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();