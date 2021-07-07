using product.api.Models.ProductOptions;
using product.api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace product.api.test.Fakes.Builders
{
    // purely for helping setup data
    public class ProductDtoSetup : ProductDto
    {
        public IEnumerable<ProductOptionDto> ProductOptions { get; set; }
    }

    public static class ProductDtoSetupBuilder
    {
        public static ProductDtoSetup WithDefault(this ProductDtoSetup product)
        {
            product.Id = Guid.Empty;
            product.Name = "Test Name";
            product.Description = "Test Descr";
            product.Price = 100m;
            product.DeliveryPrice = 10m;
            product.ProductOptions = new List<ProductOptionDto>();

            return product;
        }

        public static ProductDtoSetup WithThreeDefaultProductOptions(this ProductDtoSetup product)
        {
            product.ProductOptions = new List<ProductOptionDto>
            {
                new ProductOptionDto().WithDefault().WithName("Product Option 1"),
                new ProductOptionDto().WithDefault().WithName("Product Option 2"),
                new ProductOptionDto().WithDefault().WithName("Product Option 3")
            };

            product.WithId(Guid.NewGuid());
            return product;
        }

        public static ProductDtoSetup WithProductOptions(this ProductDtoSetup product, params ProductOptionDto[] options)
        {
            product.ProductOptions = options.ToList();
            product.WithId(Guid.NewGuid());
            return product;
        }

        public static ProductDtoSetup WithId(this ProductDtoSetup product, Guid id)
        {
            product.Id = id;

            foreach (var option in product.ProductOptions)
                option.WithProductId(id);

            return product;
        }

        public static ProductDtoSetup WithName(this ProductDtoSetup product, string name)
        {
            product.Name = name;
            return product;
        }

        public static ProductDtoSetup WithDescription(this ProductDtoSetup product, string description)
        {
            product.Description = description;
            return product;
        }

        public static ProductDtoSetup WithPrice(this ProductDtoSetup product, decimal price)
        {
            product.Price = price;
            return product;
        }

        public static ProductDtoSetup WithDelieveryPrice(this ProductDtoSetup product, decimal delieveryPrice)
        {
            product.DeliveryPrice = delieveryPrice;
            return product;
        }
    }
}