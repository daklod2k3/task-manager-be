namespace server.Entities;

public class DepartmentUser
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UserId { get; set; }

    public long? DepartmentId { get; set; }
    public EDepartmentOwnerType OwnerType { get; set; } = EDepartmentOwnerType.Member;

    public virtual Department? Department { get; set; }

    public virtual Profile? User { get; set; }
}