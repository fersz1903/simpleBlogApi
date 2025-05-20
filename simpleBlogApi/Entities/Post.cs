using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;

        [Column(TypeName = "text"), MaxLength(30000, ErrorMessage = "Post details too long")]
        public string Details { get; set; } = null!;

        public string CoverPicturePath { get; set; } = string.Empty;
        public ICollection<PostImage>? Images { get; set; } = new List<PostImage>();

        public int? ContentId { get; set; }
        public Content? Content { get; set; }
    }
}
