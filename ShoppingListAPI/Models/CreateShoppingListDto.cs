using System.ComponentModel.DataAnnotations;

namespace ShoppingListAPI.Models
{
    public class CreateShoppingListDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
