using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Domain.Entities.Category>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Category> builder)
        {
            // cấu hình cho Category
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired().HasColumnType("NVARCHAR(max)");
            builder.Property(c => c.Description).HasColumnType("NVARCHAR(max)");
            builder.Property(c => c.ParentId);
            builder.Property(c => c.CreatedAt);
            builder.Property(c => c.UpdatedAt); 
        }
    }
}
