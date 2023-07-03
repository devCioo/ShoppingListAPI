using FluentValidation;
using System.Linq;

namespace ShoppingListAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(ShoppingListDbContext dbContext)
        {
            RuleFor(x => x.Login)
                .NotEmpty();
            RuleFor(x => x.Login)
                .Custom((value, context) =>
                {
                    var loginInUse = dbContext.Users.Any(u => u.Login == value);
                    if (loginInUse)
                    {
                        context.AddFailure("Login", "That login is taken");
                    }
                });

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.UsersDatas.Any(ud => ud.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
