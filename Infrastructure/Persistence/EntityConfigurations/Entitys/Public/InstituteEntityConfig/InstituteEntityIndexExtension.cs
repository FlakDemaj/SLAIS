using Domain.Institutes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys;

internal static class InstituteEntityIndexExtension
{
    public static void AddIndexes(this EntityTypeBuilder<InstituteEntity> builder)
    {
        builder.HasIndex(user => user.CreatedByUserGuid)
            .HasDatabaseName("idx_institutes_created_by");

        builder.HasIndex(user => user.UpdatedByUserGuid)
            .HasDatabaseName("idx_institutes_updated_by");

        builder.HasIndex(user => user.DeletedByUserGuid)
            .HasDatabaseName("idx_institutes_deleted_by");
    }
}
