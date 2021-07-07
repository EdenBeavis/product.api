using AutoMapper;
using Bearded.Monads;
using MediatR;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.Products;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Features.Products.Handlers
{
    public class CreateNewProductRequest : IRequest<Option<Product>>
    {
        public ProductDto ProductDto { get; set; }
    }

    public class CreateNewProductRequestHandler : IRequestHandler<CreateNewProductRequest, Option<Product>>
    {
        private readonly ProductsDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateNewProductRequestHandler(ProductsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Option<Product>> Handle(CreateNewProductRequest request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.ProductDto);
            product.Id = Guid.NewGuid();

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}