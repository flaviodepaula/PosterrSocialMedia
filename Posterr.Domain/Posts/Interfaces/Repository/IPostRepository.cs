using FluentResults;
using Posterr.Domain.Posts.Queries;

namespace Posterr.Domain.Posts.Interfaces.Repository;

public interface IPostRepository
{
    Task<Result<IEnumerable<Entities.Posts>>> ListPostsAsync(PostQuery postsQuery, CancellationToken cancellationToken);

    Task CreatePostAsync(Entities.Posts post, CancellationToken cancellationToken);

    Task<Result<long>> GetCountPostsByUser(Guid userGuid, CancellationToken cancellationToken);
}