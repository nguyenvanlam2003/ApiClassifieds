using Application.DTOs;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Features.PostFeature.Command
{
    public class CreatePostCommand : IRequest<string>
    {
        public PostDto PostDto { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, string>
    {
        public readonly IDbConnection _dbConnection;
        public CreatePostCommandHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<string> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var insertPostSql = @"INSERT INTO Posts (UserId, CategoryId, Title, Address, Price, Description, Image, Status, ViewCount , CreatedAt , UpdatedAt)
                            VALUES (@UserId, @CategoryId, @Title, @Address, @Price, @Description, @Image, @Status, @ViewCount, @CreatedAt, @UpdatedAt);
                            SELECT CAST(SCOPE_IDENTITY() as int);";
                var postId = await _dbConnection.ExecuteScalarAsync<int>(insertPostSql, new
                {
                    request.PostDto.UserId,
                    request.PostDto.CategoryId,
                    request.PostDto.Title,
                    request.PostDto.Address,
                    request.PostDto.Price,
                    request.PostDto.Description,
                    request.PostDto.Image,
                    request.PostDto.Status,
                    request.PostDto.ViewCount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                if (request.PostDto.Attributes != null && request.PostDto.Attributes.Any())
                {
                    var insertAttributeSql = @"INSERT INTO Attributes (Name, Value, PostId,  CreatedAt, UpdatedAt)
                                                VALUES (@Name, @Value, @PostId, @CreatedAt, @UpdatedAt);";
                    foreach (var attribute in request.PostDto.Attributes)
                    {
                        await _dbConnection.ExecuteAsync(insertAttributeSql, new
                        {
                            attribute.Name,
                            attribute.Value,
                            PostId = postId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        });
                    }
                }
                return JsonSerializer.Serialize(new
                {
                    status_code = 200,
                    message = "Thêm bài đăng thành công"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status_code = 500,
                    message = "Có lỗi xảy ra khi thêm bài đăng"
                });
            }
        }
    }
}
