using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using simpleBlogApi.Models;
using simpleBlogApi.Services.Interfaces;
using Sprache;

namespace simpleBlogApi.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly FileUploadSettings _settings;
        private readonly ILogger<FileService> _logger;

        public FileService(
            IWebHostEnvironment env,
            IOptions<FileUploadSettings> settings,
            ILogger<FileService> logger
        )
        {
            _env = env;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (
                !_settings.AllowedExtensions.Contains(ext)
                || !_settings.AllowedMimeTypes.Contains(file.ContentType)
            )
                throw new Exception("Invalid file type");

            if (file.Length > _settings.MaxSizeMB * 1024 * 1024)
                throw new Exception("File size exceeds limit");

            var fileName = Guid.NewGuid() + ext;
            var folderPath = Path.Combine(
                _env.WebRootPath ?? throw new Exception("Missing wwwroot"),
                folderName
            );

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var path = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }

        public void DeleteFiles(params string[] files)
        {
            foreach (var file in files)
            { // security for path travelling
                if (
                    string.IsNullOrWhiteSpace(file)
                    || file.Contains("..")
                    || Path.IsPathFullyQualified(file)
                )
                {
                    continue; // log or continue
                }
                var path = Path.Combine("wwwroot/", file);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    // _logger.LogInformation("File Deleted: ", path);
                }
            }
        }
    }
}
