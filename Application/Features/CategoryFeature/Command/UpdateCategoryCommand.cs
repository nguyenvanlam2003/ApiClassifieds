using Application.DTOs;
using Application.Interfaces;
using Dapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeature.Command
{
    public class UpdateCategoryCommand : IRequest<string>
    {
        public CategoryDto Category { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, string>
    {
        private readonly IDbConnection _dbConnection;
        private readonly IApplicationDbContext _context;
        public UpdateCategoryCommandHandler(IDbConnection dbConnection , IApplicationDbContext context)
        {
            _dbConnection = dbConnection;
            _context = context;
        }
        public async Task<string> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var parent = await _context.Categories.FindAsync(request.Category.Id);
            if (parent == null)
            {
                return "Không tìm thấy danh mục cha";
            }
            // Cập nhật thông tin danh mục cha
            parent.Name = request.Category.Name;
            parent.Description = request.Category.Description;
            parent.UpdatedAt = DateTime.UtcNow;

            // Cập nhật danh sách danh mục con
            var existingChildren = _context.Categories
                .Where(c => c.ParentId == parent.Id)
                .ToList();
            // Lấy ID các con được gửi từ client
            var sentChildIds = request.Category.Children?.Select(c => c.Id).ToList() ?? new List<int>();

            // Xoá những đứa con cũ không còn tồn tại
            var childrenToRemove = existingChildren.Where(c => !sentChildIds.Contains(c.Id)).ToList();
            _context.Categories.RemoveRange(childrenToRemove);
            // Cập nhật hoặc thêm mới
            foreach (var childDto in request.Category.Children ?? new List<CategoryChildDto>())
            {
                if (childDto.Id == 0)
                {
                    // Thêm mới
                    var newChild = new Category
                    {
                        Name = childDto.Name,
                        Description = childDto.Description,
                        ParentId = parent.Id
                    };
                    await _context.Categories.AddAsync(newChild, cancellationToken);
                }
                else
                {
                    // Cập nhật
                    var child = existingChildren.FirstOrDefault(c => c.Id == childDto.Id);
                    if (child != null)
                    {
                        child.Name = childDto.Name;
                        child.Description = childDto.Description;
                        child.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            await _context.SaveChangesAsync();
            // Trả về kết quả
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
                     WHERE c.ParentId = @Id";

            var category = await _dbConnection.QueryAsync(sql, new { Id = request.Category.Id });
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
                status_code = 200,
                message = "Cập nhật danh mục thành công",
                data=groupedCategory.FirstOrDefault()
            });
        }
    }
}
