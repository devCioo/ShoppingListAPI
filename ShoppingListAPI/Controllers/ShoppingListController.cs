using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Models;
using ShoppingListAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ShoppingListAPI.Controllers
{
    [Route("api/shoppinglist")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShoppingListDto>> GetAll()
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var shoppingLists = _shoppingListService.GetUserShoppingLists(userId);

            return Ok(shoppingLists);
        }

        [HttpGet("{shoppingListId}")]
        public ActionResult<ShoppingListDto> Get([FromRoute] int shoppingListId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var shoppingList = _shoppingListService.GetShoppingListById(userId, shoppingListId);

            return Ok(shoppingList);
        }

        [HttpPost]
        [Authorize(Policy = "ShoppingListsLimit")]
        public ActionResult Post([FromBody] CreateShoppingListDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var shoppingListId = _shoppingListService.CreateShoppingList(userId, dto);

            return Created($"api/shoppinglist/{shoppingListId}", null);
        }

        [HttpDelete("{shoppingListId}")]
        public ActionResult Delete([FromRoute] int shoppingListId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _shoppingListService.RemoveShoppingList(userId, shoppingListId);

            return NoContent();
        }

        [HttpPut("{shoppingListId}")]
        public ActionResult Update([FromRoute] int shoppingListId, [FromBody] UpdateShoppingListDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _shoppingListService.UpdateShoppingList(userId, shoppingListId, dto);

            return Ok();
        }
    }
}
