using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using simpleBlogApi.Data;
using simpleBlogApi.Entities;
using simpleBlogApi.Middlewares;
using simpleBlogApi.Models;
using simpleBlogApi.Repositories;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services;
using simpleBlogApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Development",
        policy =>
        {
            policy
                .AllowCredentials()
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});
#endregion

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

#region AUTH
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
if (
    string.IsNullOrWhiteSpace(jwtSecret)
    || string.IsNullOrWhiteSpace(jwtIssuer)
    || string.IsNullOrWhiteSpace(jwtAudience)
)
{
    throw new InvalidOperationException("JWT environment variables are not set.");
}
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "OnlyAdmin",
        policy =>
        {
            policy.RequireRole("Admin");
        }
    );
});

#endregion

#region DEPENDENCY
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.Configure<FileUploadSettings>(
    builder.Configuration.GetSection("FileUploadSettings")
);
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
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
    app.UseCors("Development");
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

app.UseStaticFiles();

app.MapControllers();

app.Run();
