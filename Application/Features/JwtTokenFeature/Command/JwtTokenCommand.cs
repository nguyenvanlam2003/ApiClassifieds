using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.JwtTokenFeature.Command
{
    public class JwtTokenCommand : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenCommand(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            // Danh sách các role hợp lệ
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("FullName", user.FullName),
                new Claim("Role", user.Role),
                new Claim("Phone", user.Phone),
                new Claim("Address", user.Address),
                new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpirationInMinutes"])),  // Sử dụng cấu hình Expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
