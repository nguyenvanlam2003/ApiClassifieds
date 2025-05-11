using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class MyDbContext : DbContext, IApplicationDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        // khai báo DbSet cho các thực thể
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Áp dụng cấu hình từ UserConfiguration
        }
    }
}
