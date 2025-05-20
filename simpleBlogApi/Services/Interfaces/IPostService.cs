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
    }
}
