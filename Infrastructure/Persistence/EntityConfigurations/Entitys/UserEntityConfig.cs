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
        builder.ToTable(Schema, Table);
        
        base.Configure(builder);

        builder
            .Property(p => p.Email)
            .HasColumnName("email")
            .IsRequired();
        
        builder
            .Property(p => p.PasswordHashed)
            .HasColumnName("password_hashed")
            .IsRequired();
        
        builder
            .Property(p => p.Username)
            .HasColumnName("username")
            .IsRequired();
        
        builder
            .Property(p => p.FirstName)
            .HasColumnName("first_name")
            .IsRequired();
        
        builder
            .Property(p => p.LastName)
            .HasColumnName("last_name")
            .IsRequired();
        
        builder
            .Property(p => p.Role)
            .HasColumnName("role_id")
            .IsRequired();
        
        builder
            .Property(p => p.LoginAttempts)
            .HasColumnName("login_attempts")
            .IsRequired()
            .HasDefaultValue(0);
        
        builder
            .Property(p => p.IsBlocked)
            .HasColumnName("is_blocked")
            .IsRequired()
            .HasDefaultValue(false);
        
        builder
            .Property(p => p.InstituteUuid)
            .HasColumnName("fk_institute_uuid")
            .IsRequired();
        
        builder
            .Property(p => p.State)
            .HasColumnName("state")
            .IsRequired();
        
        builder
            .HasOne(u => u.CreatedByUser)           
            .WithMany(u => u.CreatedUsers)
            .HasForeignKey(u => u.CreatedByUserGuid)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(u => u.Institute)
            .WithOne()
            .HasForeignKey<UserEntity>(u => u.InstituteUuid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}