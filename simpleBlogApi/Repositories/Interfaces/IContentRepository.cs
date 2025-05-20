using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Entities;

namespace simpleBlogApi.Repositories.Interfaces
{
    public interface IContentRepository
    {
        // Task<IEnumerable<Content>> GetAllContentsAsync();
        Task<Content?> GetContentByPublicIdAsync(Guid publicId);
    }
}
