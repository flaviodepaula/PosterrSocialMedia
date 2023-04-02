using FluentValidation;
using Posterr.Domain.Entities;
using Posterr.Domain.Support.Enums;

namespace Posterr.Domain.Validations
{
    public class CreatePostValidation : CommandValidation<Posts>
    {
        public CreatePostValidation()
        {
            RuleFor(cf => cf.AuthorId).NotEmpty().WithMessage("The userId field is required");
            RuleFor(cf => cf.Content).MaximumLength(777).WithMessage("The field Content must be a string with a maximum length of 777");            

            //Validating content for all type of posts
            RuleFor(cf => cf.TypeOfPost).Equal(EnumTypeOfPost.Original).When(cf => cf.Content == null).WithMessage("User must provide a content for Original posts");
            RuleFor(cf => cf.TypeOfPost).Equal(EnumTypeOfPost.Quode).When(cf => cf.Content == null).WithMessage("User must provide a content for Quode posts");
            RuleFor(cf => cf.TypeOfPost).Equal(EnumTypeOfPost.Repost).When(cf => cf.Content != null).WithMessage("User can't provide a content for Repost posts");
            
            //Validating referencedPost for Repost type of posts
            RuleFor(cf=> cf.TypeOfPost).Equal(EnumTypeOfPost.Repost).When(cf => cf.ReferencedPost == null).WithMessage("User must provide a original Post for Repost posts");
            RuleFor(cf=> cf.TypeOfPost).Equal(EnumTypeOfPost.Repost).When(cf => cf.ReferencedPost?.TypeOfPost == EnumTypeOfPost.Repost).WithMessage("User can't Repost a post that is a Repost");

            //Validating referencedPost for Qude type of posts
            RuleFor(cf=> cf.TypeOfPost).Equal(EnumTypeOfPost.Quode).When(cf => cf.ReferencedPost == null).WithMessage("User must provide a original Post for Quode posts");
            RuleFor(cf => cf.TypeOfPost).Equal(EnumTypeOfPost.Quode).When(cf => cf.ReferencedPost?.TypeOfPost == EnumTypeOfPost.Quode).WithMessage("User can't Repost a post and leave a coment along it if this post is a Quode Post");
        }
    }
}
