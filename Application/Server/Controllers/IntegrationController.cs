#nullable enable
using System.Collections;
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
    public class IntegrationController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private const double RateVAT = 23;

        public IntegrationController(IApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable> GetProducts()
        {
            return GetProducts(null);
        }

        /// <summary>
        /// Get products per specific contractor or default
        /// </summary>
        /// <param name="nip">NIP of Contractor</param>
        /// <returns>List of products</returns>
        [HttpGet("{nip}")]
        public ActionResult<IEnumerable> GetProducts(string? nip)
        {
            var products = GetProductsByNip(nip);

            return Ok(products);
        }

        private IEnumerable GetProductsByNip(string? nip)
        {
            var query = _context.Products
                .Include(x => x.ProductType)
                .Include(x => x.ContractorProductPrices)
                .ThenInclude(x => x.Contractor)
                .ToList();

            var products = query.Select(x =>
            {
                var hasCustomPrice = !string.IsNullOrWhiteSpace(nip) && x.ContractorProductPrices.Any(
                    contractorProductPrice =>
                        contractorProductPrice.Contractor.ID.ToString().PadLeft(9, '0')
                        == nip.PadLeft(9, '0'));

                var price = (hasCustomPrice
                    ? x.ContractorProductPrices.First().Price
                    : x.Price) * (decimal) (1 + RateVAT / 100.0);

                return new
                {
                    x.ID,
                    x.Name,
                    Price = price,
                    UnitOfMeasure = "$",
                    RateVAT
                };
            });
            return products;
        }
    }
}