using Domain.Institutes;
using Infrastructure.Persistence.EntityConfigurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys;

public class InstituteEntityConfig : BaseIdEntityConfig<InstituteEntity>
{
    private string Table  { get; init; }

    
    public InstituteEntityConfig()
    {
        Prefix = "institute_";
        Table = "institutes";
        Schema = "public";
    }

    public override void Configure(EntityTypeBuilder<InstituteEntity> builder)
    {
       builder.ToTable(Schema, Table);
        
       base.Configure(builder);
        
       builder
           .Property(e => e.Name)
           .HasColumnName("name")
           .IsRequired();
       
       builder
           .Property(p => p.Branch)
           .HasColumnName("branch")
           .IsRequired();
       
       builder
           .HasMany(p => p.Users)
           .WithOne(p => p.Institute)
           .HasForeignKey(p => p.InstituteUuid)
           .OnDelete(DeleteBehavior.Restrict);
    }
}