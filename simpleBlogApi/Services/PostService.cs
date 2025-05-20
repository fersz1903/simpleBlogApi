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
        private readonly IContentRepository _contentRepo;

        public PostService(
            IFileService fileService,
            IRepository<Post> repo,
            IContentRepository contentRepository
        )
        {
            _fileService = fileService;
            _repo = repo;
            _contentRepo = contentRepository;
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
                _fileService.DeleteFiles(files);
                throw;
            }

            return new ResponseDto<object>(true, "Post Created");
        }

        public async Task<ResponseDto<object>> GetAllPostsAsync()
        {
            var posts = await _repo.GetAllAsync();

            var result = posts
                .Select(x => new
                {
                    x.PublicId,
                    x.Name,
                    x.Details,
                    x.CoverPicturePath,
                    x.Images,
                    x.Content,
                })
                .ToList();

            return new ResponseDto<object>(true, "Fetch Successfull", result);
        }
    }
}
