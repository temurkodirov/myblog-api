using MyBlog.Repository.Contexts;
using MyBlog.Repository.Interfaces;

namespace MyBlog.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IUserRepository _userRepository;
    private ICategoryRepository _categoryRepository;
    private IPostRepository _postRepository;
    private IPostImageRepository _postImageRepository;
    private ICommentRepository _commentRepository;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_dbContext));

    public ICategoryRepository CategoryRepository => _categoryRepository ?? (_categoryRepository = new CategoryRepository(_dbContext));

    public IPostRepository PostRepository => _postRepository ?? (_postRepository = new PostRepository(_dbContext));

    public IPostImageRepository PostImageRepository => _postImageRepository ?? (_postImageRepository = new PostImageRepository(_dbContext));

    public ICommentRepository CommentRepository => _commentRepository ?? (_commentRepository = new CommentRepository(_dbContext));

    public void Dispose()
    { GC.SuppressFinalize(true); }

    public async Task CommitAsync()
             => await _dbContext.SaveChangesAsync();
    public async Task RollbackAsync()
        => await _dbContext.DisposeAsync();
}
