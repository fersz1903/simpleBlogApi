using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.Post;

namespace simpleBlogApi.Services.Interfaces
{
    public interface IPostService
    {
        public Task<ResponseDto<object>> CreatePostAsync(CreatePostWithContentDto dto);
        public Task<ResponseDto<object>> GetAllPostsAsync();
        public Task<ResponseDto<object>> UpdatePostDetailsAsync(
            string PostPublicId,
            UpdatePostDto dto
        );
        public Task<ResponseDto<object>> UpdatePostCoverImageAsync(
            string PostPublicId,
            IFormFile file
        );
        public Task<ResponseDto<object>> AddImagesToPostAsync(
            string PostPublicId,
            List<IFormFile> Images
        );

        public Task<ResponseDto<object>> DeleteImagesFromPostAsync(
            string PostPublicId,
            List<Guid> ImageIds
        );

        public Task<ResponseDto<object>> DeletePostAsync(string PostPublicId);
    }
}
