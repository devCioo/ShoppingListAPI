namespace ShoppingListAPI.Models
{
    public class ItemQuery
    {
        public string SortBy { get; set; }
        public  SortDirection SortDirection { get; set; }
    }
    public enum SortDirection
    {
        Asc,
        Desc
    }
}
