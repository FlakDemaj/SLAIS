namespace SLAIS.Domain.Commom;

public abstract class BaseIdEntity : BaseDeleteByEntity
{
    public int? Id { get; init; }

    protected BaseIdEntity(
        Guid createdByUserGuid)
        : base(createdByUserGuid)
    {
        Id = null;
    }
}
