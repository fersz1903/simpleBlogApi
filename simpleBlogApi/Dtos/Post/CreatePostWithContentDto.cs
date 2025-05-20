using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Dtos.Content;

namespace simpleBlogApi.Dtos.Post
{
    public class CreatePostWithContentDto
    {
        [Required]
        public CreatePostDto CreatePostDto { get; set; } = null!;
        public string? ContentPublicId { get; set; }
    }
}
