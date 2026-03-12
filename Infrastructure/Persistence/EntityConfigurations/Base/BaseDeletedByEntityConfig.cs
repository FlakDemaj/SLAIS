using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAIS.Domain.Commom;

namespace Infrastructure.Persistence.EntityConfigurations.Base;

public class BaseDeletedByEntityConfig<T> : BaseUpdatedByEntityConfig<T>
    where T : BaseDeleteByEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder
            .Property(p => p.DeleteDate)
            .HasColumnName("deletion_date");

        builder
            .Property(p => p.DeleteByUser)
            .HasColumnName("deletion_by_user_uuid");
    }
}