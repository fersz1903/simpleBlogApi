using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.Post;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services.Interfaces;

namespace simpleBlogApi.Controllers
{
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;

        public PostController(ILogger<PostController> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _postService.GetAllPostsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostWithContentDto dto)
        {
            var result = await _postService.CreatePostAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{PostPublicId}/UpdatePostDetails")]
        public async Task<IActionResult> UpdatePostDetails(string PostPublicId, UpdatePostDto dto)
        {
            var result = await _postService.UpdatePostDetailsAsync(PostPublicId, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{PostPublicId}/UpdatePostCoverImage")]
        public async Task<IActionResult> UpdatePostCoverImage(
            string PostPublicId,
            [FromForm] IFormFile CoverImage
        )
        {
            var result = await _postService.UpdatePostCoverImageAsync(PostPublicId, CoverImage);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{PostPublicId}/AddImages")]
        public async Task<IActionResult> AddImages(
            string PostPublicId,
            [FromForm] List<IFormFile> Images
        )
        {
            var result = await _postService.AddImagesToPostAsync(PostPublicId, Images);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{PostPublicId}/DeleteImages")]
        public async Task<IActionResult> DeleteImages(string PostPublicId, List<Guid> Images)
        {
            var result = await _postService.DeleteImagesFromPostAsync(PostPublicId, Images);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{PostPublicId}/DeletePost")]
        public async Task<IActionResult> DeletePost(string PostPublicId)
        {
            var result = await _postService.DeletePostAsync(PostPublicId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
