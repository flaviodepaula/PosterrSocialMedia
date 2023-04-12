using FluentResults;
using Posterr.Domain.Posts.DTO;
using Posterr.Domain.Posts.Queries;

namespace Posterr.Domain.Posts.Interfaces.Application;

public interface IPostApplication
{
    Task<Result<IEnumerable<PostDTO>>> ListPostsAsync(PostQuery postQuery, CancellationToken cancellationToken);
    
    Task<Result<PostDTO>> CreatePostAsync(Entities.Post post, CancellationToken cancellationToken);
}