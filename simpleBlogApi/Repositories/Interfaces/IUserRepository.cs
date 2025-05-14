using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Entities;
using simpleBlogApi.Models.Results;
using simpleBlogApi.Repositories.Interfaces;

namespace simpleBlogApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> RegisterAsync(User user, string password);
        Task<LoginResult> LoginAsync(string email, string password);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<bool> SaveRefreshTokenDBAsync(int userid, string refreshToken);
    }
}
