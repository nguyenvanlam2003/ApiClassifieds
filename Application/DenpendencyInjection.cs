using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using NETCore.MailKit;
using Application.Interfaces;
using Application.Features.JwtTokenFeature.Command;

namespace Application
{
    public static class  DenpendencyInjection
    {
        // Phương thức mở rộng cho IServiceCollection để thêm các dịch vụ của ứng dụng
        public static IServiceCollection AddApplication(this IServiceCollection services,  IConfiguration configuration)
        {
            // Add application services MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());
            // Thêm các dịch vụ khác của ứng dụng tại đây
            services.AddScoped<IJwtTokenGenerator, JwtTokenCommand>();

            // Add MailKit Email Service
            var smtpSection = configuration.GetSection("Smtp");
            if (!smtpSection.Exists())
            {
                throw new InvalidOperationException("SMTP configuration is missing in appsettings.json");
            }

            services.AddSingleton<IEmailService>(sp =>
            {
                var options = new MailKitOptions
                {
                    Server = smtpSection["Server"],
                    Port = int.Parse(smtpSection["Port"]),
                    SenderName = smtpSection["SenderName"],
                    SenderEmail = smtpSection["SenderEmail"],
                    Account = smtpSection["UserName"],
                    Password = smtpSection["Password"],
                    Security = bool.Parse(smtpSection["UseSsl"] ?? "true")
                };
                return new EmailService(new MailKitProvider(options));
            });
            return services;
        }
    }
}
