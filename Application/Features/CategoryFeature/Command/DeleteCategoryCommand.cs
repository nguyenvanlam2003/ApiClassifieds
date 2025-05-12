using Application.Interfaces;
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

namespace Application.Features.CategoryFeature.Command
{
    public class DeleteCategoryCommand : IRequest<string>
    {
        public int Id { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class DeteleCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, string>
    {
        private readonly IDbConnection _context;
        public readonly IApplicationDbContext _applicationDbContext;
        public DeteleCategoryCommandHandler(IDbConnection context, IApplicationDbContext applicationDbContext)
        {
            _context = context;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<string> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _applicationDbContext.Categories.FindAsync(request.Id);
            if (category == null)
            {
                return JsonSerializer.Serialize(new
                {
                    status_code = 404,
                    message = "Không tìm thấy danh mục cần xóa",
                });
            }
            // Xóa danh mục
            var sql = "DELETE FROM Categories WHERE Id = @Id OR ParentId = @Id";
            var affectedRows = await _context.ExecuteAsync(sql, new { Id = request.Id });
            return JsonSerializer.Serialize(new
            {
                status_code = 200,
                message = "Xóa danh mục thành công",
            });
        }
    }
}
