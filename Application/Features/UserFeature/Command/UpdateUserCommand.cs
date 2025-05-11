using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.UserFeature.Command
{
    public class UpdateUserCommand : IRequest<string>
    {
        public UpdateUserDto User { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public UpdateUserCommandHandler(IApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FindAsync(request.User.Id);
                if (user == null)
                {
                    return null; // Hoặc ném ngoại lệ nếu không tìm thấy người dùng
                }
                user.Email = request.User.Email;
                user.FullName = request.User.FullName;
                user.Phone = request.User.Phone;
                user.Address = request.User.Address;
                user.UpdatedAt = DateTime.UtcNow;
                var notification = new Notification
                {
                    UserId = user.Id,
                    Title = "Cập nhật thông tin cá nhân",
                    Message = "Thông tin của bạn đã được cập nhật thành công",
                    CreatedAt = user.UpdatedAt,
                };
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                _emailService.Send(user.Email, notification.Title, $"{notification.Message} lúc {notification.CreatedAt}");
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Đổi thông tin thành công"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status_code = 500,
                    message = "Lỗi không xác định"
                });
            }

        }
    }
}
