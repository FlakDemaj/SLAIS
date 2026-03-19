namespace SLAIS.Domain.Commom;

public abstract class BaseGuidEntity
{
    public Guid Guid { get; private set; }

    protected BaseGuidEntity()
    {
        Guid = Guid.CreateVersion7();
    }
}
