using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingListAPI.Authorization
{
    public class ShoppingListsCreatingLimitRequirementHandler : AuthorizationHandler<ShoppingListsCreatingLimitRequirement>
    {
        private readonly ShoppingListDbContext _dbContext;

        public ShoppingListsCreatingLimitRequirementHandler(ShoppingListDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShoppingListsCreatingLimitRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            requirement.ShoppingListsCreatingLimit = userRole.Equals("Standard user") ? 5 : 10;

            var shoppingListsCreatedCount = _dbContext.ShoppingLists.Count(sl => sl.UserId == userId);
            if (shoppingListsCreatedCount < requirement.ShoppingListsCreatingLimit)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
