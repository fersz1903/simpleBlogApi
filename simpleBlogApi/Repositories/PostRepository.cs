using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using simpleBlogApi.Data;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;

namespace simpleBlogApi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsWithImagesAsync()
        {
            return await _context
                .Posts.Include(p => p.Images)
                .Include(p => p.Content)
                .ToListAsync();
        }

        public async Task<Post?> GetPostWithImagesAsync(Guid PublicId)
        {
            return await _context
                .Posts.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.PublicId == PublicId);
        }

        // public async Task<Post?> GetPostByPublicIdAsync(Guid publicId)
        // {
        //     return await _context.Posts.FirstOrDefaultAsync(p => p.PublicId == publicId);
        // }
    }
}
