
---

# `src/CleanArchitectureCQRS.Presentation/README.md`
```markdown
# Presentation (Controllers / DTO / API Layer)

## Amaç
HTTP/Transport yüzeyini sağlar. Controller'lar, request/response DTO'ları, API-specific concerns burada.

## Sorumluluklar
- Controllers / Endpoints
- Request DTOs (ex: `ProductCreateRequest`)
- Presentation-level validation attributes (ModelState) ve mapping (Request -> Command)
- Response mapping yapılmazsa Application DTO'sunu direk dönebilir.

## Önemli Dosyalar
- `Controllers/ProductsController.cs`
- `DTOs/ProductCreateRequest.cs`, `DTOs/ProductResponse.cs`
- `Mapping/PresentationMappingProfile.cs` (Request -> Command mapping)
- `Filters/ApiExceptionFilter.cs` (opsiyonel)
- `Extensions/PresentationServiceRegistration.cs`



## Mapping kuralı (net)
- **PresentationMappingProfile**: `Request` → `CreateProductCommand` gibi sadece request→command map'leri içerir.
- Presentation, Application DTO'larına doğrudan bağlı olmamalıdır. Controller -> Mediator send için mapping yeterli.

````
## Örnek Controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateRequest req)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var command = _mapper.Map<CreateProductCommand>(req);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess) return Problem(detail: result.Error, statusCode: result.StatusCode ?? 400);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }
}
