using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using simpleBlogApi.Data;
using simpleBlogApi.Entities;
using simpleBlogApi.Models.Results;
using simpleBlogApi.Repositories.Interfaces;

namespace simpleBlogApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepository(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user == null)
                return new LoginResult { Success = false, ErrorMessage = "User not found" };

            var verificationres = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

            return verificationres == PasswordVerificationResult.Success
                ? new LoginResult { Success = true, User = user }
                : new LoginResult { Success = false, ErrorMessage = "Incorrect password" };
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return false;
            }
            // TODO password hasher
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<bool> SaveRefreshTokenDBAsync(int userid, string refreshToken)
        {
            var result = await _context
                .Users.Where(u => u.Id == userid)
                .ExecuteUpdateAsync(update =>
                    update
                        .SetProperty(user => user.RefreshToken, refreshToken)
                        .SetProperty(
                            user => user.RefreshTokenExpiryTime,
                            DateTime.UtcNow.AddDays(7)
                        )
                );

            return result > 0; // return true if updated rows bigger than 0
        }
    }
}
