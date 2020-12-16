using System.ComponentModel.DataAnnotations;

namespace Application.Shared
{
    public class ContractorProductPrice
    {
        [Key]
        public int ID { get; set; }
        public Contractor Contractor { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }
    }
}
