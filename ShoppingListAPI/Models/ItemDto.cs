using ShoppingListAPI.Entities;

namespace ShoppingListAPI.Models
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public int ShoppingListId { get; set; }
    }
}
