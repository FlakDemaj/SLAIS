using Domain.System.RefreshToken;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys.System.RefreshTokenEntityConfig;

internal static class RefreshTokenEntityForeignKeyExtension
{
    internal static void AddForeignKeys(this EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder
            .HasOne(rt => rt.User)
            .WithMany(rt => rt.RefreshTokens)
            .HasForeignKey(rt => rt.UserGuid)
            .IsRequired();
    }
}
