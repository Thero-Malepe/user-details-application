using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserDetailsApi.Interfaces;


namespace UserDetailsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserDetailsController(IUserDetailsService detailsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUserDetails([FromQuery] string email)
        {
            var user = await detailsService.GetUserDetails(email);
            if(user is null)
            {
                return NotFound("User details not found");
            }

            return Ok(user);        
        }
    }
}
