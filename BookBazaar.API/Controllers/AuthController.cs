using BookBazaar.Application.DTOs;
using BookBazaar.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookBazaar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            var user = await _userService.RegisterUserAsync(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var token = await _userService.LoginUserAsync(dto);
            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }
    }
}