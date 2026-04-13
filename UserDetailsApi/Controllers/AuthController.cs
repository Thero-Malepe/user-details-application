using Microsoft.AspNetCore.Mvc;
using UserDetailsApi.DTOs.AuthDtos;
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

        [HttpGet()]
        [Route("send-reset-email")]
        public async Task<IActionResult> SendResetEmail(string email)
        {
            var result = await authManager.PasswordResetEmail(email);
            if(!result)
                return NotFound();

            return Ok();
        }

        [HttpPost()]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetDto details)
        {
            var result = await authManager.ResetPassword(details);
            if (!result)
                return NotFound();

            return Ok();
        } 
    }
}
