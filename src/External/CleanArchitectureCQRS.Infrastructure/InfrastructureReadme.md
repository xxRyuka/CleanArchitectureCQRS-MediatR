
---

# `src/CleanArchitectureCQRS.Infrastructure/README.md`
```markdown
# Infrastructure (External Adapters)

## Amaç
E-posta servisleri, dosya storage, 3rd-party API client'ları gibi dış servis adaptörleri.

## Sorumluluklar
- Implementasyonlar: IEmailSender -> SmtpEmailSender
- HTTP clients (HttpClientFactory)
- Feature toggles / external configs

## Önemli Dosyalar
- `Services/EmailSender.cs`
- `Clients/ExternalApiClient.cs`
````



# Neden

Infrastructure, uygulama ihtiyaçlarına çözüm getirir; Application tarafındaki interface'lere implementasyon sağlar.



````
## DI örneği
```csharp
public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
{
    services.AddHttpClient<IExternalApiClient, ExternalApiClient>(c =>
    {
        c.BaseAddress = new Uri(cfg["ExternalApi:BaseUrl"]);
    });
    services.AddTransient<IEmailSender, SmtpEmailSender>();
    return services;
}
