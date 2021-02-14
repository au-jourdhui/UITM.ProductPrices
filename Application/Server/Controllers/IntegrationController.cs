#nullable enable
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        /// <summary>
        /// Get contractors for integration
        /// </summary>
        /// <returns>List of contractors</returns>
        [HttpGet]
        [Route("contractors")]
        public IEnumerable GetContractors()
        {
            var contractors = _context.Contractors.Select(contractor => new
            {
                name = contractor.Name,
                nip = contractor.ID.ToString().PadLeft(9, '0')
            }).ToList();

            return contractors;
        }


        /// <summary>
        /// Get products without relation to specific contractor
        /// </summary>
        /// <returns>Prices per product by default</returns>
        [HttpGet]
        public ActionResult<IEnumerable> GetProducts()
        {
            var products = GetProductsByNip(null);

            return Ok(products);
        }

        /// <summary>
        /// Get products per specific contractor or default
        /// </summary>
        /// <param name="nip">NIP of Contractor</param>
        /// <returns>List of products</returns>
        [HttpGet("{nip}")]
        public ActionResult<IEnumerable> GetProducts(string? nip)
        {
            if (_context.Contractors.ToList().All(x => x.ID.ToString().PadLeft(9, '0') != nip))
            {
                return NotFound();
            }

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

            var products = query.Select(product =>
            {
                var hasCustomPrice = !string.IsNullOrWhiteSpace(nip) && product.ContractorProductPrices.Any(
                    contractorProductPrice =>
                        contractorProductPrice.Contractor.ID.ToString().PadLeft(9, '0') == nip);

                decimal netto;
                if (hasCustomPrice)
                {
                    netto = product.ContractorProductPrices.First().Price;
                }
                else
                {
                    netto = product.Price;
                }
                var brutto = netto * (decimal)(1 + RateVAT / 100.0);

                return new
                {
                    product.ID,
                    product.Name,
                    PriceBrutto = brutto,
                    PriceNetto = netto,
                    UnitOfMeasure = "$",
                    RateVAT
                };
            });
            return products;
        }
    }
}