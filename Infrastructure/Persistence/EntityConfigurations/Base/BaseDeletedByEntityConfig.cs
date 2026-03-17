using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SLAIS.Domain.Commom;

namespace Infrastructure.Persistence.EntityConfigurations.Base;

public class BaseDeletedByEntityConfig<T> : BaseUpdatedByEntityConfig<T>
    where T : BaseDeleteByEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder
            .Property(p => p.DeleteDate)
            .HasColumnName("deleted_at");

        builder
            .Property(p => p.DeletedByUserGuid)
            .HasColumnName("deleted_by_user_guid");
    }
}
