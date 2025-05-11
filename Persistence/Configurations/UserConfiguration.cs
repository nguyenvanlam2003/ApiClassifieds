using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Cấu hình entity User
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(u => u.Role).IsRequired().HasMaxLength(50);
            builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Phone).HasMaxLength(20);
            builder.Property(u => u.Address).HasMaxLength(500);
            builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

            // Cấu hình giá trị cho Role
            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
