using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;
using ShoppingListAPI.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace ShoppingListAPI.Controllers
{
    [Route("api/shoppinglist/{shoppingListId}/item")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<List<ItemDto>> GetAll([FromRoute] int shoppingListId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var items = _itemService.GetAllShoppingListItems(userId, shoppingListId);

            return Ok(items);
        }

        [HttpGet("{itemId}")]
        public ActionResult<ItemDto> Get([FromRoute] int shoppingListId, [FromRoute] int itemId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var item = _itemService.GetItemById(userId, shoppingListId, itemId);

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int shoppingListId, [FromBody] CreateItemDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var itemId = _itemService.AddItemToShoppingList(userId, shoppingListId, dto);

            return Created($"api/shoppinglist/{shoppingListId}/item/{itemId}", null);
        }

        [HttpDelete("{itemId}")]
        public ActionResult Delete([FromRoute] int shoppingListId, [FromRoute] int itemId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _itemService.RemoveItemFromShoppingList(userId, shoppingListId, itemId);

            return NoContent();
        }
    }
}
