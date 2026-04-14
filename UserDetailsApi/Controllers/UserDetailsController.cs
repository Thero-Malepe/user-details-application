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
            var email = User.FindFirstValue(ClaimTypes.Email);
            if(email is null)
            {
                return NotFound();
            }
            var details = await detailsService.GetUserDetails(email);

            return details is null ? NotFound() : Ok(details);
        }
    }
}
