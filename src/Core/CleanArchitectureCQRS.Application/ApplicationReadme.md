
---

# `src/CleanArchitectureCQRS.Application/README.md`
```markdown
# Application (CQRS / Use Cases / Mediator)

## Amaç
Use-case'ler, Commands/Queries ve handler'lar bu katmanda.
Application, Domain ile etkileşir ve Persistence/Infrastructure için interface'ler sunar.

## Sorumluluklar
- `Features/<FeatureName>/Commands|Queries|Handlers`
- `DTOs` (use-case seviyesinde gerekiyorsa)
- `Interfaces` (IProductRepository, IUnitOfWork)
- `Common/Result.cs` (Response pattern)
- `Behaviors` (ValidationBehavior, LoggingBehavior, TransactionBehavior)
- `Mapping/ApplicationMappingProfile.cs` (Domain entity -> Application DTO dönüşümleri)
````
````markdown
## Önemli Dosyalar
- `Common/Result.cs` (veya ServiceResult)
- `Interfaces/IProductRepository.cs`
- `Features/Products/Commands/CreateProduct/CreateProductCommand.cs`
- `Features/Products/Commands/CreateProduct/CreateProductCommandHandler.cs`
- `Mapping/ApplicationMappingProfile.cs`
- `Behaviors/ValidationBehavior.cs`
````

```markdown
Validation (FluentValidation)

Validator sınıfları Application içinde yer alır (örn. CreateProductCommandValidator).

ValidationBehavior<TRequest, TResponse> pipeline olarak register edilir.

DI / 
````
````csharp
builder.Services.AddMediatR(...)

builder.Services.AddValidatorsFromAssembly(...)

builder.Services.AddAutoMapper(...) (Application mapping profile assembly)
````
Neden

````
Application, iş akışlarının merkezidir. Handler'lar test edilebilmeli, transaction/validation pipeline easy-to-inject olmalı.
````
## Result<T> örneği (basit)
```csharp
public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    public IEnumerable<ValidationError>? Errors { get; init; }
    public int? StatusCode { get; init; }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Fail(string error, int? status = 400) => new() { IsSuccess = false, Error = error, StatusCode = status };
}

````
## Örnek Command & Handler
```csharp
// CreateProductCommand (Application)
public record CreateProductCommand(string Name, decimal Price) : IRequest<Result<ProductDto>>;

// Handler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    public CreateProductCommandHandler(IProductRepository repo, IMapper mapper) { ... }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        // Domain invariants via factory
        var product = Product.Create(request.Name, request.Price);
        await _repo.AddAsync(product);
        var dto = _mapper.Map<ProductDto>(product); // ApplicationMappingProfile
        return Result<ProductDto>.Success(dto);
    }
}
