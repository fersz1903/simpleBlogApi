using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Dtos.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email required"), EmailAddress]
        public string Email { get; set; } = null!;

        [
            Required(ErrorMessage = "Password required"),
            MinLength(6, ErrorMessage = "Password must be at least 6 characters")
        ]
        public string Password { get; set; } = null!;
    }
}
