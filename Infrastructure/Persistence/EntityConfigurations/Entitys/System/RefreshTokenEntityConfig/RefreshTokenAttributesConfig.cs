using Domain.Systems.RefreshToken;
using Infrastructure.Persistence.EntityConfigurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys.System.RefreshTokenEntityConfig;

public class RefreshTokenAttributesConfig : BaseGuidEntityConfig<RefreshTokenEntity>
{
    private string Table { get; set; }

    public RefreshTokenAttributesConfig()
    {
        Table = "refresh_tokens";
        Schema = "public";
        Prefix = "refresh_token_";
    }

    public override void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable(Table, Schema);
        
        base.Configure(builder);
        
        builder
            .Property()
        
    }
}