using API6.Services;
using Microsoft.AspNetCore.Mvc;

namespace API6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {

            try
            {
                var users =await usersService.GetAllUsers();
                if(users.Any())
                return Ok(users);

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(404);
            }
        }

    }
}