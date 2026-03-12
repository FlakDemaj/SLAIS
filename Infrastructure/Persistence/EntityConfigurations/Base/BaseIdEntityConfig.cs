using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAIS.Domain.Commom;

namespace Infrastructure.Persistence.EntityConfigurations.Base;

public class BaseIdEntityConfig<T> : BaseDeletedByEntityConfig<T>
    where T : BaseIdEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder
            .Property(p => p.Id)
            .HasColumnName(Prefix + "id");
    }
}