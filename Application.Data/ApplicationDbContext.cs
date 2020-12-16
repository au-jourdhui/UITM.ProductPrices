using Application.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<ContractorProductPrice> ContractorProductPrices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var productTypes = new[]
            {
                new ProductType { ID = 1, Name = "Goods" },
                new ProductType { ID = 2, Name = "Service" }
            };
            modelBuilder.Entity<ProductType>().HasData(productTypes);

            var contractors = new[]
            {
                new Contractor { ID = 1, Name = "Amazon" },
                new Contractor { ID = 2, Name = "Google" },
                new Contractor { ID = 3, Name = "Orange" },
                new Contractor { ID = 4, Name = "MediaMarkt" },
            };
            modelBuilder.Entity<Contractor>().HasData(contractors);

            base.OnModelCreating(modelBuilder);
        }
    }
}
