using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Dtos.Post
{
    public class UpdatePostDto
    {
        // [Required]
        // public string PostPublicId { get; set; } = null!;

        [MaxLength(255)]
        public string? PostName { get; set; }

        [MaxLength(30000, ErrorMessage = "Post details too long")]
        public string? PostBody { get; set; }
    }
}
