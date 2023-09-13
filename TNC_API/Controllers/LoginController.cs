using Microsoft.AspNetCore.Mvc;
using TNC_API.DTO.Input;
using TNC_API.Interfaces;

namespace TNC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {   
        public readonly ILogin _login;
        public LoginController(ILogin login)
        {
            _login = login;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult LoginUser(LoginRequestDTO login)
        {
            if (ModelState.IsValid)
            {
                var response = _login.AuthenticateUser(login);

                if (response == "No user found.")
                {
                    return Unauthorized("Invalid Credentials");
                }
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                return BadRequest("No Data Posted.");
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult> LogoutUser()
        { 
            return BadRequest("No Data Posted.");
        }
    }
}
