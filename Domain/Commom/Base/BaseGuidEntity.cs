namespace SLAIS.Domain.Commom;

public abstract class BaseGuidEntity
{
    public Guid Guid { get; private set; }

    public void SetGuid(Guid guid)
    {
        Guid = guid;
    }
}
