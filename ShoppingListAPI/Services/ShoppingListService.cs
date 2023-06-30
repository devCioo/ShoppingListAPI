using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingListAPI.Services
{
    public interface IShoppingListService
    {
        IEnumerable<ShoppingListDto> GetUserShoppingLists(int userId);
        ShoppingListDto GetShoppingListById(int userId, int shoppingListId);
        int CreateShoppingList(int userId, CreateShoppingListDto dto);
        bool RemoveShoppingList(int userId, int shoppingListId);
        bool UpdateShoppingList(int userId, int shoppingListId, UpdateShoppingListDto dto);
    }
    public class ShoppingListService : IShoppingListService
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShoppingListService(ShoppingListDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<ShoppingListDto> GetUserShoppingLists(int userId)
        {
            var shoppingLists = _dbContext
                .ShoppingLists
                .Include(sl => sl.Items)
                .Where(sl => sl.UserId == userId)
                .ToList();

            var shoppingListsDtos = _mapper.Map<List<ShoppingListDto>>(shoppingLists);

            return shoppingListsDtos;
        }
        public ShoppingListDto GetShoppingListById(int userId, int shoppingListId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Include(sl => sl.Items)
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                return null;
            }

            var shoppingListDto = _mapper.Map<ShoppingListDto>(shoppingList);

            return shoppingListDto;
        }
        public int CreateShoppingList(int userId, CreateShoppingListDto dto)
        {
            var shoppingList = _mapper.Map<ShoppingList>(dto);
            shoppingList.UserId = userId;

            _dbContext.ShoppingLists.Add(shoppingList);
            _dbContext.SaveChanges();

            return shoppingList.Id;
        }
        public bool RemoveShoppingList(int userId, int shoppingListId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                return false;
            }

            _dbContext.ShoppingLists.Remove(shoppingList);
            _dbContext.SaveChanges();

            return true;
        }
        public bool UpdateShoppingList(int userId, int shoppingListId, UpdateShoppingListDto dto)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                return false;
            }

            shoppingList.Name = dto.Name;
            shoppingList.Description = dto.Description;

            _dbContext.SaveChanges();

            return true;
        }
    }
}
