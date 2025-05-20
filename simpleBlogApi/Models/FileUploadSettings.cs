using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Models
{
    public class FileUploadSettings
    {
        public int MaxSizeMB { get; set; }
        public List<string> AllowedExtensions { get; set; } = new();
        public List<string> AllowedMimeTypes { get; set; } = new();
    }
}
