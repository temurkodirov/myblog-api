using AutoMapper;
using MyBlog.Repository.Entities;
using MyBlog.Repository.Models.CategoryModels;

namespace MyBlog.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryEntity, CategoryModel>().ReverseMap();
        CreateMap<CategoryUpdateModel, CategoryEntity>().ReverseMap();
        CreateMap<CategoryCreateModel, CategoryEntity>().ReverseMap();  
    }
}
