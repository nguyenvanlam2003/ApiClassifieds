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
    public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            // Cấu hình entity PasswordResetToken
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            // Cấu hình các thuộc tính
            builder.Property(t => t.UserId);
            builder.Property(t => t.OTP).IsRequired().HasMaxLength(50);
            builder.Property(t => t.ExpiryDate).IsRequired();
            builder.Property(t => t.IsUsed).IsRequired().HasDefaultValue(false);
            builder.Property(t => t.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(t => t.Email).HasMaxLength(200);
            // Cấu hình khóa ngoại
            builder.HasOne(t => t.User)
                   .WithMany()
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
