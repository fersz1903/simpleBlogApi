using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.Content;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services.Interfaces;

namespace simpleBlogApi.Controllers
{
    [Route("[controller]")]
    public class ContentController : Controller
    {
        private readonly ILogger<ContentController> _logger;
        private readonly IContentService _contentService;

        public ContentController(ILogger<ContentController> logger, IContentService contentService)
        {
            _logger = logger;
            _contentService = contentService;
        }

        [HttpPost("createContent")]
        public async Task<IActionResult> CreateContent(CreateContentDto dto)
        {
            var result = await _contentService.CreateContentAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetAllContents")]
        public async Task<IActionResult> GetAllContents()
        {
            var result = await _contentService.GetAllContentsAsync();
            return StatusCode(result.StatusCode, result);
        }
    }
}
