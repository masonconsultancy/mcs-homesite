using FluentValidation;
using MCS.HomeSite.Data.Models.Users;

namespace MCS.HomeSite.Data.CustomValidators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id);
            RuleFor(x => x.Name);
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
        }
    }
}
