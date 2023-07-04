using System.Text.Json.Serialization;

namespace ShoppingListAPI.Entities
{
    public class Item
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        [JsonIgnore]
        public int ShoppingListId { get; set; }
        [JsonIgnore]
        public virtual ShoppingList ShoppingList { get; set; }
    }
    public enum MeasureUnit
    {
        Units = 0,
        Kilograms = 1
    }
}
