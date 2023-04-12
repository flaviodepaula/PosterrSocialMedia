using FluentResults;
using Moq;
using Posterr.Domain.Posts.DTO;
using Posterr.Domain.Posts.Entities;
using Posterr.Domain.Posts.Interfaces.Repository;
using Posterr.Domain.Posts.Queries;
using Posterr.Domain.Posts.Support.Enums;

namespace UnitTests.Posts;

public static class Mock_IPostRepository
{
    public static Mock<IPostRepository> CreatePostMock(Post postModel)
    {
        var repositoryMock = new Mock<IPostRepository>();
        repositoryMock.Setup((e => e.GetCountPostsByUser(It.IsAny<Guid>(), CancellationToken.None))).ReturnsAsync(() => 1);
        repositoryMock.Setup(e => e.CreatePostAsync(It.IsAny<Post>(), CancellationToken.None))
            .ReturnsAsync(() => ConvertPostOnPostDTO(postModel));
        
        return repositoryMock;
    }

    public static Mock<IPostRepository> ListPostsMock(PostQuery query)
    {
        var repositoryMock = new Mock<IPostRepository>();
        repositoryMock.Setup(e => e.ListPostsAsync(query, CancellationToken.None)).ReturnsAsync(GetListFakePosts());

        return repositoryMock;
    }
 
    private static Result<PostDTO> ConvertPostOnPostDTO(Post postModel)
    {
        return new PostDTO(postModel.GetId(), postModel.CreatedDate, postModel.Content, postModel.AuthorId,
            postModel.TypeOfPost, null);
    }

    private static Result<IEnumerable<PostDTO>> GetListFakePosts()
    {
        IEnumerable<PostDTO> resultQuery = new List<PostDTO>()
        {
            new PostDTO( Guid.NewGuid(),DateTime.Now, "1st test object", Guid.NewGuid(), EnumTypeOfPost.Original),
            new PostDTO( Guid.NewGuid(),DateTime.Now,"2st test object", Guid.NewGuid(), EnumTypeOfPost.Original),
            new PostDTO( Guid.NewGuid(),DateTime.Now,"3st test object", Guid.NewGuid(), EnumTypeOfPost.Original),
        };

        return Result.Ok(resultQuery);
    }
}