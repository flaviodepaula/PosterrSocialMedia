using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NuGet.Frameworks;
using Posterr.Domain.Posts.Entities;
using Posterr.Domain.Posts.Queries;
using Posterr.Domain.Posts.Services;
using Posterr.Domain.Posts.Support.Enums;
using Posterr.Domain.Posts.Support.Options;
using UnitTests.Posts;

namespace UnitTests;

public class PostClassTests
{
    
    private Mock<ILogger<PostApplicationService>> _loggerPostApplication;
    private Mock<IOptions<GeneralOptions>> _mockOptions;
    
    [SetUp]
    public void Setup()
    {
        _loggerPostApplication = new Mock<ILogger<PostApplicationService>>();
        _mockOptions = new Mock<IOptions<GeneralOptions>>();
    }

    [Test]
    public void CreateValidClass()
    {
        const string content = "value test";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Post referencedPost = null;

        Assert.That(new Post(content, authorId, postType, referencedPost), Is.InstanceOf(typeof(Post)));
    } 
    
    [Test]
    public void ValidClass_OriginalPost()
    {
        const string content = "value test";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Post referencedPost = null;

        var newPost = new Post(content, authorId, postType, referencedPost);

        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void ValidClass_RepostPost_BaseOriginal()
    {
        const string content = "";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Repost;
        var referencedPost = new Post("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void ValidClass_QuodePost_BaseOriginal()
    {
        const string content = "testing quode post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        var referencedPost = new Post("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToRepostAPostWithComment()
    {
        const string content = "trying to repost a original post with comment - not allowed";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Repost;
        var referencedPost = new Post("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToQuodeAPostWithoutComment()
    {
        //trying to quode a original post without comment - not allowed
        const string content = "";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        var referencedPost = new Post("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void InvalidClass_NotAllowedToRepostPostsOfSameType()
    {
        /*
         * User mustn't repost a repost post
         * User mustn't quode a quode post
         */
        const string content = "trying to quode a quode post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        var referencedPost = new Post("quode post", Guid.NewGuid(), EnumTypeOfPost.Quode, null );

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToQuodeAQuodePost()
    {
        //trying to quode a quode post - not allowed
        const string content = "trying to quode a quode post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        var referencedPost = new Post("quode post", Guid.NewGuid(), EnumTypeOfPost.Quode, null );

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    
    [Test]
    public void InvalidClass_RepostWithoutReferencedPost()
    {
        //trying to repost a post without a referenced post - not allowed
        const string content = "trying to repost a post without a referenced post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Repost;
        Post referencedPost = null;

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void InvalidClass_QuodeWithoutReferencedPost()
    {
        //trying to quode a post without a referenced post - not allowed
        const string content = "trying to quode a quode post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        Post referencedPost = null;

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToCreateAPostWithoutAuthor()
    {
        const string content = "trying to create a post without a author";
        var authorId = Guid.Empty;
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Post referencedPost = null;

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    
    [Test]
    public void InvalidClass_ContentExtraLarge()
    {
        const string text = "X";
        const int repetitions = 778;
        
        var content = new StringBuilder(text.Length * repetitions).Insert(0, text, repetitions).ToString() ;
        var authorId = Guid.Empty;
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Post referencedPost = null;

        var newPost = new Post(content, authorId, postType, referencedPost);
        var isOk = newPost.IsValid();
        var errorMsg = isOk.Errors.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk.IsValid, errorMsg);
    }
    
    [Test]
    public async Task ListPostsAsync()
    {
        var query = new PostQuery()
        {
            AllPosts = true
        };

        var postRepositoryMock = Mock_IPostRepository.ListPostsMock(query);
        var postApplication = new PostApplicationService(postRepositoryMock.Object, _loggerPostApplication.Object, _mockOptions.Object);    

        var result = await postApplication.ListPostsAsync(query, CancellationToken.None);

        Assert.That(result.Value, Is.Not.Empty);
    }
    
    [Test]
    public async Task CreatePostAsync()
    {
        var guidUser = Guid.NewGuid();
        var newPost = new Post( "1st test object", guidUser, EnumTypeOfPost.Original);

        var repositoryMock = Mock_IPostRepository.CreatePostMock(newPost);
        var postApplication = new PostApplicationService(repositoryMock.Object, _loggerPostApplication.Object, _mockOptions.Object);

        var result = await postApplication.CreatePostAsync(newPost, CancellationToken.None);

        Assert.That(result.Value.Content, Is.EqualTo(newPost.Content));
    }
}