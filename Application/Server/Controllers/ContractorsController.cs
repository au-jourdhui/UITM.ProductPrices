using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Shared;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;

namespace Application.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorsController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        public ContractorsController(IApplicationDbContext context)
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
            const string url = "https://sakuta.bednarski.dev/api/contractors";
            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            var integrationContractors = JsonConvert.DeserializeObject<IEnumerable<ContractorIntegrationModel>>(json);

            var contractors = _context.Contractors.Include(x => x.ContractorProductPrices).ToList();

            var toDelete = contractors.Where(x => !integrationContractors.Any(ic => ic.ContractorId == x.ExternalID));
            foreach (var contractor in toDelete)
            {
                _context.ContractorProductPrices.RemoveRange(contractor.ContractorProductPrices);
                _context.Contractors.Remove(contractor);
            }

            var toUpdate = contractors.Except(toDelete)
                                      .Join(integrationContractors, x => x.ExternalID, x => x.ContractorId, (c, ic) => new { Contractor = c, Integration = ic });

            foreach (var contractor in toUpdate)
            {
                contractor.Contractor.Name = contractor.Integration.Name;
            }

            var toAdd = integrationContractors.Where(x => !contractors.Any(c => c.ExternalID == x.ContractorId))
                                              .Select(x => new Contractor { Name = x.Name, ExternalID = x.ContractorId });
            _context.Contractors.AddRange(toAdd);

            var result = _context.SaveChanges() > 0;
            return result;
        }

        class ContractorIntegrationModel
        {
            [JsonProperty("contractor_id")]
            public int ContractorId { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}
