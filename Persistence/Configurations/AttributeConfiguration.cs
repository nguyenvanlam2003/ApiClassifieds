using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Domain.Entities.Attribute>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Attribute> builder)
        {
            // cấu hình cho Attribute
            builder.ToTable("Attributes");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.Name).IsRequired().HasColumnType("NVARCHAR(max)");
            builder.Property(a => a.Value).IsRequired().HasColumnType("NVARCHAR(max)");
            builder.Property(a => a.CreatedAt);
            builder.Property(a => a.UpdatedAt);
            builder.Property(a => a.PostId).IsRequired();
            // cấu hình quan hệ với Post
            builder.HasOne(a => a.Post)
                .WithMany()
                .HasForeignKey(a => a.PostId)
                .OnDelete(DeleteBehavior.Cascade); // xóa bản ghi cha thì sẽ xóa bản ghi con
        }
    }
}
