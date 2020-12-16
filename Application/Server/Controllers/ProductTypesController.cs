using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Shared;

namespace Application.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public ProductTypesController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductTypes
        [HttpGet]
        [Route("{size?}/{page?}")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductTypes(int? size = null, int? page = null)
        {
            var query = _context.ProductTypes.Include(x => x.Products).AsQueryable();
            if (size.GetValueOrDefault() > 0)
            {
                if (page.GetValueOrDefault() > 0)
                {
                    query = query.Skip((page.Value - 1) * size.Value);
                }
                query = query.Take(size.Value);
            }

            var result = await query.OrderBy(x => x.ID).ToListAsync();
            return result;
        }

        // GET: api/ProductTypes/Count
        [HttpGet]
        [Route("Count")]
        public async Task<ActionResult<int>> Count()
        {
            return await _context.ProductTypes.CountAsync();
        }

        // GET: api/ProductTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);

            if (productType == null)
            {
                return NotFound();
            }

            return productType;
        }

        // PUT: api/ProductTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductType(int id, ProductType productType)
        {
            if (id != productType.ID)
            {
                return BadRequest();
            }

            _context.Entry(productType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductTypes
        [HttpPost]
        public async Task<ActionResult<ProductType>> PostProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductType", new { id = productType.ID }, productType);
        }

        // DELETE: api/ProductTypes/5
        [HttpDelete("{id}")]
        [NonAction]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            var productType = await _context.ProductTypes.Include(x => x.Products).FirstOrDefaultAsync(x => x.ID == id);
            if (productType == null)
            {
                return NotFound();
            }

            _context.Products.RemoveRange(productType.Products);
            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductTypes.Any(e => e.ID == id);
        }
    }
}
