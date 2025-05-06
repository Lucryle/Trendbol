using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Data;
using TrendbolAPI.Models;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly TrendbolContext _context;

        public UserController(TrendbolContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            return user;
        }

        // POST: api/user
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.UserID }, user); // 201 + data
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser == null)
                return NotFound();

            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            // varsa diğer alanlar...

            _context.SaveChanges();
            return Ok(existingUser);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
