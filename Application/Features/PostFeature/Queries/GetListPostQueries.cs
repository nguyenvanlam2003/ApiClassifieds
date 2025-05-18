using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.PostFeature.Queries
{
    public class GetListPostQueries : IRequest<string>
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 15;
        public CancellationToken CancellationToken { get; set; }
    }
    public class GetListPostQueriesHandler : IRequestHandler<GetListPostQueries, string>
    {
        private readonly IDbConnection _context;
        public GetListPostQueriesHandler(IDbConnection context)
        {
            _context = context;
        }
        public async Task<string> Handle(GetListPostQueries request, CancellationToken cancellationToken)
        {
            int offset = (request.page - 1) * request.pageSize;
            var sql = $@"with ListPost as (
                        SELECT p.id
                        FROM Posts p
                        ORDER BY p.id
                        OFFSET @Offset ROWS
                        FETCH NEXT @Limit ROWS ONLY
                        )
                        select p.Id, p.Title, p.Address, p.Description, p.Image,
                        p.Price, p.Status, p.ViewCount,p.CreatedAt, p.UpdatedAt, a.Name as attribute_name ,a.Value,c.Name as category_name, u.FullName from Posts p
                        JOIN Users u on u.id = p.UserId
                        Join Categories c on c.id=p.CategoryId
                        JOIN Attributes a on a.PostId=p.id
                        Where p.Id in (select p.id from ListPost)";
            var sqlCountRecord = $@"select count(p.id) from Posts p";
            var posts = await _context.QueryAsync(sql, new { Offset = offset, Limit = request.pageSize });
            var totalRecord = await _context.ExecuteScalarAsync<int>(sqlCountRecord);

            var groupedPosts = posts.GroupBy(p => new { p.Id, p.Title, p.Address, p.Description, p.Image, p.Price, p.Status, p.ViewCount , p.CreatedAt, p.UpdatedAt, p.category_name , p.FullName })
                .Select(g => new
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title,
                    Address = g.Key.Address,
                    Description = g.Key.Description,
                    Image = g.Key.Image,
                    Price = g.Key.Price,
                    Status = g.Key.Status,
                    ViewCount = g.Key.ViewCount,
                    CreatedAt = g.Key.CreatedAt,
                    UpdatedAt = g.Key.UpdatedAt,
                    Attributes = g.Select(a => new
                    {
                        AttributeName = a.attribute_name,
                        AttributeValue = a.Value
                    }).ToList(),
                    CategoryName = g.Key.category_name,
                    UserName = g.Key.FullName
                }).ToList();
            var response = new
            {
                meta = new
                {
                    pagination = new
                    {
                        total_record = totalRecord,
                        page_size = request.pageSize,
                        current_page = request.page,
                        total_page = (int)Math.Ceiling((double)totalRecord / request.pageSize)
                    },
                    status_code = 200,
                    message = "Lấy danh sách bài đăng thành công"
                },
                data = groupedPosts
            };
            return JsonSerializer.Serialize(response);
        }
    }
}
