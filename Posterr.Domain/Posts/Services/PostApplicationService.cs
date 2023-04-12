using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Posterr.Domain.Posts.DTO;
using Posterr.Domain.Posts.Interfaces.Application;
using Posterr.Domain.Posts.Interfaces.Repository;
using Posterr.Domain.Posts.Queries;
using Posterr.Domain.Posts.Support.Options;

namespace Posterr.Domain.Posts.Services;

public class PostApplicationService : IPostApplication
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostApplicationService> _logger;
    private readonly GeneralOptions _generalOptions;
    
    public PostApplicationService(IPostRepository postRepository, ILogger<PostApplicationService> logger, IOptions<GeneralOptions> generalOptions)
    {
        _postRepository = postRepository;
        _logger = logger;
        _generalOptions = generalOptions.Value;
    }

    public async Task<Result<IEnumerable<PostDTO>>> ListPostsAsync(PostQuery postQuery, CancellationToken cancellationToken)
    {
        return await _postRepository.ListPostsAsync(postQuery, cancellationToken);
    }

    public async Task<Result<PostDTO>> CreatePostAsync(Entities.Post post, CancellationToken cancellationToken)
    {
        try
        {
            var validationModel = post.IsValid();
            if (!validationModel.IsValid)
            {
                var errors = validationModel.Errors.Select(x => x.ErrorMessage).ToList();
                var error = string.Join("\n", errors);
                
                _logger.LogError($"PostApplicationService.CreatePost.Error: Please check the data used to create the post. Errors: {error}");

                return Result.Fail(error);  
            }

            var isValid = await CheckUserHasExceededDailyLimitPosts(post.AuthorId, cancellationToken)
                .ConfigureAwait(false);

            if (!isValid.Value)
            {
                const string strError = "PostApplicationService.CreatePost.Error: User has reached the maximum number of daily posts";
                _logger.LogError(strError);
                return Result.Fail<PostDTO>(strError);
            }

            var newPost = await _postRepository.CreatePostAsync(post, cancellationToken);

            return Result.Ok(newPost.Value);
        }
        catch (Exception error)
        {
            var strError = $"PostApplicationService.CreatePost.Error: Please check the data used to create the post. Error: {error.Message}";
            _logger.LogError(strError);
            return Result.Fail(strError);
        }
    }
    
    private async Task<Result<bool>> CheckUserHasExceededDailyLimitPosts(Guid userGuid, CancellationToken cancellationToken)
    {
        var countPosts = (await _postRepository.GetCountPostsByUser(userGuid, cancellationToken).ConfigureAwait(false)).Value;
        
        //TODO: use _generalOptions.MaxAllowedPostsByDay to validate. 
        //only for test purpose, using a fixed number
        return (countPosts <= 5);
    }
}