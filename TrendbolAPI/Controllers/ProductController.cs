using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrendbolAPI.Data;
using TrendbolAPI.Models;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly TrendbolContext _context;

        public ProductController(TrendbolContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Category).Include(p => p.Seller).ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            return product;
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            // Kategori kontrolü
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryID == product.CategoryID);
            if (!categoryExists)
                return BadRequest("Invalid CategoryID: Category does not exist.");

            // Satıcı kontrolü
            var seller = await _context.Users.FindAsync(product.SellerID);
            if (seller == null || seller.Role.ToLower() != "seller")
                return BadRequest("Invalid SellerID: User does not exist or doesnt authorized to sell.");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductID)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductID == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
