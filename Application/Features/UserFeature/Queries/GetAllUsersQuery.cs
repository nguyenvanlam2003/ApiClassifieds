using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.UserFeature.Queries
{
    public class GetAllUsersQuery : IRequest<string>
    {
        public CancellationToken CancellationToken { get; set; }
    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, string>
    {
        private readonly IApplicationDbContext _context;
        public GetAllUsersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _context.Users.ToListAsync(cancellationToken);
                if (users == null || !users.Any())
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 404,
                        message = "No users found"
                    });
                }
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Users retrieved successfully",
                    users
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
