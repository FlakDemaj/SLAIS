using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SLAIS.Domain.Commom;

namespace Infrastructure.Persistence.EntityConfigurations.Base;

public class BaseUpdatedByEntityConfig<T> : BaseCreatedByEntityConfig<T>
    where T : BaseUpdatedByEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder
            .Property(p => p.UpdateDate)
            .HasColumnName("updated_at");

        builder
            .Property(p => p.UpdatedByUserGuid)
            .HasColumnName("updated_by_user_guid");
    }
}
