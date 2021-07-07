using AutoMapper;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.ProductOptions;

namespace product.api.Infrastructure.MappingProfiles
{
    public class ProductOptionProfile : Profile
    {
        public ProductOptionProfile()
        {
            CreateMap<ProductOption, ProductOptionDto>();
            CreateMap<ProductOptionDto, ProductOption>();
        }
    }
}