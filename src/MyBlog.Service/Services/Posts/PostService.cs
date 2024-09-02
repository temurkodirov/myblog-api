using Microsoft.EntityFrameworkCore.Migrations.Operations;
using MyBlog.Domain.Entities;
using MyBlog.Domain.Exceptions;
using MyBlog.Repository.Interfaces;
using MyBlog.Repository.Models.CategoryModels;
using MyBlog.Repository.Models.PostImageModels;
using MyBlog.Repository.Models.PostModels;
using MyBlog.Repository.Utilities.Listing;
using MyBlog.Service.Common.Helpers;
using MyBlog.Service.Interfaces.Files;
using MyBlog.Service.Interfaces.Posts;
using MyBlog.Service.Interfaces.User;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace MyBlog.Service.Services.Posts;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilesService _filesService;
    private readonly IIdentityService _identityService;

    public PostService(IUnitOfWork unitOfWork, IFilesService filesService, IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _filesService = filesService;
        _identityService = identityService;
    }
    public async Task<bool> CreateAsync(PostCreateModel post)
    {
        var entity = new PostEntity
        {
            Title = post.Title,
            Content = post.Content,
            UserId = _identityService.UserId,
            CategoryId = post.CategoryId,
            CreatedAt = TimeHelper.GetDateTime(),
            UpdatedAt = TimeHelper.GetDateTime()
        };
        await _unitOfWork.PostRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();

        if (entity.Id != 0)
        {
            try
            {
                foreach (var item in post.Images)
                {
                    var imgPath = await _filesService.UploadImageAsync(item, "Post");

                    var postImageEntity = new PostImageEntity
                    {
                        PostId = entity.Id,
                        PostImagePath = imgPath,
                        CreatedAt = TimeHelper.GetDateTime(),
                        UpdatedAt = TimeHelper.GetDateTime()
                    };

                    await _unitOfWork.PostImageRepository.AddAsync(postImageEntity);
                    await _unitOfWork.CommitAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return false;
    }


    public async Task<bool> DeleteAsync(long id)
    {
        var post = await _unitOfWork.PostRepository.GetAsync(item => item.Id == id);
        if (post is null) throw new NotFoundException("Post not found");
        if (post.UserId != _identityService.UserId) throw new CustomException(401, "Post belongs other user");

        var images = await _unitOfWork.PostImageRepository.GetAllByQueryAsync(item => item.PostId == post.Id);
        if (images is not null)
        {
            foreach (var image in images)
            {
                await _filesService.DeleteImageAsync(image.PostImagePath);
            }
                 _unitOfWork.PostImageRepository.RemoveRange(images);
           await _unitOfWork.CommitAsync();
        }

        _unitOfWork.PostRepository.Remove(post);
        _unitOfWork.CommitAsync();

        return true;
    }


    public async Task<PagedList<PostModel>> GetAllAsync(PostFilterParams filterParams)
    {
        var entities = await _unitOfWork.PostRepository.GetAllByQueryAsync(item =>
            (filterParams.SearchText == string.Empty || item.Title.ToLower().Contains(filterParams.SearchText.ToLower())) &&
            (filterParams.CategoryId == null || item.CategoryId == filterParams.CategoryId) &&
            (filterParams.UserId == null || item.UserId == filterParams.UserId),
            null, x => x.CreatedAt,
            filterParams.Order == "desc");

        var items = entities.Select(entity => new PostModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            UserId = entity.UserId,
            CategoryId = entity.CategoryId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        }).ToList();

        PagedList<PostModel> pagedList = PagedList<PostModel>.ToPagedListFromQuery(
            filterParams.PageNumber,
            filterParams.PageSize,
            filterParams.Order,
            items.AsQueryable()
        );

        foreach (var post in pagedList)
        {
            var images = await _unitOfWork.PostImageRepository.GetAllByQueryAsync(item =>
            item.PostId == post.Id);

            var postImages = images.Select(entity => new PostImageModel
            {
                Id = entity.Id,
                PostId = entity.PostId,
                ImagePath = entity.PostImagePath,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            });

            post.Images.AddRange(postImages);
        }

        return pagedList;
    }

    public async Task<PostModel> GetByIdAsync(long id)
    {
        var entity = await _unitOfWork.PostRepository.GetAsync(item => item.Id == id);
        if (entity is null) throw new NotFoundException("Post not found");

        var post = new PostModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            UserId = entity.UserId,
            CategoryId = entity.CategoryId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };

        var images = await _unitOfWork.PostImageRepository.GetAllByQueryAsync(item => item.PostId == post.Id);

        var postImages = images.Select(entity => new PostImageModel
        {
            Id = entity.Id,
            PostId = entity.PostId,
            ImagePath = entity.PostImagePath,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        });

        post.Images.AddRange(postImages);

        return post;
    }

    public async Task<bool> UpdateAsync(long id, PostUpdateModel post)
    {
        var entity = await _unitOfWork.PostRepository.GetAsync(item => item.Id == id);
        if (entity is null) throw new NotFoundException("Post not found");

        entity.Title = post.Title;
        entity.Content = post.Content;
        entity.UpdatedAt = TimeHelper.GetDateTime();
        entity.CategoryId = post.CategoryId;

        if (post.Images != null) 
        {
            var images = await _unitOfWork.PostImageRepository.GetAllByQueryAsync(item => item.PostId == entity.Id);
            try
            {
                foreach (var image in images)
                {
                    await _filesService.DeleteImageAsync(image.PostImagePath);
                    _unitOfWork.PostImageRepository.Remove(image);
                    
                }

                foreach (var item in post.Images)
                {
                    var imgPath = await _filesService.UploadImageAsync(item, "Post");

                    var postImageEntity = new PostImageEntity
                    {
                        PostId = entity.Id,
                        PostImagePath = imgPath,
                        CreatedAt = TimeHelper.GetDateTime(),
                        UpdatedAt = TimeHelper.GetDateTime()
                    };

                    await _unitOfWork.PostImageRepository.AddAsync(postImageEntity);
                    await _unitOfWork.CommitAsync();
                }


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        await _unitOfWork.CommitAsync();
        return true;
       

    }
}
