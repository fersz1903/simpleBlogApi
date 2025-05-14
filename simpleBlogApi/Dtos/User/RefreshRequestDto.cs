using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Dtos.User
{
    public class RefreshRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
