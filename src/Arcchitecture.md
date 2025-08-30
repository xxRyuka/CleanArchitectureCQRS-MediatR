# CleanArchitectureCQRS - Mimari Özeti (ARCHITECTURE)

## Amaç
Projenin mimari kararlarını, katman sorumluluklarını, konvansiyonları ve temel akışları merkezi olarak açıklar. Öğretici amaçlı: CQRS + MediatR + Clean Architecture + Result pattern gösterimi.

## Yüksek seviye katmanlar
- `src/CleanArchitectureCQRS.Api` — Host / bootstrap (Program.cs, DI, middleware)
- `src/CleanArchitectureCQRS.Presentation` — Controllers / Endpoints / Request DTOs / HTTP concerns
- `src/CleanArchitectureCQRS.Application` — CQRS (Commands/Queries/Handlers), application DTOs, interfaces, pipeline behaviors
- `src/CleanArchitectureCQRS.Domain` — Entities, ValueObjects, DomainRules, DomainEvents
- `src/CleanArchitectureCQRS.Persistence` — EF Core DbContext, Migrations, repository implementations
- `src/CleanArchitectureCQRS.Infrastructure` — External adapters (email, file storage, third-party clients)
- `test` — Unit & Integration tests

## Temel prensipler (Dependency Rule)
- İç katmanlar dış katmanlara bağımlı değildir. Bağımlılık yönü: Presentation → Application → Domain. Persistence/Infrastructure implementasyonları Application tarafından soyutlanır (interface’ler Application'da).
- Presentation dışa özgü (HTTP), Application use-case'leri yönetir, Domain saf iş kurallarıdır.

## Mapping konvansiyonu (düzeltme & netleştirme)
- **PresentationMappingProfile** (Presentation assembly): `Request` -> `Command` (ve gerekiyorsa `Command` -> `Request`).
    - Çünkü `Request` tipleri HTTP/transport özelidir; Application'a bağımlılık olmamalı.
- **ApplicationMappingProfile** (Application assembly): `Domain Entity` -> `Application DTO` (veya `Application DTO` -> `Domain`).
    - Çünkü use-case çıktısı (DTO) Domain ile içeriden etkileşir.
- AutoMapper veya Mapster kullanılsa bile profilleri kendi katmanlarında tut. `Api`/Host projesi tüm assembly'leri `AddAutoMapper(...)` ile yükler.

## Result / Response pattern
- Application içerisindeki handler'lar `Result<T>` veya `ServiceResult<T>` döndürmeli:
    - `{ bool IsSuccess; T? Value; string? Error; IEnumerable<ValidationError>? Errors; int? StatusCode }`
- Bu Result tipi Application içinde tanımlanır (Application/Common).
- Presentation gelen sonucu HTTP response'a çevirir (Created, Ok, BadRequest, NotFound vs).

## MediatR / Pipeline
- MediatR `IRequest<T>`/`IRequestHandler<T>` kullan.
- Pipeline Behaviors: Validation, Logging, ExceptionHandling, Transaction.
- Validation için FluentValidation (Application içinde validator class'ları, fakat Presentation request validasyonu da yapılabilir).

## Conventions
- Feature-based klasörleme (Application/Features/Products/Commands/CreateProduct)
- DTO'lar: `Presentation` için request/response; `Application` için use-case dto'ları (ihtiyaç varsa).
- Naming: `CreateProductCommand`, `CreateProductCommandHandler`, `ProductDto`, `ProductCreateRequest`.

## Örnek akış (CreateProduct)
1. `ProductsController.Create(ProductCreateRequest)` —> _mapper_ ile `CreateProductCommand`.
2. `_mediator.Send(command)` —> Application handler çalışır.
3. Handler domain entity oluşturur, repository ile persist eder, `ProductDto` döner (`Result<ProductDto>`).
4. Controller `CreatedAtAction(...)` veya `Problem(...)` döner.
