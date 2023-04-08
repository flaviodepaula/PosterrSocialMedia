using FluentResults;
using Posterr.Domain.Posts.Queries;

namespace Posterr.Domain.Posts.Interfaces.Application;

public interface IPostApplication
{
    Task<Result<IEnumerable<Entities.Posts>>> ListPostsAsync(PostQuery postQuery, CancellationToken cancellationToken);
    
    Task<Result<bool>> CreatePostAsync(Entities.Posts post, CancellationToken cancellationToken);
}