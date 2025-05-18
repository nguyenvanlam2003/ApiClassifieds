using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.AuthFeature.Command
{
    public class SignUpCommand : IRequest<string>
    {
        public SignUpDto Request { get; set; }
        public CancellationToken CancellationToken { get; set; } = default;

    }
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public SignUpCommandHandler(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Kiểm tra email đã tồn tại
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Request.Email, cancellationToken);
                if (existingUser != null)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Email đã tồn tại"
                    });
                }
                // kiem tra otp 
                var existingOtp = await _context.PasswordResetTokens.FirstOrDefaultAsync(p => p.OTP == request.Request.OTP && p.Email == request.Request.Email && p.IsUsed==false);
                if (existingOtp == null)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Email không đúng"
                    });
                }
                if (existingOtp.ExpiryDate < DateTime.UtcNow)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Mã OTP đã hết hạn"
                    });
                }
                // Kiểm tra role hợp lệ
                var validRoles = new[] { "Client", "Admin" };
                if (!validRoles.Contains(request.Request.Role))
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Vai trò không hợp lệ"
                    });
                }
               
                var user = new User
                {
                    Email = request.Request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password),
                    Role = request.Request.Role,
                    FullName = request.Request.FullName,
                    Phone = request.Request.Phone,
                    Address = request.Request.Address,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                existingOtp.IsUsed = true;
                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync();

                var token = _jwtTokenGenerator.GenerateToken(user);

                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Success",
                    token,
                    user_id = user.Id
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status_code = 500,
                    message = ex.Message,
                });
            }
        }
       
    }
}
