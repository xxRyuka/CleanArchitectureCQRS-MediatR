using CleanArchitectureCQRS.Persistence.Context;
using CleanArchitectureCQRS.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureCQRS.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {


        services.AddScoped<AuditLogInterceptor>();
        
        services.AddDbContext<AppDbContext>((sp,opt ) =>
        {
            var interceptor = sp.GetRequiredService<AuditLogInterceptor>();
            opt.AddInterceptors(interceptor);
            opt.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        });
        return services;
    }    
}