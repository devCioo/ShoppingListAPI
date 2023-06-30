using System.ComponentModel.DataAnnotations;

namespace ShoppingListAPI.Models
{
    public class UpdateShoppingListDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
