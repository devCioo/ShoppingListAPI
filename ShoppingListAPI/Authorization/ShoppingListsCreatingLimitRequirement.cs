using Microsoft.AspNetCore.Authorization;
using ShoppingListAPI.Entities;

namespace ShoppingListAPI.Authorization
{
    public class ShoppingListsCreatingLimitRequirement : IAuthorizationRequirement
    {
        public ShoppingListsCreatingLimitRequirement()
        {
            
        }
        public int ShoppingListsCreatingLimit { get; set; }
    }
}
