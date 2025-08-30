
---

# `test/CleanArchitectureCQRS.UnitTest/README.md`
# Tests (Unit / Integration)
## Amaç
```markdown
Unit ve integration test'leri barındırır. Öğretici senaryolar: handler testleri, domain invariant testleri, persistence integration testleri.
````

## Sorumluluklar
````
- Unit tests: Application handlers, Domain logic, mapping profilleri
- Integration tests: AppDbContext (InMemory veya real test db), repository implementasyonları
- Mapping tests: PresentationMappingProfileTests, ApplicationMappingProfileTests
````
## Örnek testler
````
- `Features/Products/CreateProductHandlerTests.cs` — handler success/failure senaryoları
- `Mapping/PresentationMappingProfileTests.cs` — request->command map doğrulama
- `Integration/ProductPersistenceTests.cs` — migration ve CRUD senaryoları
````

## Test araçları
````
- xUnit / NUnit / MSTest (tercih xUnit)
- FluentAssertions
- Microsoft.EntityFrameworkCore.InMemory (integration için)
````
