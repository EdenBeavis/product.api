using AutoMapper;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.Products;

namespace product.api.Infrastructure.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}