using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
     public class SavedPostConfiguration : IEntityTypeConfiguration<Domain.Entities.SavedPost>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Entities.SavedPost> builder)
        {
            // cấu hình cho SavedPost
            builder.ToTable("SavedPosts");
            builder.HasKey(sp => sp.Id);
            builder.Property(sp => sp.Id).ValueGeneratedOnAdd();
            builder.Property(sp => sp.UserId).IsRequired();
            builder.Property(sp => sp.PostId).IsRequired();
            builder.Property(sp => sp.CreatedAt);
            builder.Property(sp => sp.UpdatedAt);
            // cấu hình quan hệ với User
            builder.HasOne(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // cấu hình quan hệ với Post
            builder.HasOne(sp => sp.Post)
                .WithMany()
                .HasForeignKey(sp => sp.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
