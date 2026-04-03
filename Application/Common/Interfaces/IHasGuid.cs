namespace Application.Common.Interfaces;

public interface IHasGuid
{
    int PublicId { get; }

    Guid Guid { get; set; }

    string EntityType { get; }
}
