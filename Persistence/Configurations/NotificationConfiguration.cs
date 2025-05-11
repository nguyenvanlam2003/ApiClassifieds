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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Cấu hình entity Notification
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).ValueGeneratedOnAdd();
            // Cấu hình các thuộc tính
            builder.Property(n => n.Title).IsRequired().HasMaxLength(500);
            builder.Property(n => n.Message).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(n => n.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(n => n.IsRead).IsRequired().HasDefaultValue(false);
            // Cấu hình khóa ngoại
            builder.HasOne(n => n.User)
                   .WithMany()
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
