using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace product.api.Infrastructure.Data.Entities
{
    public abstract class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}