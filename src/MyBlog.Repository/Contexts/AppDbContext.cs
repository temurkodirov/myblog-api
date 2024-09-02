namespace MyBlog.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Entities;
using MyBlog.Repository.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
       
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<PostImageEntity> PostImages { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }

}
