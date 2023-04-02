using FluentValidation;

namespace Posterr.Domain.Validations
{
    public class CommandValidation<T> : AbstractValidator<T> where T : class
    {
    }
}
