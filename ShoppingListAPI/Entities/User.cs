using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ShoppingListAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int UserDataId { get; set; }
        public virtual UserData UserData { get; set; }
        public virtual List<ShoppingList> ShoppingLists { get; set; }
    }
}
