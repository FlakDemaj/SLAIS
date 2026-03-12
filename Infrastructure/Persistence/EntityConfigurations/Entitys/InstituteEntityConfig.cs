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
       
       builder
           .HasOne(u => u.CreatedByUser)           
           .WithMany(u => u.CreatedInstitutes)
           .HasForeignKey(u => u.CreatedByUserGuid)
           .OnDelete(DeleteBehavior.Restrict);
        
       builder
           .HasOne(u => u.UpdatedByUser)
           .WithMany(u => u.UpdatedInstitute)
           .HasForeignKey(u => u.UpdatedByUserGuid)
           .OnDelete(DeleteBehavior.Restrict);
        
       builder
           .HasOne(u => u.DeletedByUser)
           .WithMany(u => u.DeletedInstitute)
           .HasForeignKey(u => u.DeletedByUserGuid)
           .OnDelete(DeleteBehavior.Restrict);
       
       builder
           .HasMany(i => i.Users)
           .WithOne(u => u.Institute)
           .HasForeignKey(u => u.InstituteUuid)
           .OnDelete(DeleteBehavior.Restrict);
    }
}