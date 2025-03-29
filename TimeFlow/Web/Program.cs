using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;
using Services;
using Services.Profiles;
using DataAccess.Repositories;
using DataAccess;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ILessonService, LessonService>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();

builder.Services.AddDbContext<RepositoryDbContext>(
    options =>
    {
        options.UseNpgsql(connectionString);
    });

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Разрешаем CORS для фронтенда
app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();
app.Run();
