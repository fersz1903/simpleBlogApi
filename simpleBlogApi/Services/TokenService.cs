using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.IdentityModel.Tokens;
using simpleBlogApi.Data;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;

namespace simpleBlogApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;

        public TokenService(AppDbContext context)
        {
            _context = context;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user?.Role?.Name ?? "User"),
            };

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
