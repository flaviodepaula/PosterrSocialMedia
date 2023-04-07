using System.Text;
using NuGet.Frameworks;
using Posterr.Domain.Entities;
using Posterr.Domain.Support.Enums;

namespace UnitTests;

public class PostClassTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateValidClass()
    {
        const string content = "value test";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Posts referencedPost = null;

        Assert.That(new Posts(content, authorId, postType, referencedPost), Is.InstanceOf(typeof(Posts)));
    } 
    
    [Test]
    public void ValidClass_OriginalPost()
    {
        const string content = "value test";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Posts referencedPost = null;

        var newPost = new Posts(content, authorId, postType, referencedPost);
        
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(isOk, errorMsg);
    }
    
    [Test]
    public void ValidClass_RepostPost_BaseOriginal()
    {
        const string content = "";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Repost;
        var referencedPost = new Posts("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(isOk, errorMsg);
    }
    
    [Test]
    public void ValidClass_QuodePost_BaseOriginal()
    {
        const string content = "testing quode post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        var referencedPost = new Posts("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(isOk, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToRepostAPostWithComment()
    {
        const string content = "trying to repost a original post with comment - not allowed";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Repost;
        var referencedPost = new Posts("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToQuodeAPostWithoutComment()
    {
        //trying to quode a original post without comment - not allowed
        const string content = "";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        var referencedPost = new Posts("original post", Guid.NewGuid(), EnumTypeOfPost.Original, null );

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
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
        var referencedPost = new Posts("quode post", Guid.NewGuid(), EnumTypeOfPost.Quode, null );

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToQuodeAQuodePost()
    {
        //trying to quode a quode post - not allowed
        const string content = "trying to quode a quode post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        var referencedPost = new Posts("quode post", Guid.NewGuid(), EnumTypeOfPost.Quode, null );

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
    
    [Test]
    public void InvalidClass_RepostWithoutReferencedPost()
    {
        //trying to repost a post without a referenced post - not allowed
        const string content = "trying to repost a post without a referenced post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Repost;
        Posts referencedPost = null;

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
    [Test]
    public void InvalidClass_QuodeWithoutReferencedPost()
    {
        //trying to quode a post without a referenced post - not allowed
        const string content = "trying to quode a quode post";
        var authorId = Guid.NewGuid();
        const EnumTypeOfPost postType = EnumTypeOfPost.Quode;
        Posts referencedPost = null;

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
    [Test]
    public void InvalidClass_TryingToCreateAPostWithoutAuthor()
    {
        const string content = "trying to create a post without a author";
        var authorId = Guid.Empty;
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Posts referencedPost = null;

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
    
    [Test]
    public void InvalidClass_ContentExtraLarge()
    {
        const string text = "X";
        const int repetitions = 778;
        
        var content = new StringBuilder(text.Length * repetitions).Insert(0, text, repetitions).ToString() ;
        var authorId = Guid.Empty;
        const EnumTypeOfPost postType = EnumTypeOfPost.Original;
        Posts referencedPost = null;

        var newPost = new Posts(content, authorId, postType, referencedPost);
        var isOk = newPost.Validate();
        var errorMsg = newPost.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
}