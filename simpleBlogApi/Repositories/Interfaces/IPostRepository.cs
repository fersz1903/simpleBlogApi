using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleBlogApi.Entities;

namespace simpleBlogApi.Repositories.Interfaces
{
    public interface IPostRepository
    {
        // Task<Post?> GetPostByPublicIdAsync(Guid publicId);
        Task<IEnumerable<Post>> GetAllPostsWithImagesAsync();
        Task<Post?> GetPostWithImagesAsync(Guid PublicId);
    }
}
