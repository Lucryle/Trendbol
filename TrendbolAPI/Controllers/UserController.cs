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

        // POST: api/user
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.UserID }, user);
        }
    }
}
