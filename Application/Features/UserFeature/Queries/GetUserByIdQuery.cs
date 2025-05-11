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
    public class GetUserByIdQuery : IRequest<string>
    {
        public int Id { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, string>
    {
        private readonly IApplicationDbContext _context;
        public GetUserByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
                if (user == null)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 404,
                        message = "User not found"
                    });
                }
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "User retrieved successfully",
                    user
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
