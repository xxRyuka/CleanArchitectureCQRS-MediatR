using CleanArchitectureCQRS.Domain.Abstract;

namespace CleanArchitectureCQRS.Domain.Entities;

public sealed class Car : BaseEntity<int>,IAuditEntity
{
    public string Name { get; set; }
    public string Model { get; set; }
    public int EnginePower { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}