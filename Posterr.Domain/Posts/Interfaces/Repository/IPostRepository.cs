using FluentResults;
using Posterr.Domain.Posts.DTO;
using Posterr.Domain.Posts.Queries;

namespace Posterr.Domain.Posts.Interfaces.Repository;

public interface IPostRepository
{
    Task<Result<IEnumerable<PostDTO>>> ListPostsAsync(PostQuery postsQuery, CancellationToken cancellationToken);

    Task<Result<PostDTO>> CreatePostAsync(Entities.Post post, CancellationToken cancellationToken);

    Task<Result<int>> GetCountPostsByUser(Guid userGuid, CancellationToken cancellationToken);
}