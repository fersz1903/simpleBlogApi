using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Entities
{
    public class Content
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; } = Guid.NewGuid(); // unique globally key
        public string Name { get; set; } = null!;
        public string CoverPicture { get; set; } = null!;
    }
}
