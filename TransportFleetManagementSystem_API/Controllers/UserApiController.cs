using Microsoft.AspNetCore.Mvc;
using TransportFleetManagementSystem.Model;
using TransportFleetManagementSystem.Repositories;
using TransportFleetManagementSystem.DTOs;
namespace TransportFleetManagementSystem_API.Controllers
    {
    [ApiController]
    [Route("api/users")]
    public class UserApiController : ControllerBase
        {
        private readonly IUserRepository _userRepository;

        public UserApiController(IUserRepository userRepository)
            {
            _userRepository = userRepository;
            }

        [HttpGet("username/{username}")]
        public ActionResult<UserDto> GetUserByUsername(string username)
            {
            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
                {
                return NotFound(); // 404 Not Found
                }

            // Map User to UserDto
            var userDto = new UserDto
                {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                // Map other properties as needed
                };

            return Ok(userDto); // 200 OK with UserDto
            }

        [HttpGet("email/{email}")]
        public ActionResult<UserDto> GetUserByEmail(string email)
            {
            var user = _userRepository.GetUserByEmail(email);

            if (user == null)
                {
                return NotFound();
                }

            var userDto = new UserDto
                {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                // Map other properties as needed
                };

            return Ok(userDto);
            }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
            {
            if (_userRepository.GetUserByUsername(user.Username) != null)
                {
                return BadRequest("Username already exists."); // 400 Bad Request
                }

            if (_userRepository.GetUserByEmail(user.Email) != null)
                {
                return BadRequest("Email already exists."); // 400 Bad Request
                }

            _userRepository.AddUser(user);
            return Ok("Registration successful."); // 200 OK
            }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
            {
            var user = _userRepository.GetUserByUsername(request.Username);

            if (user == null || user.Password != request.Password) // Remember to hash passwords!
                {
                return Unauthorized("Invalid username or password."); // 401 Unauthorized
                }

            // In a real application, return a JWT token here.
            return Ok("Login successful."); // 200 OK
            }

        // Example: Get all users (for demonstration purposes only; use with caution in production)
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAllUsers()
            {
            var users = _userRepository.GetAllUsers(); //You will need to create this method in the repository.

            if (users == null || !users.Any())
                {
                return NotFound();
                }

            var userDtos = users.Select(user => new UserDto
                {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                // Map other properties as needed
                });

            return Ok(userDtos);
            }

        //Example: Delete a user.
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
            {
            var user = _userRepository.GetUserById(id); //You will need to create this method in the repository.

            if (user == null)
                {
                return NotFound();
                }

            _userRepository.DeleteUser(id); //You will need to create this method in the repository.

            return NoContent(); // 204 No Content
            }
        }
    }
