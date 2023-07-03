using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;
using ShoppingListAPI.Services;

namespace ShoppingListAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);

            return Ok(token);
        }
    }
}
