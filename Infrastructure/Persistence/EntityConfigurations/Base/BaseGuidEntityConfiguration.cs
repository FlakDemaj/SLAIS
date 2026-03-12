using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAIS.Domain.Commom;

namespace Infrastructure.Persistence.EntityConfigurations.Base;

public class BaseGuidEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseGuidEntity
{
    protected string Schema;
    protected string Prefix;

    public BaseGuidEntityConfiguration()
    {
        Schema = "public";
        Prefix = string.Empty;
    }
    
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .Property(p => p.Guid)
            .IsRequired()
            .HasColumnName(Prefix + "guid");

        builder.HasKey(p => p.Guid);
    }
}