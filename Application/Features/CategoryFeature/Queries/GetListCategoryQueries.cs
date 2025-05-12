using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeature.Queries
{
    public class GetListCategoryQueries : IRequest<string>
    {
        public int page { get; set; } 
        public int pageSize { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class GetListCategoryQueriesHandler : IRequestHandler<GetListCategoryQueries, string>
    {
        private readonly IDbConnection _context;
        public GetListCategoryQueriesHandler(IDbConnection context)
        {
            _context = context;
        }
        public async Task<string> Handle(GetListCategoryQueries request, CancellationToken cancellationToken)
        {
            int offset = (request.page - 1) * request.pageSize;
            var sql = @"WITH PagedParentIds AS (
                        SELECT ParentId
                        FROM Categories
                        WHERE ParentId IS NOT NULL
                        GROUP BY ParentId
                        ORDER BY ParentId
                        OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
                    )
                    SELECT 
                        c.Id, 
                        c.Name, 
                        c.Description, 
                        c.ParentId, 
                        c.CreatedAt, 
                        c.UpdatedAt,
                        parent.Name AS NameParent
                    FROM Categories c
                    JOIN Categories parent ON c.ParentId = parent.Id
                    WHERE c.ParentId IN (SELECT ParentId FROM PagedParentIds);";
            var QueryCountRecord = $@"SELECT count(Id)
                        FROM Categories
                        WHERE ParentId IS NULL";
            var categories = await _context.QueryAsync(sql, new { Offset= offset , Limit= request.pageSize });
            var totalRecord = await _context.ExecuteScalarAsync<int>(QueryCountRecord);
            var groupedCategories = categories.GroupBy(c =>new { c.ParentId , c.NameParent })
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
                    pagination = new
                    {
                        total_record = totalRecord, // Bạn có thể thay bằng tổng thực tế từ truy vấn
                        per_page = request.pageSize,
                        current_page = request.page,
                        total_page = (int)Math.Ceiling((double)totalRecord / request.pageSize) // hoặc tính đúng nếu có tổng dòng
                    },
                    status_code = 200,
                    message = "Success"
                },
                data = groupedCategories
            });
        }
    }

}
