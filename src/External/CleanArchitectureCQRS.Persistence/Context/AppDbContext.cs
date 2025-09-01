using CleanArchitectureCQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureCQRS.Persistence.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);
        
    }
}