using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserDetailsApi.Interfaces;


namespace UserDetailsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserDetailsController(IUserDetailsService detailsService) : ControllerBase
    {
        [HttpGet("user-details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var details = await detailsService.GetUserDetails(userId);

            return details is null ? NotFound() : Ok(details);
        }
    }
}
