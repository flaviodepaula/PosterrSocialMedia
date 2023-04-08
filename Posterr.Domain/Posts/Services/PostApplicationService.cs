using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Posterr.Domain.Posts.Interfaces.Application;
using Posterr.Domain.Posts.Interfaces.Repository;
using Posterr.Domain.Posts.Queries;
using Posterr.Domain.Posts.Support.Options;

namespace Posterr.Domain.Posts.Services;

public class PostApplicationService : IPostApplication
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostApplicationService> _logger;
    private readonly UsersOptions _generalOptions;
    
    public PostApplicationService(IPostRepository postRepository, ILogger<PostApplicationService> logger, IOptions<UsersOptions> generalOptions)
    {
        _postRepository = postRepository;
        _logger = logger;
        _generalOptions = generalOptions.Value;
    }

    public async Task<Result<IEnumerable<Entities.Posts>>> ListPostsAsync(PostQuery postQuery, CancellationToken cancellationToken)
    {
        return await _postRepository.ListPostsAsync(postQuery, cancellationToken);
    }

    public async Task<Result<bool>> CreatePostAsync(Entities.Posts post, CancellationToken cancellationToken)
    {
        try
        {
            var validationModel = post.IsValid();
            if (!validationModel.IsValid)
            {
                _logger.LogError("PostApplicationService.CreatePost.Error: Please check the data used to create the post");

                var errors = validationModel.Errors.SelectMany(x => x.ErrorMessage).ToString();
                return Result.Fail(errors);  
            }

            var isValid = await CheckUserHasExceededDailyLimitPosts(post.AuthorId, cancellationToken)
                .ConfigureAwait(false);

            if (!isValid.Value)
            {
                const string strError = "PostApplicationService.CreatePost.Error: User has reached the maximum number of daily posts";
                _logger.LogError(strError);
                return Result.Fail<bool>(strError);
            }

            await _postRepository.CreatePostAsync(post, cancellationToken);

            return Result.Ok(true);
        }
        catch (Exception e)
        {
            const string strError = "PostApplicationService.CreatePost.Error: Please check the data used to create the post";
            _logger.LogError(strError);
            return Result.Fail(strError);
        }
    }
    
    public async Task<Result<bool>> CheckUserHasExceededDailyLimitPosts(Guid userGuid, CancellationToken cancellationToken)
    {
        var countPosts = (await _postRepository.GetCountPostsByUser(userGuid, cancellationToken).ConfigureAwait(false)).Value;

        return (countPosts <= _generalOptions.MaxAllowedPostsByDay);
    }
}