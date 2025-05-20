using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.Content;

namespace simpleBlogApi.Services.Interfaces
{
    public interface IContentService
    {
        public Task<ResponseDto<object>> CreateContentAsync(CreateContentDto dto);
        public Task<ResponseDto<object>> GetAllContentsAsync();
    }
}
