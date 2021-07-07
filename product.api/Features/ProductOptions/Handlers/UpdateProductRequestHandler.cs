using Bearded.Monads;
using MediatR;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.ProductOptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace product.api.Features.ProductOptions.Handlers
{
    public class UpdateProductOptionRequest : IRequest<Option<ProductOption>>
    {
        public Guid Id { get; set; }
        public ProductOptionDto ProductOptionDto { get; set; }
    }

    public class UpdateProductOptionRequestHandler : IRequestHandler<UpdateProductOptionRequest, Option<ProductOption>>
    {
        private readonly IMediator _mediator;
        private readonly ProductsDbContext _dbContext;

        public UpdateProductOptionRequestHandler(IMediator mediator, ProductsDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
        }

        public async Task<Option<ProductOption>> Handle(UpdateProductOptionRequest request, CancellationToken cancellationToken)
        {
            var productOptionToUpdate = await _mediator.Send(new GetProductOptionByIdRequest { Id = request.Id });

            if (!productOptionToUpdate)
                return Option<ProductOption>.None;

            var updatedProductOption = UpdateProductOption(productOptionToUpdate, request.ProductOptionDto);

            _dbContext.ProductOptions.Update(updatedProductOption);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return updatedProductOption;
        }

        private ProductOption UpdateProductOption(Option<ProductOption> productOptional, ProductOptionDto dto) =>
            productOptional.Map((productOption) =>
            {
                productOption.Name = dto.Name;
                productOption.Description = dto.Description;

                return productOption;
            }).ElseNew();
    }
}