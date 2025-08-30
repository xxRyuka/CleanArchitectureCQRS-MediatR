# Api (Host / Bootstrap)

## Amaç
Uygulamayı host eden proje. `Program.cs`, konfigürasyon, logging, swagger, global middleware burada.

## Sorumluluklar
- Hosting (Kestrel), configuration, logging, swagger, health-checks.
- DI container'a diğer katmanların servislerini register etmek (AddPresentation, AddApplication, AddPersistence, AddInfrastructure).
- Global middlewares (Error handling, RequestLogging).

## Önemli Dosyalar
- `Program.cs`
- `appsettings.json` / `appsettings.Development.json`
- `Extensions/ServiceRegistrationExtensions.cs` (modüler servıs register kodu)
- `Middleware/ErrorHandlerMiddleware.cs`

## Örnek Program.cs (kısa)
```csharp
var builder = WebApplication.CreateBuilder(args);

// register katmanlar
builder.Services.AddPresentation();   // Presentation: controllers, swagger
builder.Services.AddApplication();    // Application: MediatR, validators, behaviors
builder.Services.AddPersistence(builder.Configuration); // DbContext, Repos
builder.Services.AddInfrastructure(builder.Configuration); // 3rd party clients

builder.Services.AddAutoMapper(
    typeof(CleanArchitectureCQRS.Presentation.Mapping.PresentationMappingProfile).Assembly,
    typeof(CleanArchitectureCQRS.Application.Mapping.ApplicationMappingProfile).Assembly
);

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();
app.Run();
```

## Serviceregistration
```csharp
public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        // Presentation-specific registrations (filters, policies)
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SomeMarker>());
        services.AddValidatorsFromAssemblyContaining<SomeValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(cfg.GetConnectionString("Default")));
        // repository implementations
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        // HTTP clients, email senders etc.
        return services;
    }
}
