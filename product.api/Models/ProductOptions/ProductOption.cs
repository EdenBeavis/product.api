using System;
using System.ComponentModel.DataAnnotations;

namespace product.api.Models.ProductOptions
{
    public class ProductOptionDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [StringLength(250, ErrorMessage = "Name must be less than 250 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }
    }
}