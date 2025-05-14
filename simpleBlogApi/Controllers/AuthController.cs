using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.User;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services;

namespace simpleBlogApi.Controllers
{
    [ApiController]
    [Route("[controller]")] // [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;

        public AuthController(
            ILogger<AuthController> logger,
            IUserRepository userRepository,
            ITokenService tokenService
        )
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            User user = new User { Email = dto.Email, Name = dto.Name };
            var result = await _userRepository.RegisterAsync(user, dto.Password);

            if (!result)
            {
                return BadRequest(new ResponseDto<object>(false, "Email already in use"));
            }
            return Ok(
                new ResponseDto<object>(
                    true,
                    "User registered successfully",
                    new
                    {
                        user.PublicId,
                        user.Name,
                        user.Email,
                    }
                )
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var loginResult = await _userRepository.LoginAsync(dto.Email, dto.Password);

            if (!loginResult.Success)
                return Unauthorized(new ResponseDto<object>(false, loginResult.ErrorMessage!));

            if (loginResult.User == null)
            {
                return Unauthorized(new ResponseDto<object>(false, "User Not Found!"));
            }

            var accessToken = _tokenService.GenerateAccessToken(loginResult.User);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var result = await _userRepository.SaveRefreshTokenDBAsync(
                loginResult.User.Id,
                refreshToken
            );

            if (!result)
            {
                return Unauthorized(new ResponseDto<object>(false, "Refresh Token Error!"));
            }

            return Ok(
                new ResponseDto<object>(
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
                )
            );
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(dto.RefreshToken);

            if (user == null)
            {
                return Unauthorized(
                    new ResponseDto<object>(false, "Invalid or Expired Refresh Token")
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
                return Unauthorized(new ResponseDto<object>(false, "Refresh token creation error"));
            }

            return Ok(
                new ResponseDto<object>(
                    true,
                    "Refresh Succesfull",
                    new
                    {
                        Token = new
                        {
                            accessToken = newAccessToken,
                            refreshToken = newRefreshToken,
                        },
                    }
                )
            );
        }
    }
}
