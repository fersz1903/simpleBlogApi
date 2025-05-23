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
using simpleBlogApi.Services.Interfaces;

namespace simpleBlogApi.Controllers
{
    [ApiController]
    [Route("[controller]")] // [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public AuthController(
            ILogger<AuthController> logger,
            IUserRepository userRepository,
            ITokenService tokenService,
            IAuthService authService
        )
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterUserAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            var result = await _authService.RefreshAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
