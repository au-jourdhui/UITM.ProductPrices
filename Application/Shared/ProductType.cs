using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared
{
    public class ProductType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public ProductType()
        {
            Products = new List<Product>();
        }
    }
}
