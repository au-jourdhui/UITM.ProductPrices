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
    public class ContractorProductPricesController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public ContractorProductPricesController(IApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ContractorProductPrices
        [HttpGet]
        [Route("{size?}/{page?}")]
        public async Task<ActionResult<IEnumerable<ContractorProductPrice>>> GetContractorProductPrices(int? size = null, int? page = null)
        {
            var query = _context.ContractorProductPrices
                                .Include(x => x.Contractor)
                                .Include(x => x.Product)
                                .AsQueryable();

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
        // GET: api/ContractorProductPrices/Count
        [HttpGet]
        [Route("Count")]
        public async Task<ActionResult<int>> Count()
        {
            return await _context.Contractors.CountAsync();
        }

        // GET: api/ContractorProductPrices/Contractor/5
        [HttpGet("contractor/{id?}/{size?}/{page?}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetContractorPrices(int? id = null,
                                                                                  int? size = null,
                                                                                  int? page = null)
        {
            var query = _context.Products.Include(x => x.ProductType)
                                         .Include(x => x.ContractorProductPrices)
                                         .AsQueryable();

            if (size.GetValueOrDefault() > 0)
            {
                if (page.GetValueOrDefault() > 0)
                {
                    query = query.Skip((page.Value - 1) * size.Value);
                }
                query = query.Take(size.Value);
            }

            return await query.OrderBy(x => x.ID)
                              .Select(x => new Product
                              {
                                  // Product
                                  ID = x.ID,
                                  Created = x.Created,
                                  Name = x.Name,
                                  ProductType = x.ProductType,

                                  // ContractorProductPrice if exists, else use default price from Product
                                  Price = x.ContractorProductPrices.Any(x => x.ID == id)
                                          ? x.ContractorProductPrices.FirstOrDefault(x => x.ID == id).Price
                                          : x.Price,
                              })
                              .ToListAsync();
        }

        // GET: api/ContractorProductPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContractorProductPrice>> GetContractorProductPrice(int id)
        {
            var contractorProductPrice = await _context.ContractorProductPrices.FindAsync(id);

            if (contractorProductPrice == null)
            {
                return NotFound();
            }

            return contractorProductPrice;
        }

        // PUT: api/ContractorProductPrices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContractorProductPrice(int id, ContractorProductPrice contractorProductPrice)
        {
            if (id != contractorProductPrice.ID)
            {
                return BadRequest();
            }

            contractorProductPrice.Product = await _context.Products.FirstOrDefaultAsync(x => x.ID == contractorProductPrice.Product.ID);
            contractorProductPrice.Contractor = await _context.Contractors.FirstOrDefaultAsync(x => x.ID == contractorProductPrice.Contractor.ID);
            _context.Entry(contractorProductPrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractorProductPriceExists(id))
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

        // POST: api/ContractorProductPrices
        [HttpPost]
        public async Task<ActionResult<ContractorProductPrice>> PostContractorProductPrice(ContractorProductPrice contractorProductPrice)
        {
            contractorProductPrice.Product = await _context.Products.FirstOrDefaultAsync(x => x.ID == contractorProductPrice.Product.ID);
            contractorProductPrice.Contractor = await _context.Contractors.FirstOrDefaultAsync(x => x.ID == contractorProductPrice.Contractor.ID);
            _context.ContractorProductPrices.Add(contractorProductPrice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContractorProductPrice", new { id = contractorProductPrice.ID }, contractorProductPrice);
        }

        // DELETE: api/ContractorProductPrices/5
        [HttpDelete("{id}")]
        [NonAction]
        public async Task<IActionResult> DeleteContractorProductPrice(int id)
        {
            var contractorProductPrice = await _context.ContractorProductPrices.FindAsync(id);
            if (contractorProductPrice == null)
            {
                return NotFound();
            }

            _context.ContractorProductPrices.Remove(contractorProductPrice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContractorProductPriceExists(int id)
        {
            return _context.ContractorProductPrices.Any(e => e.ID == id);
        }
    }
}
