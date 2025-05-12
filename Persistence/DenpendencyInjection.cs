using Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class DenpendencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // thêm DbContext
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            // thêm IApplicationDbContext
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<MyDbContext>());
            // thêm các dịch vụ khác nếu cần
            services.AddScoped<IDbConnection>(provider =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
           
            return services;
        }
    }
}
