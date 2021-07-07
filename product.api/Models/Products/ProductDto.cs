using System;
using System.ComponentModel.DataAnnotations;

namespace product.api.Models.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        [StringLength(250, ErrorMessage = "Name must be less than 250 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be a greater or equal to 0.0.")]
        public decimal Price { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Delivery price must be a greater or equal to 0.0.")]
        public decimal DeliveryPrice { get; set; }
    }
}