using FluentValidation;

namespace Posterr.Domain.Validations;

public class CreateUserValidation : CommandValidation<User.Entities.User>
{
    public CreateUserValidation()
    {
        RuleFor(user=> user.Name).NotEmpty().WithMessage("The Username is required");
        RuleFor(user=> user.Name).NotEmpty().Length(6, 14).WithMessage("Username must be between 6 and 14 characters long ");
        RuleFor(user => user.Name).Matches(@"^[0-9a-zA-Z ]+$")
            .WithMessage("Only alphanumeric characters can be used for username");
        
        
    }
}