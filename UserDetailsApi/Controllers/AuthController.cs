using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserDetailsApi.DTOs;
using UserDetailsApi.Interfaces;

namespace UserDetailsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthManagerService authManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register ([FromBody] UserDto request)
        {
            var user = await authManager.Register(request);

            if (user is null)
                return BadRequest("User already exists");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await authManager.Login(request);
            if (result is null)
            {
                return BadRequest("Email or password is incorrect");
            }

            return Ok(result);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto request)
        {
            var result = await authManager.RefreshToken(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token");
            }

            return Ok(result);
        }
    }
}
