using System;
using System.Collections.Generic;

namespace ShoppingListAPI.Models
{
    public class ShoppingListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<ItemDto> Items { get; set; }
    }
}
