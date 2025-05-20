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
    public class ContentRepository : IContentRepository
    {
        private readonly AppDbContext _context;

        public ContentRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // public Task<bool> CreateContentAsync()
        // {
        //     throw new NotImplementedException();
        // }

        // public Task<IEnumerable<Content>> GetAllContentsAsync()
        // {
        //     throw new NotImplementedException();
        // }
        public async Task<Content?> GetContentByPublicIdAsync(Guid publicId)
        {
            return await _context.Contents.FirstOrDefaultAsync(c => c.PublicId == publicId);
        }
    }
}
