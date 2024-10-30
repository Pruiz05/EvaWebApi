using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// greetings from jwt valid token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("greetings")]
        public ActionResult Get()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userName == null || userEmail == null)
            {
                return Unauthorized("User information is not available.");
            }

            var response = new
            {
                Message = $"Welcome {userName}",
                StatusCode = HttpStatusCode.OK,
            };

            return Ok(response);
        }


        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ServiceResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            if (!response.Success)
            {
                return BadRequest(response);
            }

            if(response.Data!.Count <= 0)
            {
                return NotFound("There is no user");
            }

            return Ok(response);
        }


        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="userDto"></param>
        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ServiceResponse<UserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] RegisterUserDto userDto)
        {
            var response = await _userService.Register(userDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Created("", response);
        }

    }
}
