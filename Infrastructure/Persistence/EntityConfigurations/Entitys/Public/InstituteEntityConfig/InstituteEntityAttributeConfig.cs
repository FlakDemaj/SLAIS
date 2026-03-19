using Domain.Institutes;

using Infrastructure.Persistence.EntityConfigurations.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys;

internal sealed class InstituteEntityAttributeConfig : BaseIdEntityConfig<InstituteEntity>
{
    private string Table { get; }

    public InstituteEntityAttributeConfig()
    {
        _prefix = "institute_";
        Table = "institutes";
        _schema = "public";
    }

    public override void Configure(EntityTypeBuilder<InstituteEntity> builder)
    {
        builder.ToTable(Table, _schema);

        base.Configure(builder);

        builder
            .Property(i => i.Name)
            .HasColumnName("name")
            .IsRequired();

        builder
            .Property(i => i.Branch)
            .HasColumnName("branch")
            .IsRequired();

        builder.AddForeignKeys();
        builder.AddIndexes();
    }
}
