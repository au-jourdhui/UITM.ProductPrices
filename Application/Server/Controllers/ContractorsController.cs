using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Shared;
using System.Linq;

namespace Application.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContractorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Read (pagination, select all)
        // size - amount products on page
        // page - number of page
        // GET: api/Contractors
        [HttpGet]
        [Route("{size?}/{page?}")]
        public async Task<ActionResult<IEnumerable<Contractor>>> GetContractors(int? size = null, int? page = null)
        {
            var query = _context.Contractors.AsQueryable();
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

        // GET: api/Contractor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contractor>> GetContractor(int id)
        {
            var contractor = await _context.Contractors.Include(x => x.ContractorProductPrices)
                                                       .ThenInclude(x => x.Product)
                                                       .FirstOrDefaultAsync(x => x.ID == id);

            if (contractor == null)
            {
                return NotFound();
            }

            return contractor;
        }

        // Read (get amount of all records)
        // GET: api/Contractors/Count
        [HttpGet]
        [Route("Count")]
        public async Task<ActionResult<int>> Count()
        {
            return await _context.Contractors.CountAsync();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Post()
        {
            // syncing contractors
            var result = await Task.FromResult(true);

            return result;
        }
    }
}
