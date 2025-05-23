using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.User;

namespace simpleBlogApi.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<ResponseDto<object>> RegisterUserAsync(RegisterDto dto);
        public Task<ResponseDto<object>> LoginAsync(LoginDto dto);
        public Task<ResponseDto<object>> RefreshAsync(RefreshRequestDto dto);
    }
}
