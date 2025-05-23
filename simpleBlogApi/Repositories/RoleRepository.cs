using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using simpleBlogApi.Data;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;

namespace simpleBlogApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Role?> GetByRoleNameAsync(string name)
        {
            return await _context.Roles.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
    }
}
