using FluentValidation;
using ShoppingListAPI.Entities;
using System.Linq;

namespace ShoppingListAPI.Models.Validators
{
    public class ItemQueryValidator : AbstractValidator<ItemQuery>
    {
        private string[] allowedSortByColumnNames =
            {nameof(Item.Name), nameof(Item.Description), nameof(Item.UnitPrice), nameof(Item.Quantity)};
        public ItemQueryValidator()
        {
            RuleFor(i => i.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",",allowedSortByColumnNames)}]");
        }
    }
}
