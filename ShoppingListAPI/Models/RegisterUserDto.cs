using System.ComponentModel.DataAnnotations;

namespace ShoppingListAPI.Models
{
    public class RegisterUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
