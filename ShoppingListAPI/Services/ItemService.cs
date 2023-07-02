using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Exceptions;
using ShoppingListAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingListAPI.Services
{
    public interface IItemService
    {
        List<ItemDto> GetAllShoppingListItems(int userId, int shoppingListId);
        ItemDto GetItemById(int userId, int shoppingListId, int itemId);
        int AddItemToShoppingList(int userId, int shoppingListId, CreateItemDto dto);
        void RemoveItemFromShoppingList(int userId, int shoppingListId, int itemId);
    }
    public class ItemService : IItemService
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IMapper _mapper;

        public ItemService(ShoppingListDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<ItemDto> GetAllShoppingListItems(int userId, int shoppingListId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .Include(sl => sl.Items)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var itemDtos = _mapper.Map<List<ItemDto>>(shoppingList.Items);

            return itemDtos;
        }
        public ItemDto GetItemById(int userId, int shoppingListId, int itemId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var item = _dbContext.Items
                .FirstOrDefault(i => i.Id == itemId);

            if (item is null || item.ShoppingListId != shoppingListId)
            {
                throw new NotFoundException("Item not found");
            }

            var itemDto = _mapper.Map<ItemDto>(item);

            return itemDto;
        }
        public int AddItemToShoppingList(int userId, int shoppingListId, CreateItemDto dto)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var item = _mapper.Map<Item>(dto);
            item.ShoppingListId = shoppingListId;

            _dbContext.Items.Add(item);
            _dbContext.SaveChanges();

            return item.Id;
        }
        public void RemoveItemFromShoppingList(int userId, int shoppingListId, int itemId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var item = _dbContext.Items
                .FirstOrDefault(i => i.Id == itemId);

            if (item is null || item.ShoppingListId != shoppingListId)
            {
                throw new NotFoundException("Item not found");
            }

            _dbContext.Remove(item);
            _dbContext.SaveChanges();
        }
    }
}
