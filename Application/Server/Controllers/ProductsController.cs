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
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Read (pagination, select all)
        // size - amount products on page
        // page - number of page
        // GET: api/Products
        [HttpGet]
        [Route("{size?}/{page?}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int? size = null, int? page = null)
        {
            var query = _context.Products.Include(x => x.ProductType).AsQueryable();
            if (size.GetValueOrDefault() > 0)
            {
                if (page.GetValueOrDefault() > 0)
                {
                    query = query.Skip((page.Value - 1) * size.Value);
                }
                query = query.Take(size.Value);
            }

            return await query.OrderBy(x => x.ID).ToListAsync();
        }

        // Read (get amount of all records)
        // GET: api/Products/Count
        [HttpGet]
        [Route("Count")]
        public async Task<ActionResult<int>> Count()
        {
            return await _context.Products.CountAsync();
        }


        // Read (search by parameters)
        // GET: api/Products/Search
        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult<SearchResult>> Search(int? productId = null,
                                                             int? productTypeId = null,
                                                             string productName = null,
                                                             int? size = null,
                                                             int? page = null)
        {
            var query = _context.Products.Include(x => x.ProductType).AsQueryable();
            if (productId.HasValue)
            {
                query = query.Where(x => x.ID == productId.Value);
            }
            if (productTypeId.HasValue)
            {
                query = query.Where(x => x.ProductType.ID == productTypeId.Value);
            }
            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(productName.ToUpper()));
            }

            int total = await query.CountAsync();
            if (size.GetValueOrDefault() > 0)
            {
                if (page.GetValueOrDefault() > 0)
                {
                    query = query.Skip((page.Value - 1) * size.Value);
                }
                query = query.Take(size.Value);
            }

            return new SearchResult(await query.OrderBy(x => x.ID).ToListAsync(), total);
        }

        // Read (search by string)
        // GET: api/Products/Search/{SearchString}
        [HttpGet]
        [Route("Search/{size?}/{page?}/{string?}")]
        public async Task<ActionResult<SearchResult>> Search(int? size = null,
                                                             int? page = null,
                                                             string @string = null)
        {
            var query = _context.Products.Include(x => x.ProductType).AsQueryable();

            var isInteger = int.TryParse(@string, out int integer);
            var isEmpty = string.IsNullOrWhiteSpace(@string);
            query = query.Where(x => (isInteger  && (x.ID == integer || x.ProductType.ID == integer))
                                  || (!isEmpty && x.Name.ToUpper().Contains(@string.ToUpper())));

            int total = await query.CountAsync();
            if (size.GetValueOrDefault() > 0)
            {
                if (page.GetValueOrDefault() > 0)
                {
                    query = query.Skip((page.Value - 1) * size.Value);
                }
                query = query.Take(size.Value);
            }

            return new SearchResult(await query.OrderBy(x => x.ID).ToListAsync(), total);
        }

        // Read (get product by id)
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(x => x.ProductType).FirstOrDefaultAsync(x => x.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // Update
        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            product.ProductType = await _context.ProductTypes.FirstOrDefaultAsync(x => x.ID == product.ProductType.ID);
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // Create
        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            product.ProductType = await _context.ProductTypes.FirstOrDefaultAsync(x => x.ID == product.ProductType.ID);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // Delete
        // DELETE: api/Products/5
        [NonAction]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #region Helpers
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
        #endregion
    }
    public record SearchResult(IEnumerable<Product> Products, int Total);
}
