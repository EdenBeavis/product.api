using product.api.Models.ProductOptions;
using System;

namespace product.api.test.Fakes.Builders
{
    public static class ProductOptionDtoBuilder
    {
        public static ProductOptionDto WithDefault(this ProductOptionDto productOption)
        {
            productOption.Id = Guid.Empty;
            productOption.ProductId = Guid.Empty;
            productOption.Name = "Test product option name";
            productOption.Description = "Test product option description";

            return productOption;
        }

        public static ProductOptionDto WithId(this ProductOptionDto productOption, Guid id)
        {
            productOption.Id = id;
            return productOption;
        }

        public static ProductOptionDto WithProductId(this ProductOptionDto productOption, Guid productId)
        {
            productOption.ProductId = productId;
            return productOption;
        }

        public static ProductOptionDto WithName(this ProductOptionDto productOption, string name)
        {
            productOption.Name = name;
            return productOption;
        }

        public static ProductOptionDto WithDescription(this ProductOptionDto productOption, string description)
        {
            productOption.Description = description;
            return productOption;
        }
    }
}