using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string OTP { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Email { get; set; }

        public User User { get; set; }
    }
}
