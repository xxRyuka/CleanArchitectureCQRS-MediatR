
---

# `src/CleanArchitectureCQRS.Persistence/README.md`
```markdown
# Persistence (EF Core / DbContext / Migrations)

## Amaç
Veritabanı erişimini yönetir: `AppDbContext`, entity configurations (Fluent API), migration'lar ve repository implementasyonları.

````

## Sorumluluklar
````
- `AppDbContext : DbContext`
- EntityTypeConfiguration sınıfları (`Configurations/ProductConfiguration.cs`)
- Repository implementasyonları (IProductRepository -> EfProductRepository)
- Migrations
````
## Önemli Dosyalar
````
- `AppDbContext.cs`
- `Configurations/*.cs` (Fluent API)
- `Repositories/EfProductRepository.cs`
- 
````

Neden
````
Persistence uygulama dış sınırıdır. Application yalnızca repository interface'lerini kullanır; implementasyon burada kalır.
````

## Örnek DbContext kayıt (DI)
```csharp
public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration cfg)
{
    services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(cfg.GetConnectionString("Default")));
    services.AddScoped<IProductRepository, EfProductRepository>();
    return services;
}
