namespace MyBlog.Repository.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IPostRepository PostRepository { get; }
    IPostImageRepository PostImageRepository { get; }
    ICommentRepository CommentRepository { get; }


    Task CommitAsync();
    Task RollbackAsync();
}
