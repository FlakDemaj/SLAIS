using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAIS.Domain.Users;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys.UserEntityConfig;

public static class UserEntityForeignKeysExtension
{
    public static void AddForeignKeys(this EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasOne(u => u.CreatedByUser)           
            .WithMany(u => u.CreatedUsers)
            .HasForeignKey(u => u.CreatedByUserGuid)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(u => u.UpdatedByUser)
            .WithMany(u => u.UpdatedUsers)
            .HasForeignKey(u => u.UpdatedByUserGuid)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(u => u.DeletedByUser)
            .WithMany(u => u.DeletedUsers)
            .HasForeignKey(u => u.DeletedByUserGuid)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(i => i.Institute)
            .WithMany(i => i.Users)
            .HasForeignKey(u => u.InstituteUuid)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}