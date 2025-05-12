using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeature.Queries
{
    public class GetCategoryByIdQueries : IRequest<string>
    {
        public int Id { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class GetCategoryByIdQueriesHandler : IRequestHandler<GetCategoryByIdQueries, string>
    {
        private readonly IDbConnection _context;
        public GetCategoryByIdQueriesHandler(IDbConnection context)
        {
            _context = context;
        }
        public async Task<string> Handle(GetCategoryByIdQueries request, CancellationToken cancellationToken)
        {
            var sql = @" SELECT 
                         c.Id, 
                         c.Name, 
                         c.Description, 
                         c.ParentId, 
                         c.CreatedAt, 
                         c.UpdatedAt,
                         parent.Name AS NameParent
                     FROM Categories c
                     JOIN Categories parent ON c.ParentId = parent.Id
                     WHERE c.ParentId = @Id"
            ;

            var category = await _context.QueryAsync(sql, new { Id = request.Id });
            var groupedCategory = category.GroupBy(c => new { c.ParentId, c.NameParent })
            .Select(g => new
            {
                Id = g.Key.ParentId,
                ParentName = g.Key.NameParent,
                Categories = g.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    c.CreatedAt,
                    c.UpdatedAt
                }).ToList()
            }).ToList();
            return JsonSerializer.Serialize(new
            {
                meta = new
                {
                    
                    status_code = 200,
                    message = "Success"
                },
                data = groupedCategory.FirstOrDefault()
            });
        }
    }
}
