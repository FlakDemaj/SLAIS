using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAIS.Domain.Commom;
using SAIS.Domain.Users;

namespace Infrastructure.Persistence.EntityConfigurations.Base;

public class BaseCreatedByEntityConfig<T> : BaseGuidEntityConfig<T>
    where T : BaseCreatedByEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    { 
        base.Configure(builder);

        builder
            .Property(p => p.CreatedDate)
            .HasColumnName("created_at");
        
        builder
            .Property(p => p.CreatedByUserGuid)
            .HasColumnName("created_by_user_guid");
    }
}