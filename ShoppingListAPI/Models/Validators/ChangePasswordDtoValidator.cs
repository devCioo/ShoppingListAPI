using FluentValidation;
using System.Linq;

namespace ShoppingListAPI.Models.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator(ShoppingListDbContext dbContext)
        {
            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.OldPassword);
            RuleFor(x => x.NewPassword)
                .MinimumLength(6);
            RuleFor(x => x.ConfirmNewPassword)
                .Equal(e => e.NewPassword);
        }
    }
}
