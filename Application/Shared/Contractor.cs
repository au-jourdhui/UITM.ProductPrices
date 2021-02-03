using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared
{
    public class Contractor
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ExternalID { get; set; }

        public ICollection<ContractorProductPrice> ContractorProductPrices { get; set; }

        public Contractor()
        {
            ContractorProductPrices = new List<ContractorProductPrice>();
        }
    }
}
