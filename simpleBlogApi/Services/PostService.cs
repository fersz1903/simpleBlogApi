using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using simpleBlogApi.Dtos;
using simpleBlogApi.Dtos.Post;
using simpleBlogApi.Entities;
using simpleBlogApi.Repositories.Interfaces;
using simpleBlogApi.Services.Interfaces;

namespace simpleBlogApi.Services
{
    public class PostService : IPostService
    {
        private readonly IFileService _fileService;
        private readonly IRepository<Post> _repo;
        private readonly IRepository<PostImage> _postImageRepo;
        private readonly IContentRepository _contentRepo;
        private readonly IPostRepository _postRepository;

        public PostService(
            IFileService fileService,
            IRepository<Post> repo,
            IContentRepository contentRepository,
            IPostRepository postRepository,
            IRepository<PostImage> postImageRepo
        )
        {
            _fileService = fileService;
            _repo = repo;
            _contentRepo = contentRepository;
            _postRepository = postRepository;
            _postImageRepo = postImageRepo;
        }

        public async Task<ResponseDto<object>> CreatePostAsync(CreatePostWithContentDto dto)
        {
            var files = new List<string>(); // list file paths, if exception occurs delete files
            try
            {
                var post = new Post()
                {
                    Name = dto.CreatePostDto.PostName,
                    Details = dto.CreatePostDto.PostBody,
                    Images = new List<PostImage>(),
                    CoverPicturePath = string.Empty,
                };
                if (dto.CreatePostDto.CoverPicture != null)
                {
                    var coverImage = await _fileService.SaveFileAsync(
                        dto.CreatePostDto.CoverPicture,
                        "uploads/posts"
                    );
                    post.CoverPicturePath = coverImage;
                    files.Add(coverImage);
                }
                if (dto.CreatePostDto.Images != null)
                {
                    foreach (var image in dto.CreatePostDto.Images)
                    {
                        var postImage = new PostImage()
                        {
                            Path = await _fileService.SaveFileAsync(image, "uploads/posts"),
                            Post = post,
                            // PostId = post.Id,
                        };
                        post.Images.Add(postImage);
                        files.Add(postImage.Path);
                    }
                }

                if (dto.ContentPublicId != null)
                {
                    if (!Guid.TryParse(dto.ContentPublicId, out var publicId))
                        return new ResponseDto<object>(
                            false,
                            "Invalid content ID",
                            statusCode: 400
                        );

                    var content = await _contentRepo.GetContentByPublicIdAsync(publicId);
                    if (content == null)
                        return new ResponseDto<object>(false, "Content not found", statusCode: 404);

                    post.Content = content;
                    post.ContentId = content.Id;
                }
                await _repo.AddAsync(post);
                await _repo.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                _fileService.DeleteFiles(files.ToArray());
                throw;
            }

            return new ResponseDto<object>(true, "Post Created");
        }

        public async Task<ResponseDto<object>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsWithImagesAsync();

            var result = posts
                .Select(x => new
                {
                    x.PublicId,
                    x.Name,
                    x.Details,
                    x.CoverPicturePath,
                    Images = x.Images?.Select(x => new { x.PublicId, x.Path }),
                    Content = x.Content == null
                        ? null
                        : new { x.Content?.PublicId, x.Content?.Name },
                })
                .ToList();

            return new ResponseDto<object>(true, "Fetch Successfull", result);
        }

        public async Task<ResponseDto<object>> UpdatePostDetailsAsync(
            string PostPublicId,
            UpdatePostDto dto
        )
        {
            if (!Guid.TryParse(PostPublicId, out var publicId))
                return new ResponseDto<object>(false, "Invalid post ID", statusCode: 400);

            var post = await _repo.GetByPublicIdAsync(publicId);
            if (post == null)
                return new ResponseDto<object>(false, "Post not found", statusCode: 404);

            if (dto.PostName != null)
            {
                post.Name = dto.PostName;
            }
            if (dto.PostBody != null)
            {
                post.Details = dto.PostBody;
            }

            _repo.Update(post);
            await _repo.SaveChangesAsync();

            return new ResponseDto<object>(true, "Post Details Updated Successfully");
        }

        public async Task<ResponseDto<object>> UpdatePostCoverImageAsync(
            string PostPublicId,
            IFormFile CoverImage
        )
        {
            string oldCoverImagePath = "";
            string newCoverImagePath = "";
            if (!Guid.TryParse(PostPublicId, out var publicId))
                return new ResponseDto<object>(false, "Invalid post ID", statusCode: 400);

            var post = await _repo.GetByPublicIdAsync(publicId);
            if (post == null)
                return new ResponseDto<object>(false, "Post not found", statusCode: 404);

            try
            {
                if (CoverImage != null)
                {
                    oldCoverImagePath = post.CoverPicturePath;
                    newCoverImagePath = await _fileService.SaveFileAsync(
                        CoverImage,
                        "uploads/posts"
                    );
                    post.CoverPicturePath = newCoverImagePath;

                    _repo.Update(post);
                    await _repo.SaveChangesAsync();
                }
            }
            catch (System.Exception)
            {
                _fileService.DeleteFiles(newCoverImagePath);
                throw;
            }
            _fileService.DeleteFiles(oldCoverImagePath);

            return new ResponseDto<object>(true, "Post Cover Image Updated Successfully");
        }

        public async Task<ResponseDto<object>> AddImagesToPostAsync(
            string PostPublicId,
            List<IFormFile> Images
        )
        {
            var images = new List<string>();
            if (!Guid.TryParse(PostPublicId, out var publicId))
                return new ResponseDto<object>(false, "Invalid post ID", statusCode: 400);

            var post = await _repo.GetByPublicIdAsync(publicId);
            if (post == null)
                return new ResponseDto<object>(false, "Post not found", statusCode: 404);
            if (post.Images == null)
            {
                post.Images = new List<PostImage>();
            }
            try
            {
                if (Images != null)
                {
                    foreach (var image in Images)
                    {
                        var postImage = new PostImage()
                        {
                            Path = await _fileService.SaveFileAsync(image, "uploads/posts"),
                            Post = post,
                        };
                        post.Images.Add(postImage);
                        images.Add(postImage.Path);
                    }
                }
                _repo.Update(post);
                await _repo.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                _fileService.DeleteFiles(images.ToArray());
                throw;
            }
            return new ResponseDto<object>(true, "Images Added to post successfully");
        }

        public async Task<ResponseDto<object>> DeleteImagesFromPostAsync(
            string PostPublicId,
            List<Guid> ImageIds
        )
        {
            if (!Guid.TryParse(PostPublicId, out var publicId))
                return new ResponseDto<object>(false, "Invalid post ID", statusCode: 400);

            var post = await _postRepository.GetPostWithImagesAsync(publicId);
            if (post == null)
                return new ResponseDto<object>(false, "Post not found", statusCode: 404);

            if (post.Images == null)
            {
                return new ResponseDto<object>(false, "Post has no images", statusCode: 404);
            }

            if (ImageIds == null)
                return new ResponseDto<object>(false, "No image IDs provided", statusCode: 400);

            var imagesToDelete = post.Images.Where(x => ImageIds.Contains(x.PublicId)).ToList();

            if (imagesToDelete.Count == 0)
                return new ResponseDto<object>(false, "No matching images found", statusCode: 404);

            var filesToDelete = imagesToDelete.Select(x => x.Path).ToList();

            foreach (var img in imagesToDelete)
            {
                _postImageRepo.Delete(img);
            }

            await _postImageRepo.SaveChangesAsync();

            _fileService.DeleteFiles(filesToDelete.ToArray());

            return new ResponseDto<object>(true, "Images deleted successfully");
        }

        public async Task<ResponseDto<object>> DeletePostAsync(string PostPublicId)
        {
            var deleteFiles = new List<string>();

            if (!Guid.TryParse(PostPublicId, out var publicId))
                return new ResponseDto<object>(false, "Invalid post ID", statusCode: 400);

            var post = await _postRepository.GetPostWithImagesAsync(publicId);
            if (post == null)
                return new ResponseDto<object>(false, "Post not found", statusCode: 404);

            if (post.Images != null)
            {
                var imagesToDelete = post.Images.Select(p => p.Path).ToList();
                deleteFiles.AddRange(imagesToDelete);
            }

            deleteFiles.Add(post.CoverPicturePath);

            _repo.Delete(post);
            await _repo.SaveChangesAsync();

            _fileService.DeleteFiles(deleteFiles.ToArray());

            return new ResponseDto<object>(true, "Post deleted successfully");
        }
    }
}
