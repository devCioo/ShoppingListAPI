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
            CreateMap<CreateItemDto, Item>();

            CreateMap<ShoppingListDto, ShoppingList>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
