using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using simpleBlogApi.Dtos.Content;

namespace simpleBlogApi.Dtos.Post
{
    public class CreatePostDto
    {
        [Required, MaxLength(255)]
        public string PostName { get; set; } = null!;

        [Required, MaxLength(30000, ErrorMessage = "Post details too long")]
        public string PostBody { get; set; } = null!;

        public List<IFormFile>? Images { get; set; }

        public IFormFile? CoverPicture { get; set; }
    }
}
