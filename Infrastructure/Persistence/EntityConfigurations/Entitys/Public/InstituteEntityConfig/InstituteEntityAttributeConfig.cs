using Domain.Institutes;
using Infrastructure.Persistence.EntityConfigurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys;

public class InstituteEntityAttributeConfig : BaseIdEntityConfig<InstituteEntity>
{
    private string Table  { get; init; }
    
    public InstituteEntityAttributeConfig()
    {
        Prefix = "institute_";
        Table = "institutes";
        Schema = "public";
    }

    public override void Configure(EntityTypeBuilder<InstituteEntity> builder)
    {
       builder.ToTable(Table, Schema);
        
       base.Configure(builder);
        
       builder
           .Property(i => i.Name)
           .HasColumnName("name")
           .IsRequired();
       
       builder
           .Property(i => i.Branch)
           .HasColumnName("branch")
           .IsRequired();
       
       builder.AddForeignKeys();
       builder.AddIndexes();
    }
}