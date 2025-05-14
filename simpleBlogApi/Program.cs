using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using simpleBlogApi.Data;
using simpleBlogApi.Entities;
using simpleBlogApi.Middlewares;
using simpleBlogApi.Models;
using simpleBlogApi.Repositories;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Env.Load();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(80); // Docker container içinde 80 portu dinlesin
// });

#region DB CONNECTION
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
#endregion


#region DEPENDENCY
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ITokenService, TokenService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference(options =>
    {
        options.Title = "Simple Blog API";
        options.ShowSidebar = true;
        options.DefaultHttpClient = new(ScalarTarget.JavaScript, ScalarClient.Axios);
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
