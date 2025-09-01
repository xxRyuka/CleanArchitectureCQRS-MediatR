namespace CleanArchitectureCQRS.Domain.Abstract;

public interface IAuditEntity
{
    public DateTime CreatedDate { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
}