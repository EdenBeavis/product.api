using System;

namespace product.api.Infrastructure.Data.Entities
{
    public class ProductOption : EntityBase
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Product Product { get; set; }
    }
}