using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public ProductType ProductType { get; set; }
        public ICollection<ContractorProductPrice> ContractorProductPrices { get; set; }

        public Product()
        {
            ContractorProductPrices = new List<ContractorProductPrice>();
        }
    }
}
