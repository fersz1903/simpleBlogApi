using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Entities;

namespace simpleBlogApi.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByRoleNameAsync(string name);
    }
}
