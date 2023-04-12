using FluentValidation;
using Posterr.Domain.Posts.Support.Enums;

namespace Posterr.Domain.Validations
{
    public class CreatePostValidation : CommandValidation<Posts.Entities.Post>
    {
        public CreatePostValidation()
        {
            RuleFor(cf => cf.AuthorId).NotEmpty().WithMessage("The Author of post (userId) is required");
            RuleFor(cf => cf.Content).MaximumLength(777).WithMessage("The field Content must be a string with a maximum length of 777");            

            //Validating content for all type of posts
            RuleFor(cf => cf.Content).NotEmpty().When(type=> type.TypeOfPost == EnumTypeOfPost.Original).WithMessage("User must provide a content for Original posts");
            RuleFor(cf => cf.Content).NotEmpty().When(type=> type.TypeOfPost == EnumTypeOfPost.Quode).WithMessage("User must provide a content for Quode posts");
            RuleFor(cf => cf.Content).Empty().When(type=> type.TypeOfPost == EnumTypeOfPost.Repost).WithMessage("User can't provide a content for Repost posts");
            
            
            //Validating referencedPost for Repost and Quode type posts
            RuleFor(cf => cf.ReferencedPost).NotNull().When(type => type.TypeOfPost == EnumTypeOfPost.Repost).WithMessage("User must provide a referenced post for Repost Post");
            RuleFor(cf=> cf.ReferencedPost).NotNull().When(type => type.TypeOfPost == EnumTypeOfPost.Quode).WithMessage("User must provide a referenced post for Quode Post");
            
            
            //Users mustn't repost post's of same type
            RuleFor(cf => cf.ReferencedPost!.TypeOfPost.ToString()).NotEqual(cf => cf.TypeOfPost.ToString()).When(cf=> cf.ReferencedPost != null).WithMessage("User mustn't repost posts of same type");
        }
    }
}
