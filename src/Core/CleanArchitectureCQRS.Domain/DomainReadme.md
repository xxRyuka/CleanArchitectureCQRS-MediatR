
---

# `src/CleanArchitectureCQRS.Domain/README.md`
```markdown
# Domain (Entities / Business Rules)

## Amaç
Temel iş kuralları, entity'ler, value objects ve domain event'ler burada.
Hiçbir dış bağımlılık (EF, AutoMapper, MediatR) olmamalı.

## Sorumluluklar
- Entities (Aggregate Roots)
- ValueObjects (örn. Money)
- DomainExceptions, Domain Services (stateless)
- Domain Events

## Önemli Dosyalar
- `Entities/Product.cs`
- `ValueObjects/Money.cs`
- `Exceptions/DomainException.cs`
- `Events/ProductCreatedEvent.cs`


````
````
# Neden

- Domain saf iş mantığını korur; testler domain kurallarını doğrudan validate eder.
````
## Örnek Entity (factory & invariants)
```csharp
public class Product
{
//bu Domain-Driven Design örneği, (yani simdilik asagiyi gorme ama ogrencez bunuda)
//Product entity'sinin nasıl oluşturulacağını ve iş kurallarını (invariants) nasıl uygulayacağını gösterir.
    public int Id { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }

    private Product() { }

    public static Product Create(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name required");
        if (price <= 0) throw new DomainException("Price must be > 0");
        return new Product { Name = name, Price = new Money(price) };
    }
}
