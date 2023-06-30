using AutoMapper;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Models;

namespace ShoppingListAPI
{
    public class ShoppingListMappingProfile : Profile
    {
        public ShoppingListMappingProfile()
        {
            CreateMap<ShoppingList, ShoppingListDto>();
            CreateMap<CreateShoppingListDto, ShoppingList>();

            CreateMap<Item, ItemDto>();
        }
    }
}
