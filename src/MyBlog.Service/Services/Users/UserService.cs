using MyBlog.Domain.Exceptions;
using MyBlog.Repository.Interfaces;
using MyBlog.Repository.Models.UserModels;
using MyBlog.Repository.Utilities.Listing;
using MyBlog.Service.Common.Helpers;
using MyBlog.Service.Dtos.User;
using MyBlog.Service.Interfaces.Files;
using MyBlog.Service.Interfaces.User;
using MyBlog.Service.Interfaces.Users;

namespace MyBlog.Service.Services.Users;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFilesService _filesService;
    private readonly IIdentityService _identityService;

    public UserService(IUnitOfWork unitOfWork, IFilesService filesService, IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _filesService = filesService;
        _identityService = identityService;
    }

    public async Task<PagedList<UserDto>> GetAllAsync(UserFilterParams filterParams)
    {

        var entities = await _unitOfWork.UserRepository.GetAllByQueryAsync(item =>
              (filterParams.SearchText == string.Empty || item.Email.Contains(filterParams.SearchText)),
               null, x => x.CreatedAt,
              filterParams.Order == "desc");

        var items = entities.Select(entity => new UserDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            IsMale = entity.IsMale,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            ImagePath = entity.ImagePath,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        }).ToList();



        PagedList<UserDto> pagedList = PagedList<UserDto>.ToPagedListFromQuery(
            filterParams.PageNumber,
            filterParams.PageSize,
            filterParams.Order,
            items.AsQueryable()
        );


        return pagedList;

    }

    public async Task<UserDto> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(item => item.Id == id);

        if (user is null) throw new NotFoundException("User not found");

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsMale = user.IsMale,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            ImagePath = user.ImagePath,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt

        };

        return userDto;
    }



    public async Task<bool> UpdateAsync(UserUpdateDto userUpdateDto)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(item => item.Id == _identityService.UserId);

        if (user.Id != _identityService.UserId) throw new CustomException(401, "Post belongs other user");

        if (user is null) throw new NotFoundException("User not found");

        var deleteImage = await _filesService.DeleteImageAsync(user.ImagePath);
        var imgPath = await _filesService.UploadImageAsync(userUpdateDto.Image, "User");
       
        user.ImagePath = imgPath;
        user.UpdatedAt = TimeHelper.GetDateTime();
        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;
        user.PhoneNumber = userUpdateDto.PhoneNumber;
        user.IsMale = userUpdateDto.IsMale;
    

         _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.CommitAsync();
        return true;
    }
}
