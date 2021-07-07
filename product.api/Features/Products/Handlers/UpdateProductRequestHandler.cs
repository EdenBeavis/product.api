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
    public class UpdateProductRequest : IRequest<Option<Product>>
    {
        public Guid Id { get; set; }
        public ProductDto ProductDto { get; set; }
    }

    public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest, Option<Product>>
    {
        private readonly IMediator _mediator;
        private readonly ProductsDbContext _dbContext;

        public UpdateProductRequestHandler(IMediator mediator, ProductsDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Option<Product>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var productToUpdateOptional = await _mediator.Send(new GetProductByIdRequest { Id = request.Id });

            if (!productToUpdateOptional)
                return Option<Product>.None;

            var updatedProduct = UpdateProduct(productToUpdateOptional, request.ProductDto);

            _dbContext.Products.Update(updatedProduct);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return updatedProduct;
        }

        private Product UpdateProduct(Option<Product> productOptional, ProductDto dto) =>
            productOptional.Map((product) =>
            {
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.DeliveryPrice = dto.DeliveryPrice;

                return product;
            }).ElseNew();
    }
}