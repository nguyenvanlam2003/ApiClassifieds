using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        // khai báo thực thể
        DbSet<User> Users { get; set; }
        DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Domain.Entities.Attribute> Attributes { get; set; }
        DbSet<PostReview> PostReviews { get; set; }
        DbSet<SavedPost> savedPosts { get; set; }
        // lưu bất đồng bộ
        Task<int> SaveChangesAsync();
    }
}
