namespace Posterr.Domain.Posts.Support.Options;

public class GeneralOptions
{
/// <summary>
/// value used to set the max posts of user per day. This config allow the value be set by pipeline configurations
/// </summary>
    public int MaxAllowedPostsByDay { get; set; }

    public GeneralOptions()
    {
    }
}