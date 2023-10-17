using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNC_API.DTO.Input;
using TNC_API.Interfaces;
using TNC_API.Models;

namespace TNC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult> CreateUser(UserRequestDTO user)
        {
            if (user != null)
            {
                bool result = await _user.CreateUser(user);

                if (result) {
                    return Ok(); // User creation was successful
                }
                else
                {
                    return BadRequest(); // User creation failed
                }
            } else
            {
                return BadRequest(); // User creation failed
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _user.GetUsers();
            if (users != null)
            {
                return Ok(users);
            } 

            return BadRequest();
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _user.GetUser(id);
            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult> UpdateUser(int Id, UserRequestDTO user)
        {
            var result = await _user.UpdateUser(Id, user);

            if (result)
            {
                return Ok(); // User update was successful
            }
            else
            {
                return BadRequest(); // User update failed
            }
        }

        [HttpPatch]
        [Route("UpdateUserStatus")]
        public async Task<ActionResult> UpdateUserStatus(int Id, int status)
        {
            var user = new UserRequestDTO
            {
                Status = status
            };

            var result = await _user.UpdateUser(Id, user);

            if (result)
            {
                return Ok(); // User update was successful
            }
            else
            {
                return BadRequest(); // User update failed
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _user.DeleteUser(id);

            if (result)
            {
                return Ok();
            } else
            {
                return BadRequest();
            }
        }
    }
}
