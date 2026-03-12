using Infrastructure.Persistence.EntityConfigurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAIS.Domain.Users;

namespace Infrastructure.Persistence.EntityConfigurations.Entitys;

public class UserEntityConfig :  BaseIdEntityConfig<UserEntity>
{
    private string Table { get; }
    
    public UserEntityConfig()
    {
        Table = "users";
        Schema = "public";
        Prefix = "user_";
    }

    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable(Table, Schema);
        
        base.Configure(builder);

        builder
            .Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired();
        
        builder
            .Property(u => u.PasswordHashed)
            .HasColumnName("password_hashed")
            .IsRequired();
        
        builder
            .Property(u => u.Username)
            .HasColumnName("username")
            .IsRequired();
        
        builder
            .Property(u => u.FirstName)
            .HasColumnName("first_name")
            .IsRequired();
        
        builder
            .Property(u => u.LastName)
            .HasColumnName("last_name")
            .IsRequired();
        
        builder
            .Property(u => u.Role)
            .HasColumnName("role_id")
            .IsRequired();
        
        builder
            .Property(u => u.LoginAttempts)
            .HasColumnName("login_attempts")
            .IsRequired()
            .HasDefaultValue(0);
        
        builder
            .Property(u => u.IsBlocked)
            .HasColumnName("is_blocked")
            .IsRequired()
            .HasDefaultValue(false);
        
        builder
            .Property(u => u.InstituteUuid)
            .HasColumnName("fk_institute_guid")
            .IsRequired();
        
        builder
            .Property(u => u.State)
            .HasColumnName("state")
            .IsRequired();
        
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