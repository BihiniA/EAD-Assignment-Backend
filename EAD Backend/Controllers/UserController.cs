using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDBExample.Models;

namespace EAD_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : Controller
    {

        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet] // get all user endpoint
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAsync();

            if (users == null)
            {
                return NotFound(new { success = false, data = users, msg = "record not found" });

            }

            return Ok(new { success = true, data = users, msg = "success" });

        }

        [HttpPost] //craete user endpoint
        public async Task<IActionResult> Post([FromBody] Users users)
        {
            var user = await _userService.CreateAsync(users);
            if (user == null)
            {
                return BadRequest(new { success = false, data = user, msg = "record not found" });
            }

            return Ok(new { success = true, data = user, msg = "success" });
        }

        [HttpPut("updateuser/{id}")] //update user
        public async Task<IActionResult> UpdateUser(string id, [FromBody] Users users)
        {
            await _userService.UpdateUser(id, users);
            return Ok();
        }

        // [HttpPut("updatestatus/{id}")] // Update user status only
        // public async Task<IActionResult> UpdateStatus(string id, [FromBody] StatusUpdateRequest statusUpdate)
        // {
        // var success = await _userService.UpdateUserStatus(id, statusUpdate.status);

        // if (!success)
        // {
        //return NotFound(new { success = false, msg = "User not found" });
        //    }

        //    return Ok(new { success = true, msg = "Status updated successfully" });
        //}


        [HttpDelete("delete/{id}")] //delete user
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }

        //login method
        [HttpPost("login")] //login validation and token generation
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userService.Login(login.email, login.password);
            if (user == null)
            {
                return NotFound(new { success = false, data = user, token = "", msg = "record not found" });
            }
            var token = _userService.GenerateJSONWebToken(user);
            return Ok(new { success = true, data = user, token = token, msg = "success" });
        }

        [HttpGet("get/{id}")] // get single user
        public async Task<Users> GetUser(string id)
        {
            var user = await _userService.GetUser(id);

            return user;
        }

    }

    public class StatusUpdateRequest
    {
        public string status { get; set; }
    }

}

