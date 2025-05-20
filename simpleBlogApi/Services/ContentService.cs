using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.Content;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services.Interfaces;

namespace simpleBlogApi.Services
{
    public class ContentService : IContentService
    {
        private readonly IRepository<Content> _repo;
        private readonly IFileService _fileService;

        public ContentService(IRepository<Content> repository, IFileService fileService)
        {
            _repo = repository;
            _fileService = fileService;
        }

        public async Task<ResponseDto<object>> CreateContentAsync(CreateContentDto dto)
        {
            var files = new List<string>(); // list file paths, if exception occurs delete files
            try
            {
                if (dto.CoverImage == null || dto.CoverImage.Length == 0)
                    return new ResponseDto<object>(
                        false,
                        "Content Cover Picture Can't Be Empty",
                        statusCode: 401
                    );

                var image = await _fileService.SaveFileAsync(dto.CoverImage, "uploads/contents");
                files.Add(image);
                Content content = new Content { Name = dto.ContentName, CoverPicture = image };

                await _repo.AddAsync(content);
                await _repo.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                _fileService.DeleteFiles(files);
                throw;
            }

            return new ResponseDto<object>(true, "Content Created Successfully");
        }

        public async Task<ResponseDto<object>> GetAllContentsAsync()
        {
            var contents = await _repo.GetAllAsync();

            var result = contents
                .Select(x => new
                {
                    x.PublicId,
                    x.Name,
                    x.CoverPicture,
                })
                .ToList();

            return new ResponseDto<object>(true, "Fetch Successfull", result);
        }
    }
}
