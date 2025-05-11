using Application;
using Persistence;
using WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication(builder.Configuration); // Thêm dịch vụ ứng dụng vào DI container
builder.Services.AddPersistence(builder.Configuration); // Thêm dịch vụ DbContext vào DI container
builder.Services.AddCustomSwagger(builder.Configuration); // Thêm dịch vụ Swagger vào DI container
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
