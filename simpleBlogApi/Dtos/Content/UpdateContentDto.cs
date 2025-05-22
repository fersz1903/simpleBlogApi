using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Dtos.Content
{
    public class UpdateContentDto
    {
        [Required]
        public string PublicId { get; set; } = null!;

        [MaxLength(255)]
        public string? ContentName { get; set; }

        public IFormFile? CoverImage { get; set; }
    }
}
