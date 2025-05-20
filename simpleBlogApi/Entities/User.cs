using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid(); // unique globally key
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Role Role { get; set; } = new Role { Name = "User" };
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
