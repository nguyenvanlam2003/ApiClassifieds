using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using Dapper;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Features.CategoryFeature.Command
{
    public class CreateCategoryCommand : IRequest<string>
    {
        public CategoryDto Category { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, string>
    {
        private readonly IDbConnection _dbConnection;
        public CreateCategoryCommandHandler( IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<string> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.Category.ParentId ==null && (request.Category.Children == null || !request.Category.Children.Any()))
                {
                    return JsonSerializer.Serialize(new
                    {
                        status_code = 400,
                        message = "Danh mục cha không có danh mục con"
                    });
                }
                var sql = @"INSERT INTO Categories (Name, Description, ParentId, CreatedAt, UpdatedAt)
                            VALUES (@Name, @Description, @ParentId, @CreatedAt, @UpdatedAt);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                var category = new Category
                {
                    Name = request.Category.Name,
                    Description = request.Category.Description,
                    ParentId = null,
                };
                var parentId = await _dbConnection.ExecuteScalarAsync<int>(sql,category);
    
                if (request.Category.Children != null && request.Category.Children.Any())
                {
                    foreach (var child in request.Category.Children)
                    {
                        var childCategory = new Category
                        {
                            Name = child.Name,
                            Description = child.Description,
                            ParentId = parentId,
                        };
                        await _dbConnection.ExecuteAsync(sql,childCategory);
                    }
                    
                }
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Thêm danh mục thành công"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status_code = 500,
                    message = "Có lỗi xảy ra khi thêm danh mục"
                });
            }
        }
    }
}
