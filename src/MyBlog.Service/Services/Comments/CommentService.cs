using MyBlog.Domain.Entities;
using MyBlog.Domain.Exceptions;
using MyBlog.Repository.Interfaces;
using MyBlog.Repository.Models.CategoryModels;
using MyBlog.Repository.Models.CommentModels;
using MyBlog.Repository.Repositories;
using MyBlog.Repository.Utilities.Listing;
using MyBlog.Service.Common.Helpers;
using MyBlog.Service.Interfaces.Comments;
using MyBlog.Service.Interfaces.User;

namespace MyBlog.Service.Services.Comments;

public class CommentService : ICommentService
{
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IIdentityService identityService, IUnitOfWork unitOfWork)
    {
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateAsync(CommentCreateModel comment)
    {
        var post = await _unitOfWork.PostRepository.GetAsync(item => item.Id == comment.PostId);
        if (post is null) throw new NotFoundException("Post not Found");

        var newComment = new CommentEntity
        {
            AuthorId = _identityService.UserId,
            PostId = comment.PostId,
            Content = comment.Content,
            CreatedAt = TimeHelper.GetDateTime(),
            UpdatedAt = TimeHelper.GetDateTime(),
        };

        await _unitOfWork.CommentRepository.AddAsync(newComment);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var comment = await _unitOfWork.CommentRepository.GetAsync(item => item.Id == id);
        if (comment.AuthorId != _identityService.UserId) throw new CustomException(401,"Coment not belongs to you");
        if (comment is null) throw new NotFoundException("Comment not Found");

        _unitOfWork.CommentRepository.Remove(comment);
        await _unitOfWork.CommitAsync();

        return true;
    }
   

    public async Task<PagedList<CommentModel>> GetAllAsync(CommentFilterModel filterParams)
    {
        var entityItems = await _unitOfWork.CommentRepository.GetAllAsync(item =>
        (filterParams.PostId == null || item.PostId == filterParams.PostId),
              null, x => x.CreatedAt,
              filterParams.Order == "desc");

        var items = entityItems.Select(entity => new CommentModel
        {
            Id = entity.Id,
            Content = entity.Content,
            AuthorId = entity.AuthorId,
            PostId = entity.PostId,
            AuthorModel = null,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,

        }).ToList();

        foreach(var item in items)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == item.AuthorId);
            
            if (user is not null)
            {
                var author = new CommentAuthorModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };
                item.AuthorModel = author;
            }
        }

        PagedList<CommentModel> pagedList = PagedList<CommentModel>.ToPagedListFromQuery(
            filterParams.PageNumber,
            filterParams.PageSize,
            filterParams.Order,
            items.AsQueryable()
            );

        return pagedList;
    }
   

    public async Task<CommentModel> GetByIdAsync(long id)
    {
       var entity = await _unitOfWork.CommentRepository.GetAsync(item => item.Id == id);
       if (entity is null) throw new NotFoundException("Coment not found");

        var user = await _unitOfWork.UserRepository.GetAsync(x => x.Id == entity.AuthorId);

        var coment = new CommentModel
        {
            Id = entity.Id,
            Content = entity.Content,
            AuthorId = entity.AuthorId,
            PostId = entity.PostId,
            AuthorModel = null,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
        if (user is not null)
        {
            var author = new CommentAuthorModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
            coment.AuthorModel = author;
        }

        return coment;

    }

    public Task<bool> UpdateAsync(long id, CommentUpdateModel comment)
    {
        throw new NotImplementedException();
    }
}
