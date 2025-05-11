using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.AuthFeature.Queries
{
    public class LogInQueries : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CancellationToken CancellationToken { get; set; } = default;
    }
    public class LogInQueriesHandler : IRequestHandler<LogInQueries, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LogInQueriesHandler(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> Handle(LogInQueries request, CancellationToken cancellationToken)
        {
            try
            {
                // Kiểm tra email đã tồn tại
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
                if (existingUser == null)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Email không tồn tại"
                    });
                }
                // Kiểm tra mật khẩu
                if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Mật khẩu không đúng"
                    });
                }
                // Tạo JWT token
                var token = _jwtTokenGenerator.GenerateToken(existingUser);
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Đăng nhập thành công",
                    token,
                    user_id = existingUser.Id
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
