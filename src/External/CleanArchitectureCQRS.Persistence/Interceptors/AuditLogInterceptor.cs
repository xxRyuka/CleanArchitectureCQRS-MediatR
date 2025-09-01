using CleanArchitectureCQRS.Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitectureCQRS.Persistence.Interceptors;

public class AuditLogInterceptor : SaveChangesInterceptor
{
    
    private static readonly Dictionary<EntityState,Action<EntityEntry>> _behaviors = new()
    {
        { EntityState.Added, AddBehavior},
        { EntityState.Modified,UpdateBehavior }
    };
    
    public static void AddBehavior(EntityEntry entry)
    {
        entry.Property(nameof(IAuditEntity.CreatedDate)).CurrentValue = DateTime.UtcNow;
        entry.Property(nameof(IAuditEntity.UpdatedDate)).IsModified = false;
    }

    public static void UpdateBehavior(EntityEntry entry)
    {
        entry.Property(nameof(IAuditEntity.UpdatedDate)).CurrentValue = DateTime.UtcNow;
        entry.Property(nameof(IAuditEntity.CreatedDate)).IsModified = false;
    }
    
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var context = eventData.Context;
        foreach (var entityEntry in context!.ChangeTracker.Entries())
        {
            if (entityEntry.Entity is not IAuditEntity auditEntity) continue;
       
            if (_behaviors.TryGetValue(entityEntry.State, out var behavior))
            {
                behavior(entityEntry);
            }
            
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}