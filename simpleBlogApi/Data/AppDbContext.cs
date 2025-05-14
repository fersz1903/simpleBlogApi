using Microsoft.EntityFrameworkCore;
using simpleBlogApi.Entities;
using simpleBlogApi.Models;

namespace simpleBlogApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
}
