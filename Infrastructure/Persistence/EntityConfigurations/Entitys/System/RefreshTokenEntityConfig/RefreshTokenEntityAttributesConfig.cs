using Domain.Systems.RefreshToken;

using Infrastructure.Persistence.EntityConfigurations.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys.System.RefreshTokenEntityConfig;

internal sealed class RefreshTokenEntityAttributesConfig : BaseGuidEntityConfig<RefreshTokenEntity>
{
    private string Table { get; }

    public RefreshTokenEntityAttributesConfig()
    {
        Table = "refresh_tokens";
        _schema = "system";
        _prefix = "refresh_token_";
    }

    public override void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable(Table, _schema);

        base.Configure(builder);

        builder
            .Property(rt => rt.RefreshToken)
            .HasColumnName("refresh_token");

        builder
            .Property(rt => rt.ExpirationDate)
            .HasColumnName("expiration_date")
            .IsRequired();

        builder
            .Property(rt => rt.DeviceGuid)
            .HasColumnName("device_id")
            .IsRequired();

        builder
            .Property(rt => rt.DeviceName)
            .HasColumnName("device_name")
            .IsRequired();

        builder
            .Property(rt => rt.IpAddress)
            .HasColumnName("ip_address")
            .IsRequired();

        builder
            .Property(rt => rt.Revoked)
            .HasColumnName("revoked")
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(rt => rt.RevokedDate)
            .HasColumnName("revoked_date");

        builder
            .Property(rt => rt.CreatedDate)
            .HasColumnName("created_date")
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder
            .Property(rt => rt.LastUsedDate)
            .HasColumnName("last_used_date")
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder
            .Property(rt => rt.UserGuid)
            .HasColumnName("fk_user_guid")
            .IsRequired();

        builder.AddForeignKeys();
        builder.AddIndexes();


    }
}
