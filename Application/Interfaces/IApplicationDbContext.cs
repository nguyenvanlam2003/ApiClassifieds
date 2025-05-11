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
        // khai báo thực thể
        DbSet<User> Users { get; set; }
        DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        DbSet<Notification> Notifications { get; set; }
        // lưu bất đồng bộ
        Task<int> SaveChangesAsync();
    }
}
