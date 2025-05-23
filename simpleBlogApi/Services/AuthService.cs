using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.User;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services.Interfaces;

namespace simpleBlogApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenService tokenService
        )
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _roleRepository = roleRepository;
        }

        public async Task<ResponseDto<object>> RegisterUserAsync(RegisterDto dto)
        {
            var role = await _roleRepository.GetByRoleNameAsync("User");

            if (role == null)
                return new ResponseDto<object>(false, "User Roles Undefined", statusCode: 400);

            User user = new User
            {
                Email = dto.Email,
                Name = dto.Name,
                Role = role,
            };

            var result = await _userRepository.RegisterAsync(user, dto.Password);

            if (!result)
            {
                return new ResponseDto<object>(false, "Email already in use", statusCode: 400);
            }
            return new ResponseDto<object>(
                true,
                "User registered successfully",
                new
                {
                    user.PublicId,
                    user.Name,
                    user.Email,
                }
            );
        }

        public async Task<ResponseDto<object>> LoginAsync(LoginDto dto)
        {
            var loginResult = await _userRepository.LoginAsync(dto.Email, dto.Password);

            if (!loginResult.Success)
                return new ResponseDto<object>(false, loginResult.ErrorMessage!, statusCode: 401);

            if (loginResult.User == null)
            {
                return new ResponseDto<object>(false, "User Not Found!", statusCode: 401);
            }

            var accessToken = _tokenService.GenerateAccessToken(loginResult.User);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var result = await _userRepository.SaveRefreshTokenDBAsync(
                loginResult.User.Id,
                refreshToken
            );

            if (!result)
            {
                return new ResponseDto<object>(false, "Refresh Token Error!", statusCode: 401);
            }

            return new ResponseDto<object>(
                true,
                "Login successfull",
                new
                {
                    UserId = loginResult?.User?.PublicId,
                    loginResult?.User?.Email,
                    loginResult?.User?.Name,
                    Role = loginResult?.User?.Role?.Name,
                    Token = new { accessToken, refreshToken },
                }
            );
        }

        public async Task<ResponseDto<object>> RefreshAsync(RefreshRequestDto dto)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(dto.RefreshToken);

            if (user == null)
            {
                return new ResponseDto<object>(
                    false,
                    "Invalid or Expired Refresh Token",
                    statusCode: 401
                );
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var resultRefresh = await _userRepository.SaveRefreshTokenDBAsync(
                user.Id,
                newRefreshToken
            );

            if (!resultRefresh)
            {
                return new ResponseDto<object>(
                    false,
                    "Refresh token creation error",
                    statusCode: 401
                );
            }

            return new ResponseDto<object>(
                true,
                "Refresh Succesfull",
                new { Token = new { accessToken = newAccessToken, refreshToken = newRefreshToken } }
            );
        }
    }
}
