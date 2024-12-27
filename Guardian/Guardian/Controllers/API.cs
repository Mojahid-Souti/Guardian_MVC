using Guardian.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guardian.Controllers
{
    [ApiController]
    [Route("api/users")] // Changed the base route to be more RESTful
    public class UsersController : ControllerBase
    {
        private readonly GuardianContext _context;

        public UsersController(GuardianContext context)
        {
            _context = context;
        }

        [HttpGet("test")] // Now will be api/users/test
        public IActionResult TestRoute()
        {
            return Ok("Route is working!");
        }

        [HttpPost] // Now will be api/users
        public IActionResult AddUser([FromBody] User user)
        {
            if (_context.Users.Any(u => u.EmailAddress == user.EmailAddress))
            {
                return Conflict("A user with this email already exists.");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPut("{id}")] // Endpoint: api/users/{id}
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update all fields
            user.FullName = updatedUser.FullName;
            user.EmailAddress = updatedUser.EmailAddress;
            user.Password = updatedUser.Password;

            _context.SaveChanges();
            return Ok(user);
        }


        [HttpDelete("{id}")] // Now will be api/users/{id}
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("User deleted successfully.");
        }

        [HttpGet("search")] // Now will be api/users/search
        public IActionResult SearchUser([FromQuery] int? id, [FromQuery] string? email)
        {
            if (id.HasValue)
            {
                var userById = _context.Users.FirstOrDefault(u => u.UserId == id.Value);
                if (userById != null)
                {
                    return Ok(userById);
                }
            }
            if (!string.IsNullOrEmpty(email))
            {
                var userByEmail = _context.Users.FirstOrDefault(u => u.EmailAddress == email);
                if (userByEmail != null)
                {
                    return Ok(userByEmail);
                }
            }
            return NotFound("User not found.");
        }
    }
}