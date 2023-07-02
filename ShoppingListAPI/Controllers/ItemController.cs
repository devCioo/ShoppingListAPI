using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;
using ShoppingListAPI.Services;
using System.Collections.Generic;

namespace ShoppingListAPI.Controllers
{
    [Route("api/user/{userId}/shoppinglist/{shoppingListId}/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<List<ItemDto>> GetAll([FromRoute] int userId, [FromRoute] int shoppingListId)
        {
            var items = _itemService.GetAllShoppingListItems(userId, shoppingListId);

            return Ok(items);
        }

        [HttpGet("{itemId}")]
        public ActionResult<ItemDto> Get([FromRoute] int userId, [FromRoute] int shoppingListId, [FromRoute] int itemId)
        {
            var item = _itemService.GetItemById(userId, shoppingListId, itemId);

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int userId, [FromRoute] int shoppingListId, [FromBody] CreateItemDto dto)
        {
            var itemId = _itemService.AddItemToShoppingList(userId, shoppingListId, dto);

            return Created($"api/user/{userId}/shoppinglist/{shoppingListId}/item/{itemId}", null);
        }

        [HttpDelete("{itemId}")]
        public ActionResult Delete([FromRoute] int userId, [FromRoute] int shoppingListId, [FromRoute] int itemId)
        {
            _itemService.RemoveItemFromShoppingList(userId, shoppingListId, itemId);

            return NoContent();
        }
    }
}
