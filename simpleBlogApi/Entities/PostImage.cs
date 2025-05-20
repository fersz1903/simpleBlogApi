using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Entities
{
    public class PostImage
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public string Path { get; set; } = null!;

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}
