using Domain.System.RefreshToken;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys.System.RefreshTokenEntityConfig;

internal static class RefreshTokenEntityIndexExtension
{
    internal static void AddIndexes(this EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder
            .HasIndex(rt => rt.UserGuid)
            .HasDatabaseName("idx_refresh_tokens_user_guid");
    }
}
