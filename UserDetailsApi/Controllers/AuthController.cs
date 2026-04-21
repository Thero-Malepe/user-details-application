using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models.RequestModels.UserRequestModels;

namespace UserDetailsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthManagerService authManager, IMapper mapper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register ([FromBody] RegisterUserRequestModel request)
        {
            var dto = mapper.Map<UserDto>(request);
            var user = await authManager.Register(dto);
            if (user is null)
                return BadRequest("User already exists");

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel request)
        {
            var dto = mapper.Map<LoginDto>(request);
            var result = await authManager.Login(dto);
            if (result is null)
            {
                return BadRequest("Email or password is incorrect");
            }

            return Ok(result);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel request)
        {
            var dto = mapper.Map<TokenResponseDto>(request);
            var result = await authManager.RefreshToken(dto);
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel request)
        {
            var dto = mapper.Map<ResetDto>(request);
            var result = await authManager.ResetPassword(dto);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpGet()]
        [Route("validate-token")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            var result = await authManager.ValidateResetToken(token);
            if (result is null)
                return NotFound();

            return Ok();
        }

        [HttpDelete()]
        [Route("delete-user")]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!.ToString();
            var result = await authManager.DeleteUser(userId);
            if (result is null)
                return BadRequest();

            if(result == true)
                return NoContent();

            return NotFound();
        }
    }
}
