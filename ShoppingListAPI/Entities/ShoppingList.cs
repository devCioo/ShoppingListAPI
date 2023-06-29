using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ShoppingListAPI.Entities
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<Item> Items { get; set; }
    }
}
