using System.Collections.Generic;

namespace product.api.Infrastructure.Data.Entities
{
    public class Product : EntityBase
    {
        public Product()
        {
            Options = new List<ProductOption>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public virtual ICollection<ProductOption> Options { get; set; }
    }
}