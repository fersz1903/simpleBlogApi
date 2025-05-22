using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        private readonly IContentRepository _contentRepository;

        public ContentService(
            IRepository<Content> repository,
            IFileService fileService,
            IContentRepository contentRepository
        )
        {
            _repo = repository;
            _fileService = fileService;
            _contentRepository = contentRepository;
        }

        public async Task<ResponseDto<object>> CreateContentAsync(CreateContentDto dto)
        {
            string file = ""; // list file paths, if exception occurs delete files
            try
            {
                if (dto.CoverImage == null || dto.CoverImage.Length == 0)
                    return new ResponseDto<object>(
                        false,
                        "Content Cover Picture Can't Be Empty",
                        statusCode: 401
                    );

                var imagePath = await _fileService.SaveFileAsync(
                    dto.CoverImage,
                    "uploads/contents"
                );
                file = imagePath;
                Content content = new Content { Name = dto.ContentName, CoverPicture = imagePath };

                await _repo.AddAsync(content);
                await _repo.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                _fileService.DeleteFiles(file);
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

        // TODO update işlemlerinde patch requestlerine göre ayrım yapılacak
        public async Task<ResponseDto<object>> UpdateContentAsync(UpdateContentDto dto)
        {
            string oldCoverImagePath = "";
            string newCoverImagePath = "";
            if (!Guid.TryParse(dto.PublicId, out var publicId))
                return new ResponseDto<object>(false, "Invalid content ID", statusCode: 400);

            var content = await _contentRepository.GetContentByPublicIdAsync(publicId);
            if (content == null)
                return new ResponseDto<object>(false, "Content not found", statusCode: 404);
            try
            {
                if (dto.CoverImage != null)
                {
                    oldCoverImagePath = content.CoverPicture;
                    newCoverImagePath = await _fileService.SaveFileAsync(
                        dto.CoverImage,
                        "uploads/contents"
                    );
                    content.CoverPicture = newCoverImagePath;
                }
                if (dto.ContentName != null)
                    content.Name = dto.ContentName;

                _repo.Update(content);
                await _repo.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                _fileService.DeleteFiles(newCoverImagePath);
                throw;
            }
            _fileService.DeleteFiles(oldCoverImagePath);

            return new ResponseDto<object>(true, "Content Updated Successfully");
        }

        public async Task<ResponseDto<object>> DeleteContentAsync(string contentPublicId)
        {
            string oldCoverImagePath;

            if (!Guid.TryParse(contentPublicId, out var publicId))
                return new ResponseDto<object>(false, "Invalid content ID", statusCode: 400);

            var content = await _contentRepository.GetContentByPublicIdAsync(publicId);
            if (content == null)
                return new ResponseDto<object>(false, "Content not found", statusCode: 404);

            oldCoverImagePath = content.CoverPicture;
            _repo.Delete(content);
            await _repo.SaveChangesAsync();

            _fileService.DeleteFiles(oldCoverImagePath);

            return new ResponseDto<object>(true, "Content Deleted Successfully");
        }
    }
}
