using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpleBlogApi.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile formFile, string folderName);
        public void DeleteFiles(params string[] files);
    }
}
