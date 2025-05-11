using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.UserFeature.Command
{
    public class RequestChangePasswordCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string RequestType { get; set; } = "forget_password";
        public CancellationToken CancellationToken { get; set; }
    }
    public class RequestChangePasswordCommandHandler : IRequestHandler<RequestChangePasswordCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public RequestChangePasswordCommandHandler(IApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task<string> Handle(RequestChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.RequestType== "forget_password"|| request.RequestType== "change_password")
                {
                    var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
                    if (existUser == null)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            status_code = 400,
                            message = "Email không tồn tại"
                        });
                    }
                    if (!existUser.IsActive)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            status_code = 400,
                            message = "Tài khoản không hoạt động"
                        });
                    }
                }
                if (request.RequestType=="sign_up")
                {
                    // kiểm tra email đã tồn tại chưa
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
                    if (user != null)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            status_code = 400,
                            message = "Email đã tồn tại"
                        });
                    }
                }
                var otp = new Random().Next(0, 1000000).ToString("D6");
                var token = new PasswordResetToken
                {
                    Email = request.Email,
                    OTP = otp,
                    ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.PasswordResetTokens.AddAsync(token); // Thêm vào DbSet
                await _context.SaveChangesAsync(); // Lưu vào database
                _emailService.Send(request.Email, "Password Reset OTP", $"Your OTP is: {otp}");
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Success",
                });

            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status_code = 500,
                    message = ex.Message
                });
            }

        }
    }
}
