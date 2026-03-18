namespace Domain.Institutes;

public class InstituteEntity : InstituteNavigationPropertyEntity
{

    public string Name { get; private set; }

    public string Branch { get; private set; }

    private InstituteEntity(
        Guid createdByUserGuid,
        string name,
        string branch)
        : base(createdByUserGuid)
    {
        Name = name;
        Branch = branch;
    }
}
