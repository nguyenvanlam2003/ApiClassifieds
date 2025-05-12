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
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // cấu hình cho Post
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Address).HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnType("DECIMAL(18,2)");
            builder.Property(p => p.Description).HasColumnType("NVARCHAR(max)");
            builder.Property(p => p.Image).HasColumnType("NVARCHAR(max)");
            builder.Property(p => p.Status).HasMaxLength(50);
            builder.Property(p => p.ViewCount).HasDefaultValue(0);
            builder.Property(p => p.CreatedAt);
            builder.Property(p => p.UpdatedAt);
            builder.Property(p => p.UserId);
            builder.Property(p => p.CategoryId);
            // cấu hình quan hệ với User
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            // cấu hình quan hệ với Category
            builder.HasOne(p => p.Category).WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
