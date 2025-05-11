using Application.DTOs;
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
    public class ChangePasswordCommand : IRequest<string>
    {
        public PasswordResetDto passwordReset { get; set; }
        public CancellationToken cancellation { get; set; }
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public ChangePasswordCommandHandler(IApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.passwordReset.Email, cancellationToken);
                if (user == null)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Email không tồn tại"
                    });
                }
                var passwordReset = await _context.PasswordResetTokens.FirstOrDefaultAsync(p => p.Email == user.Email && p.OTP == request.passwordReset.OTP, cancellationToken);
                if (passwordReset == null)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Vui lòng điền dúng mã OTP"
                    });
                }
                if (passwordReset.IsUsed)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Mã OTP đã được sử dụng"
                    });
                }
                if (passwordReset.ExpiryDate < DateTime.UtcNow)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Mã OTP đã hết hạn"
                    });
                }
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.passwordReset.NewPassword);
                passwordReset.IsUsed = true;
                var notification = new Notification
                {
                    UserId = user.Id,
                    Message = "Mật khẩu của bạn đã được đổi thành công",
                    CreatedAt = DateTime.UtcNow,
                    Title = "Đổi mật khẩu",
                };
                user.UpdatedAt = notification.CreatedAt;
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                _emailService.Send(user.Email, notification.Title, $"{notification.Message} lúc {notification.CreatedAt}");
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Đổi mật khẩu thành công"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status_code = 500,
                    message = "Lỗi hệ thống"
                });
            }

        }
    }
}
