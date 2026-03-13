using Domain.Institutes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys;

public static class InstituteEntityForeignKeysExtension
{
    public static void AddForeignKeys(this EntityTypeBuilder<InstituteEntity> builder)
    {
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