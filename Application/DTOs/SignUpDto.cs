using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SignUpDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
