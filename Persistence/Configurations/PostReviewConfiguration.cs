using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class PostReviewConfiguration : IEntityTypeConfiguration<Domain.Entities.PostReview>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Entities.PostReview> builder)
        {
            // cấu hình cho PostReview
            builder.ToTable("PostReviews");
            builder.HasKey(pr => pr.Id);
            builder.Property(pr => pr.Id).ValueGeneratedOnAdd();
            builder.Property(pr => pr.Comment).HasColumnType("NVARCHAR(max)");
            builder.Property(pr => pr.Status).HasMaxLength(50);
            builder.Property(pr => pr.CreatedAt);
            builder.Property(pr => pr.UpdatedAt);
            builder.Property(pr => pr.PostId).IsRequired();
            builder.Property(pr => pr.UserId);
            // cấu hình quan hệ với Post
            builder.HasOne(pr => pr.Post)
                .WithMany()
                .HasForeignKey(pr => pr.PostId)
                .OnDelete(DeleteBehavior.Cascade); // xóa bản ghi cha thì sẽ xóa bản ghi con
            // cấu hình quan hệ với User
            builder.HasOne(pr => pr.User)
                .WithMany()
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
