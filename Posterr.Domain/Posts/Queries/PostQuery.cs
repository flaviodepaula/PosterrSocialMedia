namespace Posterr.Domain.Posts.Queries;

public class PostQuery
{
    public Guid? UserId { get; set; }
    public bool AllPosts { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsValid()
    {
        return AllPosts || UserId != null;
    }
}