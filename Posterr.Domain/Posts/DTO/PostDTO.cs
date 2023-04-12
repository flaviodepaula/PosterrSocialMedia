using Posterr.Domain.Posts.Entities;
using Posterr.Domain.Posts.Support.Enums;

namespace Posterr.Domain.Posts.DTO;
 public class PostDTO
{
    public Guid Id { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public string Content { get; private set; }
    public Guid AuthorId { get; private set; }
    public EnumTypeOfPost TypeOfPost { get; private set; }
    public PostDTO? ReferencedPost { get; private set; }
    
    public PostDTO(Guid id, DateTime createdDate, string content, Guid authorId, EnumTypeOfPost typeOfPost, PostDTO? referencedPost = null)
    {
        Id = id;
        CreatedDate = createdDate;
        Content = content;
        AuthorId = authorId;
        TypeOfPost = typeOfPost;
        ReferencedPost = referencedPost;
    }
}